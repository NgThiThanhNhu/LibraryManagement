using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.Notification
{
    public interface IAutoNotificationService
    {
        Task<BaseResponse<List<NotificationToUserResponse>>> SendNotificationToEmail();
    }
}
