namespace DoAnCuoiKy.Model.Entities.Usermanage
{
    public class Role : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; } //tên vai trò
        
        public List<Librarian>? librarians { get; set; }
        public List<Users>? users { get; set; }
    }
}
