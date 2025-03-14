using DoAnCuoiKy.Model.Entities.InformationLibrary;
using System.Runtime.CompilerServices;

namespace DoAnCuoiKy.Model.Entities.Usermanage
{
    public class Librarian : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
      /*  public int? RoleId { get; set; }*/ //phân quyền
        //public Role? Role { get; set; }
        public List<Role>? roles { get; set; }
        

    }
}
