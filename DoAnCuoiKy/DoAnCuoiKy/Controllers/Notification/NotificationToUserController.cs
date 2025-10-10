using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.Notification;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationToUserController : ControllerBase
    {
        private readonly INotificationToUserService _notificationToUserService;
        public NotificationToUserController(INotificationToUserService notificationToUserService)
        {
            _notificationToUserService = notificationToUserService;
        }
        [HttpPost("createNotification")]
        public async Task<BaseResponse<NotificationToUserResponse>> CreateNotification(NotificationToUserRequest notificationToUserRequest)
        {
            BaseResponse<NotificationToUserResponse> response = await _notificationToUserService.CreateNotification(notificationToUserRequest);
            return response;
        }
        [HttpGet("getAllNotifications")]
        public async Task<BaseResponse<List<NotificationToUserResponse>>> GetAllNotifications()
        {
            BaseResponse<List<NotificationToUserResponse>> response = await _notificationToUserService.GetAllNotifications();
            return response;
        }
        [HttpPost("readnotification/{NotificationId}")]
        public async Task<BaseResponse<NotificationToUserResponse>> ReadNotification(Guid NotificationId)
        {
            BaseResponse<NotificationToUserResponse> response = await _notificationToUserService.ReadNotification(NotificationId);
            return response;
        }
    }
}
