using DoAnCuoiKy.Model.Entities.Notification;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using Microsoft.AspNetCore.SignalR;
namespace DoAnCuoiKy
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(NotificationToUserResponse response)
        {
            await Clients.User(response.UserId.ToString()).SendAsync("ReceiveMessage", response);
        }
    }
}
