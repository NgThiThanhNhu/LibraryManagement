using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Notification;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using DoAnCuoiKy.Service.IService.Notification;

namespace DoAnCuoiKy.Service.Notification
{
    public class NotificationToUserService : INotificationToUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHttpContextAccessor _httpContext;
        public NotificationToUserService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _hubContext = hubContext;
            _httpContext = httpContextAccessor;
        }


        public async Task<BaseResponse<NotificationToUserResponse>> CreateNotification(NotificationToUserRequest notificationToUserRequest)
        {
            BaseResponse<NotificationToUserResponse> response = new BaseResponse<NotificationToUserResponse>();
            Borrowing findBorrowing = await _context.borrowings.Include(x => x.Librarian).Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == notificationToUserRequest.BorrowingId);
            if (findBorrowing == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại phiếu mượn này";
                return response;
            }
            NotificationToUser notificationToUser = new NotificationToUser();
            notificationToUser.BorrowingId = notificationToUserRequest.BorrowingId;
            notificationToUser.UserId = notificationToUserRequest.UserId;
            notificationToUser.Title = notificationToUserRequest.Title;
            notificationToUser.Message = notificationToUserRequest.Message;
            notificationToUser.NotificationType = notificationToUserRequest.NotificationType;
            notificationToUser.CreateDate = notificationToUserRequest.CreatedAt;
            _context.notificationToUsers.Add(notificationToUser);
            await _context.SaveChangesAsync();
            NotificationToUserResponse notificationToUserResponse = new NotificationToUserResponse();
            notificationToUserResponse.NotiId = notificationToUser.Id;
            notificationToUserResponse.Title = notificationToUser.Title;
            notificationToUserResponse.Message = notificationToUser.Message;
            notificationToUserResponse.BorrowingId = notificationToUser.BorrowingId;
            notificationToUserResponse.BorrowingCode = findBorrowing.Code;
            notificationToUserResponse.UserId = notificationToUser.UserId;
            notificationToUserResponse.SendTime = notificationToUser.CreateDate;
            notificationToUserResponse.IsRead = notificationToUser.IsRead;
            await SendToClient(notificationToUserResponse);
            response.IsSuccess = true;
            response.message = "Gửi thông báo tới user thành công";
            response.data = notificationToUserResponse;
            return response;


        }

        public async Task<BaseResponse<List<NotificationToUserResponse>>> GetAllNotifications()
        {
            BaseResponse<List<NotificationToUserResponse>> response = new BaseResponse<List<NotificationToUserResponse>>();
            var CurrentUser = getCurrentUserId();
            List<NotificationToUserResponse> notificationToUserResponses = await _context.notificationToUsers.Include(x => x.borrowing).Where(x => x.IsDeleted == false && x.UserId == CurrentUser).Select(x => new NotificationToUserResponse
            {
                NotiId = x.Id,
                BorrowingId = x.BorrowingId,
                Title = x.Title,
                Message = x.Message,
                BorrowingCode = x.borrowing.Code,
                IsRead = x.IsRead,
                SendTime = x.CreateDate
            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy danh sách thông báo thành công";
            response.data = notificationToUserResponses;
            return response;
        }



        private async Task SendToClient(NotificationToUserResponse response)
        {
            await _hubContext.Clients.User(response.UserId.ToString())
                .SendAsync("ReceiveMessage", response);
        }
        private Guid getCurrentUserId()
        {
            var user = _httpContext.HttpContext.User;
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

        public async Task<BaseResponse<NotificationToUserResponse>> ReadNotification(Guid NotificationId)
        {
            BaseResponse<NotificationToUserResponse> response = new BaseResponse<NotificationToUserResponse>();
            NotificationToUser notification = await _context.notificationToUsers.Where(x => x.IsDeleted == false && x.IsRead == false).FirstOrDefaultAsync(x => x.Id == NotificationId);
            if (notification == null)
            {
                throw new Exception();
            }
            notification.IsRead = true;
            _context.notificationToUsers.Update(notification);
            await _context.SaveChangesAsync();
            NotificationToUserResponse notificationToUserResponse = new NotificationToUserResponse();
            notificationToUserResponse.NotiId = notification.Id;
            notificationToUserResponse.Title = notification.Title;
            notificationToUserResponse.SendTime = notification.CreateDate;
            notificationToUserResponse.Message = notification.Message;
            notification.IsRead = notification.IsRead;
            response.IsSuccess = true;
            response.message = "Đã đọc thông báo";
            response.data = notificationToUserResponse;
            return response;
        }
    }
}
