using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Entities.Notification;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.EntityFrameworkCore;
using static DoAnCuoiKy.Mapper.BorrowingProfile;
using Microsoft.AspNetCore.SignalR;



namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BorrowingService : IBorrowingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        
       
        public BorrowingService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _contextAccessor = httpContextAccessor;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        private void RemoveBookItemFromList(Guid currentBookItemId)
        {
            var currentUser = getCurrentUserId();
            BookCartItem bookCartItem = _context.bookCartItems.Include(x => x.BookItem).FirstOrDefault(x => x.IsDeleted == false && x.UserId == currentUser && x.BookItemId == currentBookItemId);
            if (bookCartItem.BookItem.BookStatus == Model.Enum.InformationLibrary.BookStatus.Borrowed && bookCartItem != null)
            {
                bookCartItem.IsDeleted = true;
                _context.bookCartItems.Update(bookCartItem);
            }
            else
            {
                throw new Exception("Không tìm thấy sách cần xóa của user này");
            }
        }
        public async Task<BaseResponse<BorrowingResponse>> CreateBorrowing(BorrowingRequest borrowingRequest)
        {
            BaseResponse<BorrowingResponse> response = new BaseResponse<BorrowingResponse>();
            Borrowing newBorrowing = new Borrowing();
            newBorrowing.Id= Guid.NewGuid();
            newBorrowing.Code = GenerateBorrowingCode();
            newBorrowing.CreateDate = DateTime.Now;
            newBorrowing.Duration = borrowingRequest.Duration;
            newBorrowing.BorrowingStatus = BorrowingStatus.Wait;
            newBorrowing.UserId = getCurrentUserId();
            Librarian librarian = await _context.librarians.Where(x => x.IsDeleted == false && x.Id == newBorrowing.UserId).FirstOrDefaultAsync();
            if (librarian == null)
            {
                response.IsSuccess = false;
                response.message = "Lỗi chưa đăng nhập, không có user";
                return response;
            }
            newBorrowing.CreateUser = librarian.Name;
            newBorrowing.DueDate = newBorrowing.CreateDate.AddDays(newBorrowing.Duration);
            await _context.borrowings.AddAsync(newBorrowing);
            await _context.SaveChangesAsync();
            foreach (var item in borrowingRequest.BookiTemIds)
            {
                BorrowingDetail detail = new BorrowingDetail();
                detail.Id = Guid.NewGuid();
                detail.BookItemId = item;
                detail.BorrowingId = newBorrowing.Id;
                detail.ReturnedDate = newBorrowing.DueDate;
                detail.CreateDate = DateTime.Now;
                detail.bookStatus = BookStatus.Borrowed;
                await _context.borrowingDetails.AddAsync(detail);
                RemoveBookItemFromList(item);
                await _context.SaveChangesAsync();
            }
            BorrowingResponse borrowingResponse = _mapper.Map<BorrowingResponse>(newBorrowing);
            borrowingResponse.UserName = librarian.Name;
            response.IsSuccess = true;
            response.message = "Lập phiếu mượn thành công, đợi admin duyệt";
            response.data = borrowingResponse;
            return response;
        }

        public async Task<BaseResponse<List<BorrowingUserResponse>>> GetAllUserBorrowing()
        {
            BaseResponse<List<BorrowingUserResponse>> response = new BaseResponse<List<BorrowingUserResponse>>();
            var currentUser = getCurrentUserId();
            List<Borrowing> borrowingResponses = await _context.borrowings.Include(x=>x.Librarian).Where(x=>x.IsDeleted == false && x.UserId == currentUser).ToListAsync();
         
            List<BorrowingUserResponse> borrowingUsers = _mapper.Map<List<BorrowingUserResponse>>(borrowingResponses);
            response.IsSuccess = true;
            response.message = "tải thành công danh sách phiếu mượn";
            response.data = borrowingUsers;
            return response;
        }
        private void UpdateScheduleAsync(Guid BorrowingId)
        {
            BookPickupSchedule findBookPickupSchedule = _context.bookPickupSchedules.FirstOrDefault(x => x.IsDeleted == false && !x.IsPickedUp && x.BorrowingId == BorrowingId);
            findBookPickupSchedule.IsPickedUp = true;
            findBookPickupSchedule.UpdateDate = DateTime.Now;
            findBookPickupSchedule.UpdateUser = getCurrentName();
            _context.bookPickupSchedules.Update(findBookPickupSchedule);
        }
        private void CreateBookExportTransaction(Guid IdBorrowingDetail)
        {
            BorrowingDetail borrowingDetail = _context.borrowingDetails.Include(x=>x.borrowing).FirstOrDefault(x => x.IsDeleted == false && x.Id == IdBorrowingDetail);
            if (borrowingDetail == null)
            {
                throw new Exception("Chi tiết phiếu mượn này không tồn tại");
            }
            BookExportTransaction bookExportTransaction = new BookExportTransaction();
            bookExportTransaction.Id = Guid.NewGuid();
            bookExportTransaction.BorrowingDetailId = IdBorrowingDetail;
            bookExportTransaction.CreateUser = getCurrentName();
            bookExportTransaction.CreateDate = DateTime.Now;
            if (borrowingDetail.borrowing.BorrowingStatus == Model.Enum.InformationLibrary.BorrowingStatus.Borrowing)
            {
                bookExportTransaction.ExportReason = Model.Enum.InformationLibrary.Kho.ExportReason.Borrow;
                bookExportTransaction.TransactionType = Model.Enum.InformationLibrary.Kho.TransactionType.Export;
            }
            else if (borrowingDetail.borrowing.BorrowingStatus == Model.Enum.InformationLibrary.BorrowingStatus.Returned)
            {
                bookExportTransaction.ExportReason = null;
                bookExportTransaction.TransactionType = Model.Enum.InformationLibrary.Kho.TransactionType.ReturnToStock;
            }
            _context.bookExportTransactions.Add(bookExportTransaction);
        }
        private async Task SendToClient(NotificationToUserResponse response)
        {
            await _hubContext.Clients.User(response.UserId.ToString())
                .SendAsync("ReceiveMessage", response);
        }
        private void CreateNotification(NotificationToUserRequest notificationToUserRequest)
        {
            Borrowing findBorrowing = _context.borrowings.Include(x => x.Librarian).FirstOrDefault(x => x.Id == notificationToUserRequest.BorrowingId);
            if (findBorrowing == null)
            {
                throw new Exception("Phiếu mượn này không tồn tại");
            }
            NotificationToUser notificationToUser = new NotificationToUser();
            notificationToUser.BorrowingId = notificationToUserRequest.BorrowingId;
            notificationToUser.UserId = notificationToUserRequest.UserId;
            notificationToUser.Title = notificationToUserRequest.Title;
            notificationToUser.Message = notificationToUserRequest.Message;
            notificationToUser.NotificationType = notificationToUserRequest.NotificationType;
            notificationToUser.CreateDate = notificationToUserRequest.CreatedAt;
            _context.notificationToUsers.Add(notificationToUser);
            NotificationToUserResponse notificationToUserResponse = new NotificationToUserResponse();
            notificationToUserResponse.NotiId = notificationToUser.Id;
            notificationToUserResponse.Title = notificationToUser.Title;
            notificationToUserResponse.Message = notificationToUser.Message;
            notificationToUserResponse.BorrowingId = notificationToUser.BorrowingId;
            notificationToUserResponse.BorrowingCode = findBorrowing.Code;
            notificationToUserResponse.UserId = notificationToUser.UserId;
            notificationToUserResponse.SendTime = notificationToUser.CreateDate;
            notificationToUserResponse.IsRead = notificationToUser.IsRead;
            SendToClient(notificationToUserResponse);
        }
        public async Task<BaseResponse<ReplyBorrowingResponse>> UpdateBorrowing(Guid id, ReplyBorrowingRequest replyBorrowingRequest)
        {
            BaseResponse<ReplyBorrowingResponse> response = new BaseResponse<ReplyBorrowingResponse>();
            Borrowing borrowingUpdate = await _context.borrowings.Include(x=>x.BookPickupSchedule).Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x=>x.Id == id);
            if (borrowingUpdate == null)
            {
                response.IsSuccess=false;
                response.message = "Không tồn tại phiếu mượn";
                return response;
            }
            if (!IsValidTransition(borrowingUpdate.BorrowingStatus.Value, replyBorrowingRequest.borrowingStatus))
            {
                response.IsSuccess=false;
                response.message = $"Không thể chuyển trạng thái từ {borrowingUpdate.BorrowingStatus} sang {replyBorrowingRequest.borrowingStatus}";
                return response;
            }
            if (((DateTime.Now - borrowingUpdate.CreateDate).TotalHours > 24) && borrowingUpdate.BorrowingStatus == BorrowingStatus.Wait)
            {
                response.IsSuccess = false;
                response.message = "Phiếu mượn đã quá thời gian chờ duyệt (24 giờ).";
                return response;
            }
            if (borrowingUpdate.BorrowingStatus == BorrowingStatus.Scheduled && borrowingUpdate.BookPickupSchedule.ExpiredPickupDate < DateTime.Now)
            {
                response.IsSuccess = false;
                response.message = "Phiếu mượn bị hủy vì bạn không đến nhận đơn mượn này";
                return response;
            }
            if (replyBorrowingRequest.borrowingStatus == BorrowingStatus.Overdue && borrowingUpdate.DueDate > DateTime.Now)
            {
                response.IsSuccess = false;
                response.message = "Phiếu mượn chưa tới hạn để đổi thành trạng thái quá hạn, vui lòng kiểm tra lại duedate";
                return response;
            }
            
            var oldStatus = borrowingUpdate.BorrowingStatus;
            borrowingUpdate.BorrowingStatus = replyBorrowingRequest.borrowingStatus;
            //hủy phiếu
            if (borrowingUpdate.BorrowingStatus == BorrowingStatus.Reject)
                borrowingUpdate.IsDeleted = true;
            using(var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                if (borrowingUpdate.BorrowingStatus == BorrowingStatus.Borrowing)
                {
                    List<BorrowingDetail> findBorrowingDetails = await _context.borrowingDetails.Where(x => x.IsDeleted == false && x.BorrowingId == id).ToListAsync();
                    foreach (var item in findBorrowingDetails)
                    {
                            CreateBookExportTransaction(item.Id.Value);
                    }
                    UpdateScheduleAsync(id);
                }
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                }
                catch
                {
                    await trans.RollbackAsync();
                }

            }    
            
            if (oldStatus != replyBorrowingRequest.borrowingStatus)
            {
                NotificationToUserRequest notificationToUserRequest = new NotificationToUserRequest();
                notificationToUserRequest.BorrowingId = borrowingUpdate.Id.Value;
                notificationToUserRequest.UserId = borrowingUpdate.UserId.Value;
                notificationToUserRequest.Title = "Cập nhật trạng thái phiếu mượn";
                notificationToUserRequest.Message = $"Trạng Thái phiếu mượn được cập nhật thành {BorrowingStatusHelper.GetStatusDescription(replyBorrowingRequest.borrowingStatus)}";
                notificationToUserRequest.NotificationType = _mapper.Map<NotificationType>(replyBorrowingRequest.borrowingStatus);
                notificationToUserRequest.CreatedAt = DateTime.Now;
                CreateNotification(notificationToUserRequest);
            }
            borrowingUpdate.LibrarianId = getCurrentUserId();
            borrowingUpdate.UpdateUser = getCurrentName();
            borrowingUpdate.UpdateDate = DateTime.Now;
            _context.borrowings.Update(borrowingUpdate);
            await _context.SaveChangesAsync();

            ReplyBorrowingResponse replyBorrowingResponse = _mapper.Map<ReplyBorrowingResponse>(borrowingUpdate);

            response.IsSuccess = true;
            response.message = $"Đã phê duyệt phiếu mượn sang {BorrowingStatusHelper.GetStatusDescription(replyBorrowingRequest.borrowingStatus)}";
            response.data = replyBorrowingResponse;
            return response;
        }

        public string GenerateBorrowingCode()
        {
            var today = DateTime.Now;

            string day = today.Day.ToString("D2");         // 17
            string month = today.Month.ToString("D2");     // 07
            string year = (today.Year % 10).ToString();    // 2025 -> 5

            string datePart = $"{day}{month}{year}";       // 17075
            string guidPart = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper(); // ABC123

            return $"#{datePart}-{guidPart}";
        }

        private string getCurrentName()
        {
            return _contextAccessor.HttpContext.User.Identity.Name;
        }
        private Guid getCurrentUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not Authentiated");
            }
            var userId = user.FindFirst(JwtRegisteredClaimNames.Sub) ?? user.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("userId không tồn tại");
            }
            return Guid.Parse(userId.Value);
        }
        
        public async Task<BaseResponse<List<BorrowingResponse>>> GetAllAdminBorrowing()
        {
           
            BaseResponse<List<BorrowingResponse>> response = new BaseResponse<List<BorrowingResponse>>();
            List<Borrowing> borrowingResponse = await _context.borrowings.Include(x=>x.Librarian).Include(x=>x.borrowingDetails).Where(x => x.IsDeleted == false).ToListAsync();
            
            List<BorrowingResponse> borrowingResponses = _mapper.Map<List<BorrowingResponse>>(borrowingResponse);
            response.IsSuccess = true;
            response.message = "tải danh sách thành công";
            response.data = borrowingResponses;
            return response;
        }

        private bool IsValidTransition(BorrowingStatus current, BorrowingStatus next)
        {
            return current switch
            {
                BorrowingStatus.Wait => next == BorrowingStatus.Approved || next == BorrowingStatus.Reject,
                BorrowingStatus.Approved => next == BorrowingStatus.Scheduled || next == BorrowingStatus.Reject,
                BorrowingStatus.Scheduled => next == BorrowingStatus.Borrowing || next == BorrowingStatus.Reject,
                BorrowingStatus.Borrowing => next == BorrowingStatus.Returned || next == BorrowingStatus.Overdue,
                _ => false
            };
        }

        
    }
}
