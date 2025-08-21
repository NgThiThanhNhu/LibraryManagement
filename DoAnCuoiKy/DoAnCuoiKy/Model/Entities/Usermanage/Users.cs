using DoAnCuoiKy.Model.Entities.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.Usermanage
{
    public class Users
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? RegistrationDate { get; set; } = DateTime.Now;
        public bool? IsActive { get; set; } 
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        //public List<BookReservation>? Reservations { get; set; }
        //public List<Borrowing>? borrowings { get; set; }
        //public List<Fine>? fines { get; set; }

    }
}
