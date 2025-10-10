using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.Notification;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoNotificationController : ControllerBase
    {
        private readonly IAutoNotificationService _autoNotificationService;
        public AutoNotificationController(IAutoNotificationService autoNotificationService)
        {
            _autoNotificationService = autoNotificationService;
        }
        [HttpGet]
        public async Task<BaseResponse<List<NotificationToUserResponse>>> SendNotificationToEmail()
        {
            BaseResponse<List<NotificationToUserResponse>> response = await _autoNotificationService.SendNotificationToEmail();
            return response;
        }
    }
}
