
using DoAnCuoiKy.Model.Entities.Usermanage;
using System.ComponentModel.DataAnnotations;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookCategory : BaseEntity
    {
       
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public List<Book>? books { get; set; }
    }
}
