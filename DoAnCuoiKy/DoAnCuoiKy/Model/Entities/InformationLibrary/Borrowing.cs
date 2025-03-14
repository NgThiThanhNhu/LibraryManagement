using DoAnCuoiKy.Model.Entities.Usermanage;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class Borrowing : BaseEntity
    {
        public Guid? Id { get; set; }
      
        public Guid? UserId { get; set; }
        public Users? users { get; set; }

        //mối quan hệ giữa borrowingDetail
      
        public List<BorrowingDetail>? borrowingDetails { get; set; }


    }
}
