using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using System.Runtime.CompilerServices;

namespace DoAnCuoiKy.Model.Entities.Usermanage
{
    public class Librarian : BaseEntity
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public Guid? RoleId { get; set; } //phân quyền
        public Role? Role { get; set; }
       
        //1librarian-nhiều transaction
        public bool isValidate { get; set; } = false;
       


    }
}
