using AutoMapper;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;

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

            CreateMap<BookExportTransaction, BookExportTransactionResponse>()
                .ForMember(dest => dest.ExportReason,
                opt => opt.MapFrom(src => GetStatusExportTransactionDescription(src.ExportReason.Value)))
                .ForMember(dest => dest.CreateBy,
                opt => opt.MapFrom(src => src.CreateUser != null ? src.CreateUser : ""))
                .ForMember(dest => dest.BookStatus,
                opt => opt.MapFrom(src => GetBookStatusDescription(src.BorrowingDetail.bookStatus.Value)))
                .ForMember(dest => dest.BookItemBarCode,
                opt => opt.MapFrom(src => src.BorrowingDetail.bookItem.BarCode != null ? src.BorrowingDetail.bookItem.BarCode : ""))
                .ForMember(dest => dest.ShelfSectionName,
                opt => opt.MapFrom(src => src.BorrowingDetail.bookItem.ShelfSection.SectionName != null ? src.BorrowingDetail.bookItem.ShelfSection.SectionName : ""))
                .ForMember(dest => dest.TransactionType,
                opt => opt.MapFrom(src => GetTransactionTypeDescription(src.TransactionType)))
                .ForMember(dest => dest.ExportDate,
                 opt => opt.MapFrom(src => src.CreateDate.ToString("dd/MM/yyyy")));

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
        public string GetStatusExportTransactionDescription(ExportReason exportReason)
        {
            return exportReason switch
            {
                ExportReason.Borrow => "Mượn sách",
                _ => "Không xác định"
            };
        }
        public string GetTransactionTypeDescription(TransactionType transactionType)
        {
            return transactionType switch
            {
                TransactionType.Import => "Nhập sách",
                TransactionType.Export => "Xuất sách",
                TransactionType.ReturnToStock => "Trả sách về kho",
                TransactionType.Remove => "Bỏ sách ra khỏi kho",
                _=>"Không xác định"
            };
        }
        public string GetBookStatusDescription (BookStatus bookStatus)
        {
            return bookStatus switch
            {
                BookStatus.Available => "Có sẵn",
                BookStatus.Borrowed => "Đã được mượn",
                BookStatus.Reserved => "Đã được đặt trước",
                BookStatus.Lost => "Bị mất",
                BookStatus.Damaged => "Bị hư hỏng",
                _=>"Không xác định"
            };
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
