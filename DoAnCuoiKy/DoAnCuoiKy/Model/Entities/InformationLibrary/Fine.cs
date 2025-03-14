using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class Fine : BaseEntity
    {
        public Guid? Id { get; set; }
        public float? Amount { get; set; } //số tiền phạt
        public FineReason? fineReason { get; set; }
        public DateTime? IssuedDate { get; set; }
        public bool? IsPaid { get; set; }

        public Guid? UserId { get; set; }
        public Users? users { get; set; }

        public Guid? BorrowingDetailId { get; set; }
        public BorrowingDetail? borrowingDetail { get; set; } 
        

    }
}
