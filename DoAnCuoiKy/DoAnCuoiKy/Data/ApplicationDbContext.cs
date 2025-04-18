using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
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
        public DbSet<BookAuthor> bookAuthors { get; set; }
        public DbSet<Publisher> publishers { get; set; }
        public DbSet<Floor> floors { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<BookShelf> bookShelves { get; set; }
        public DbSet<Shelf> shelves { get; set; }
        public DbSet<ShelfSection> shelfSections { get; set; }
        public DbSet<Location> locations { get; set; }
        public DbSet<BookImportTransaction> bookImportTransactions { get; set; }
        public DbSet<BookExportTransaction> bookExportTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


    }
}
