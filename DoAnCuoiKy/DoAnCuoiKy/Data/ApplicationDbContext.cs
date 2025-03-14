using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DoAnCuoiKy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        //các dbset đại diện cho các bảng trong cơ sở dữ liệu
        public DbSet<Book> books { get; set; }
        public DbSet<BookCategory> bookCategories { get; set; }
        public DbSet<BookReservation> bookReservations { get; set; }
        public DbSet<Borrowing> borrowings { get; set; }
        public DbSet<BorrowingDetail> borrowingDetails { get; set; }
        public DbSet<Fine> fines { get; set; }
        public DbSet<Librarian> librarians { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Users> users { get; set; }
        public DbSet<BookChapter> bookChapters { get; set; }
        public DbSet<BookItem> bookItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


    }
}
