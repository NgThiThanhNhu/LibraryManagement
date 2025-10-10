using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.Notification
{
    public interface INotificationToUserService
    {
        Task<BaseResponse<NotificationToUserResponse>> CreateNotification(NotificationToUserRequest notificationToUserRequest);
        Task<BaseResponse<List<NotificationToUserResponse>>> GetAllNotifications();
        Task<BaseResponse<NotificationToUserResponse>> ReadNotification(Guid NotificationId);
    }
}
