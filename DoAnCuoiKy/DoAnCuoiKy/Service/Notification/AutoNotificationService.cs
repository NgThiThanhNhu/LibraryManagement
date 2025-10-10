using System.Net.Mail;
using System.Net;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Service.IService.Notification;
using Hangfire;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Notification;
using Azure;
using DoAnCuoiKy.Model.Entities.Usermanage;

namespace DoAnCuoiKy.Service.Notification
{
    public class AutoNotificationService : IAutoNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundJobClient _jobClient;
        public AutoNotificationService(ApplicationDbContext context, IBackgroundJobClient backgroundJobClient)
        {
            _context = context;
            _jobClient = backgroundJobClient;
        }
        public void CreateNotification(string body, string title, string email, Guid borrowingId)
        {
            BaseResponse<NotificationToUserResponse> response = new BaseResponse<NotificationToUserResponse>();
            Borrowing findBorrowing = _context.borrowings.Include(x => x.Librarian).FirstOrDefault(x => x.IsDeleted == false && x.isReminded == false && x.Librarian.Email == email && x.Id == borrowingId);
            if (findBorrowing == null) return;
            NotificationToUser newNotification = new NotificationToUser();
            newNotification.Id = Guid.NewGuid();
            newNotification.CreateDate = DateTime.Now;
            newNotification.CreateUser = "admin";
            newNotification.UserId = findBorrowing.UserId.Value;
            newNotification.Title = title;
            newNotification.Message = body;
            newNotification.NotificationType = NotificationType.Reminder;
            newNotification.BorrowingId = findBorrowing.Id.Value;
            _context.notificationToUsers.Add(newNotification);
            _context.SaveChanges();
            

        }
        public NotificationToUserResponse ResponseNotification(Guid BorrowingId)
        {
            NotificationToUser findNotification = _context.notificationToUsers.Include(x=>x.borrowing).FirstOrDefault(x => x.BorrowingId == BorrowingId && x.NotificationType == NotificationType.Reminder);
            NotificationToUserResponse notification = new NotificationToUserResponse();
            notification.NotiId = findNotification.Id;
            notification.SendTime = findNotification.CreateDate;
            notification.Title = findNotification.Title;
            notification.Message = findNotification.Message;
            notification.isReminded = findNotification.borrowing.isReminded;
            notification.IsRead = findNotification.IsRead;
            notification.BorrowingCode = findNotification.borrowing.Code;
            return notification;
        }
        public string htmlEmail(string email, Guid borrowingId)
        {
            return "Xin chào " + email + ", phiếu mượn sách " + borrowingId + " của bạn đến hạn trả sách vào ngày mai.";
        }
        [Queue("email")]
        public bool sendEmail(Guid borrowingId, string email)
        {
            string body = htmlEmail(email, borrowingId);
            string title = "Nhắc hạn phiếu mượn....";
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = "h09052003n@gmail.com",
                        Password = "debskzkkbtkmqdfe"
                    };
                }
                MailAddress fromAddress = new MailAddress("h09052003n@gmail.com", "UTC2Library");
                message.From = fromAddress;
                message.To.Add(email);
                message.Subject = title;
                message.IsBodyHtml = true;
                message.Body = body;
                smtp.Send(message);
                CreateNotification(body, title, email, borrowingId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
       
        public async Task<BaseResponse<List<NotificationToUserResponse>>> SendNotificationToEmail()
        {
            BaseResponse<List<NotificationToUserResponse>> response = new BaseResponse<List<NotificationToUserResponse>>();
            List<NotificationToUserResponse> notificationToUserResponses = new List<NotificationToUserResponse>();
            var tomorrow = DateTime.Now.Date.AddDays(1);
            List<Borrowing> borrowings = await _context.borrowings.Where(x => x.IsDeleted == false && x.isReminded == false && x.BorrowingStatus == Model.Enum.InformationLibrary.BorrowingStatus.Borrowing && x.DueDate.Date == tomorrow ).ToListAsync();
            if (!borrowings.Any())
            {
                response.IsSuccess = false;
                response.message = "Hiện chưa có phiếu mượn cần nhắc";
                return response;
            }
            foreach (var item in borrowings)
            {
                Librarian librarian = await _context.librarians.FirstOrDefaultAsync(x => x.Id == item.UserId);
                _jobClient.Enqueue(() => sendEmail(item.Id.Value, librarian.Email));

                item.isReminded = true;
                _context.borrowings.Update(item);
                notificationToUserResponses.Add(ResponseNotification(item.Id.Value));
            }
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Gửi thông báo thành công";
            response.data = notificationToUserResponses;
            return response;

        }
    }
}
