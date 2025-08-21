using AutoMapper;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Mapper
{
    public class BorrowingProfile : Profile
    {
        public BorrowingProfile()
        {
            CreateMap<Borrowing, BorrowingUserResponse>()
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => BorrowingStatusHelper.GetStatusDescription(src.BorrowingStatus.Value)))
                .ForMember(dest => dest.ReplyDate,
                opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("dd/MM/yyyy") : "Chưa phản hồi"));
               
            CreateMap<Borrowing, BorrowingResponse>()
                 .ForMember(dest => dest.Status,
                 opt => opt.MapFrom(src => BorrowingStatusHelper.GetStatusDescription(src.BorrowingStatus.Value)))
                 .ForMember(dest => dest.ReplyDate,
                 opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("dd/MM/yyyy") : "Chưa phản hồi"))

                 .ForMember(dest => dest.UserName,
                 opt => opt.MapFrom(src => src.CreateUser != null ? src.CreateUser : ""))
                 .ForMember(dest => dest.LibrarianName,
                 opt => opt.MapFrom(src => src.UpdateUser != null ? src.UpdateUser : ""));
            CreateMap<NotificationType, BorrowingStatus>();
            CreateMap<Borrowing, ReplyBorrowingResponse>()
                .ForMember(dest => dest.BorrowingStatus,
                opt => opt.MapFrom(src => BorrowingStatusHelper.GetStatusDescription(src.BorrowingStatus.Value)))
                .ForMember(dest => dest.UpdateUser,
               opt => opt.MapFrom(src => src.UpdateUser != null ? src.UpdateUser : ""))
                .ForMember(dest => dest.UpdateUser,
                 opt => opt.MapFrom(src => src.UpdateUser != null ? src.UpdateUser : ""));


        }
        public static class BorrowingStatusHelper
        {
        public static string GetStatusDescription(BorrowingStatus status)
        {
            return status switch
            {
                BorrowingStatus.Wait => "Chờ Duyệt",
                BorrowingStatus.Approved => "Đã Duyệt",
                BorrowingStatus.Scheduled => "Đã Hẹn Lịch",
                BorrowingStatus.Borrowing => "Đang Mượn",
                BorrowingStatus.Returned => "Đã trả",
                BorrowingStatus.Overdue => "Đã quá hạn",
                BorrowingStatus.Reject => "Từ chối",
                _ => "Không xác định"
            };
        }
        }
        public NotificationType MapBorrowingStatusToNotificationType(BorrowingStatus status)
        {
            return status switch
            {
                BorrowingStatus.Approved => NotificationType.Approved,
                BorrowingStatus.Borrowing => NotificationType.Borrowed,
                BorrowingStatus.Returned => NotificationType.Returned,
                BorrowingStatus.Overdue => NotificationType.Overdue,
                BorrowingStatus.Reject => NotificationType.Cancel,
                BorrowingStatus.Scheduled => NotificationType.Reminder,
                _ => NotificationType.statusChange
            };
        }
    }
}
