using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Security.Claims;
using AutoMapper;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static DoAnCuoiKy.Mapper.BorrowingProfile;


namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BorrowingService : IBorrowingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBookCartItemService _bookCartItemService;
        private readonly INotificationToUserService _notificationToUserService;
        private readonly IBookExportTransactionService _bookExportTransactionService;
        private readonly IMapper _mapper;
       
        public BorrowingService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IBookCartItemService bookCartItemService, IMapper mapper, INotificationToUserService notificationToUserService, IBookExportTransactionService bookExportTransactionService)
        {
            _context = context;
            _contextAccessor = httpContextAccessor;
            _bookCartItemService = bookCartItemService;
            _mapper = mapper;
            _notificationToUserService = notificationToUserService;
            _bookExportTransactionService = bookExportTransactionService;
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
                await _context.SaveChangesAsync();

                BaseResponse<BookCartItemResponse> removeBookItemFromList = await _bookCartItemService.DeleteBookCartItem(item);
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
            
            if (borrowingUpdate.BorrowingStatus == BorrowingStatus.Borrowing)
            {
                List<BorrowingDetail> findBorrowingDetails = await _context.borrowingDetails.Where(x=>x.IsDeleted == false && x.BorrowingId == id).ToListAsync();
                foreach (var item in findBorrowingDetails)
                {
                    await _bookExportTransactionService.CreateBookExportTransaction(item.Id.Value);
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
                 await _notificationToUserService.CreateNotification(notificationToUserRequest);
            }

            borrowingUpdate.LibrarianId = getCurrentUserId();
            borrowingUpdate.UpdateUser = getCurrentName();
            borrowingUpdate.UpdateDate = DateTime.Now;
            _context.borrowings.Update(borrowingUpdate);
            _context.SaveChanges();

            ReplyBorrowingResponse replyBorrowingResponse = _mapper.Map<ReplyBorrowingResponse>(borrowingUpdate);

            response.IsSuccess = true;
            response.message = "Đã phê duyệt phiếu mượn";
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
                BorrowingStatus.Approved => next == BorrowingStatus.Scheduled,
                BorrowingStatus.Scheduled => next == BorrowingStatus.Borrowing || next == BorrowingStatus.Reject,
                BorrowingStatus.Borrowing => next == BorrowingStatus.Returned || next == BorrowingStatus.Overdue,
                _ => false
            };
        }

       
    }
}
