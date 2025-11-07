using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BorrowingResponse
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public int Duration { get; set; }
        public DateTime DueDate {  get; set; }
        public string? Status { get; set; }
        public string UserName { get; set; }
        public string LibrarianName { get; set; }
        public string ReplyDate { get; set; }
        public bool isReminded { get; set; }
    }
}
