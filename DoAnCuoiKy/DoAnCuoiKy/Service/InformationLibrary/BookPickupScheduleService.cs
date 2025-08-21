using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using static DoAnCuoiKy.Mapper.BorrowingProfile;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookPickupScheduleService : IBookPickupScheduleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBorrowingService _borrowingService;
        private readonly IMapper _mapper;
        private readonly INotificationToUserService _notificationToUserService;
        public BookPickupScheduleService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IBorrowingService borrowingService, IMapper mapper, INotificationToUserService notificationToUserService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _borrowingService = borrowingService;
            _mapper = mapper;
            _notificationToUserService = notificationToUserService;
        }
        public async Task<BaseResponse<BookPickupScheduleResponse>> CreateSchedule(BookPickupScheduleRequest bookPickupScheduleRequest)
        {
            BaseResponse<BookPickupScheduleResponse> response = new BaseResponse<BookPickupScheduleResponse>();
            Borrowing findBorrowing = await _context.borrowings.Include(x=>x.Librarian).Where(x => x.IsDeleted == false && x.BorrowingStatus == Model.Enum.InformationLibrary.BorrowingStatus.Approved && x.isScheduled == false).FirstOrDefaultAsync(x=>x.Id == bookPickupScheduleRequest.BorrowingId);
            if (findBorrowing == null || findBorrowing.BorrowingStatus != Model.Enum.InformationLibrary.BorrowingStatus.Approved) 
            { 
                response.IsSuccess = false;
                response.message = "Không tồn tại phiếu mượn này hoặc phiếu mượn chưa được duyệt hoặc đã được xếp lịch.";
                return response;
            }
            DateTime approvedDate = findBorrowing.UpdateDate ?? DateTime.Now;

            if (bookPickupScheduleRequest.scheduledPickupDate < approvedDate ||
                bookPickupScheduleRequest.scheduledPickupDate > approvedDate.AddDays(3) ||
                bookPickupScheduleRequest.scheduledPickupDate > findBorrowing.DueDate || bookPickupScheduleRequest.scheduledPickupDate > bookPickupScheduleRequest.ExpiredPickupDate)
            {
                response.IsSuccess = false;
                response.message = "Ngày hẹn không hợp lệ. Phải trong vòng 3 ngày kể từ ngày duyệt và trước ngày hết hạn mượn sách.";
                return response;
            }
            BookPickupSchedule newSchedule = new BookPickupSchedule();
            newSchedule.Id = Guid.NewGuid();
            newSchedule.BorrowingId = bookPickupScheduleRequest.BorrowingId;
            newSchedule.CreateUser = getCurrentName();
            newSchedule.CreateDate = DateTime.Now;
            newSchedule.ScheduledPickupDate = bookPickupScheduleRequest.scheduledPickupDate;
            newSchedule.ExpiredPickupDate = bookPickupScheduleRequest.ExpiredPickupDate;
            _context.bookPickupSchedules.Add(newSchedule);
            var replyRequest = new ReplyBorrowingRequest
            {
                borrowingStatus = Model.Enum.InformationLibrary.BorrowingStatus.Scheduled
            };
            await _borrowingService.UpdateBorrowing(findBorrowing.Id.Value, replyRequest);
            findBorrowing.isScheduled = true;
            _context.borrowings.Update(findBorrowing);
            await _context.SaveChangesAsync();
            BookPickupScheduleResponse bookPickupScheduleResponse = new BookPickupScheduleResponse();
            bookPickupScheduleResponse.Id = newSchedule.Id;
            bookPickupScheduleResponse.BorrowingId = newSchedule.BorrowingId;
            bookPickupScheduleResponse.ScheduledPickupDate = newSchedule.ScheduledPickupDate.Value;
            bookPickupScheduleResponse.ExpiredPickupDate = newSchedule.ExpiredPickupDate;
            bookPickupScheduleResponse.IsPickedUp = newSchedule.IsPickedUp;
            bookPickupScheduleResponse.LibrarianName = newSchedule.CreateUser;
            bookPickupScheduleResponse.UserName = findBorrowing.CreateUser;
            bookPickupScheduleResponse.isScheduled = findBorrowing.isScheduled;
            response.IsSuccess = true;
            response.message = "Tạo lịch hẹn thành công";
            response.data = bookPickupScheduleResponse;
            return response;

        }

        public async Task<BaseResponse<BookPickupScheduleResponse>> DeleteScheduled(Guid borrowingId)
        {
            var replyRequest = new ReplyBorrowingRequest
            {
                borrowingStatus = Model.Enum.InformationLibrary.BorrowingStatus.Reject
            };
            BaseResponse<BookPickupScheduleResponse> response = new BaseResponse<BookPickupScheduleResponse>();
            BookPickupSchedule findBookPickupSchedule = await _context.bookPickupSchedules.Include(x=>x.borrowing).FirstOrDefaultAsync(x => x.IsDeleted == false && x.BorrowingId == borrowingId && x.IsPickedUp == false);
            if (findBookPickupSchedule == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại lịch hẹn theo Id của phiếu mượn này.";
                return response;
            }
            if (findBookPickupSchedule.ExpiredPickupDate > DateTime.Now)
            {
                response.IsSuccess = false;
                response.message = "Lịch hẹn vẫn còn hạn, không thể hủy tự động.";
                return response;
            }
            NotificationToUserRequest notificationToUserRequest = new NotificationToUserRequest();
            notificationToUserRequest.BorrowingId = findBookPickupSchedule.BorrowingId;
            notificationToUserRequest.UserId = findBookPickupSchedule.borrowing.UserId.Value;
            notificationToUserRequest.Title = "Hủy phiếu mượn";
            notificationToUserRequest.Message = $"Đọc giả không đến nhận đơn sách, phiếu mượn này bị hủy";
            notificationToUserRequest.NotificationType = _mapper.Map<NotificationType>(replyRequest);
            notificationToUserRequest.CreatedAt = DateTime.Now;
            await _notificationToUserService.CreateNotification(notificationToUserRequest);
            findBookPickupSchedule.IsDeleted = true;
            _context.bookPickupSchedules.Update(findBookPickupSchedule);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Đọc giả không đến nhận đơn sách, phiếu mượn này bị hủy";
            return response;
        }

        public async Task<BaseResponse<BookPickupScheduleResponse>> GetScheduledByBorrowingId(Guid borrowingId)
        {
            BaseResponse<BookPickupScheduleResponse> response = new BaseResponse<BookPickupScheduleResponse>();
            BookPickupSchedule findSchedule = await _context.bookPickupSchedules.Include(x=>x.borrowing).FirstOrDefaultAsync(x => x.IsDeleted == false && x.BorrowingId == borrowingId);
            if (findSchedule == null) 
            {
                response.IsSuccess = false;
                response.message = "Chưa lập lịch hẹn";
                return response;
            }
            BookPickupScheduleResponse bookPickupScheduleResponse = new BookPickupScheduleResponse();
            bookPickupScheduleResponse.ScheduledPickupDate = findSchedule.ScheduledPickupDate.Value;
            bookPickupScheduleResponse.ExpiredPickupDate = findSchedule.ExpiredPickupDate.Value;
            bookPickupScheduleResponse.LibrarianName = findSchedule.CreateUser;
            bookPickupScheduleResponse.UserName = findSchedule.borrowing.CreateUser;
            response.IsSuccess = true;
            response.message = "Lấy thông tin lịch hẹn thành công";
            response.data = bookPickupScheduleResponse;
            return response;

        }
        private Guid getCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
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
        private string getCurrentName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
