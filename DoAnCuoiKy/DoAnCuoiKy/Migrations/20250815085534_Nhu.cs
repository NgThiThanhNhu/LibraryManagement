using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class Nhu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bookAuthors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookAuthors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bookCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bookChapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleChapter = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookChapters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "floors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FloorName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_floors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "publishers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublisherName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publishers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: true),
                    MaxBookShelfCapity = table.Column<int>(type: "int", nullable: true),
                    FloorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rooms_floors_FloorId",
                        column: x => x.FloorId,
                        principalTable: "floors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PublisherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    YearPublished = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<float>(type: "real", nullable: true),
                    UnitPrice = table.Column<float>(type: "real", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookChapterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_books_bookAuthors_BookAuthorId",
                        column: x => x.BookAuthorId,
                        principalTable: "bookAuthors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_books_bookCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "bookCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_books_bookChapters_BookChapterId",
                        column: x => x.BookChapterId,
                        principalTable: "bookChapters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_books_publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "publishers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "librarians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isValidate = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_librarians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_librarians_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookShelves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookShelfName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NumberOfShelves = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookShelves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookShelves_rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "bookFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PublicIdImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PublicIdFile = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    bookFileType = table.Column<byte>(type: "tinyint", maxLength: 20, nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookFiles_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookImportTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    TotalPrice = table.Column<float>(type: "real", nullable: false),
                    TransactionType = table.Column<byte>(type: "tinyint", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookImportTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookImportTransactions_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "borrowings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BorrowingStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    LibrarianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isScheduled = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_borrowings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_borrowings_librarians_LibrarianId",
                        column: x => x.LibrarianId,
                        principalTable: "librarians",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "shelves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShelfName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    NumberOfSections = table.Column<int>(type: "int", nullable: true),
                    BookshelfId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shelves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shelves_bookShelves_BookshelfId",
                        column: x => x.BookshelfId,
                        principalTable: "bookShelves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookPickupSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledPickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiredPickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPickedUp = table.Column<bool>(type: "bit", nullable: false),
                    IsNotified = table.Column<bool>(type: "bit", nullable: false),
                    NotificationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookPickupSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookPickupSchedules_borrowings_BorrowingId",
                        column: x => x.BorrowingId,
                        principalTable: "borrowings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notificationToUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    NotificationType = table.Column<byte>(type: "tinyint", nullable: false),
                    BorrowingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificationToUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notificationToUsers_borrowings_BorrowingId",
                        column: x => x.BorrowingId,
                        principalTable: "borrowings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notificationToUsers_librarians_UserId",
                        column: x => x.UserId,
                        principalTable: "librarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shelfSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    ShelfId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shelfSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shelfSections_shelves_ShelfId",
                        column: x => x.ShelfId,
                        principalTable: "shelves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BarCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShelfSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookItems_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_bookItems_shelfSections_ShelfSectionId",
                        column: x => x.ShelfSectionId,
                        principalTable: "shelfSections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "bookCartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookCartItems_bookItems_BookItemId",
                        column: x => x.BookItemId,
                        principalTable: "bookItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookCartItems_librarians_UserId",
                        column: x => x.UserId,
                        principalTable: "librarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookReservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    usersId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookReservations_bookItems_BookItemId",
                        column: x => x.BookItemId,
                        principalTable: "bookItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_bookReservations_users_usersId",
                        column: x => x.usersId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "borrowingDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bookStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    BookItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_borrowingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_borrowingDetails_bookItems_BookItemId",
                        column: x => x.BookItemId,
                        principalTable: "bookItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_borrowingDetails_borrowings_BorrowingId",
                        column: x => x.BorrowingId,
                        principalTable: "borrowings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "bookExportTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExportReason = table.Column<byte>(type: "tinyint", nullable: false),
                    TransactionType = table.Column<byte>(type: "tinyint", nullable: false),
                    BorrowingDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookExportTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookExportTransactions_borrowingDetails_BorrowingDetailId",
                        column: x => x.BorrowingDetailId,
                        principalTable: "borrowingDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    fineReason = table.Column<byte>(type: "tinyint", nullable: true),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true),
                    LibrarianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowingDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fines_borrowingDetails_BorrowingDetailId",
                        column: x => x.BorrowingDetailId,
                        principalTable: "borrowingDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_fines_librarians_LibrarianId",
                        column: x => x.LibrarianId,
                        principalTable: "librarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookCartItems_BookItemId",
                table: "bookCartItems",
                column: "BookItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookCartItems_UserId",
                table: "bookCartItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_bookExportTransactions_BorrowingDetailId",
                table: "bookExportTransactions",
                column: "BorrowingDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_bookFiles_BookId",
                table: "bookFiles",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_bookImportTransactions_BookId",
                table: "bookImportTransactions",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_bookItems_BookId",
                table: "bookItems",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_bookItems_ShelfSectionId",
                table: "bookItems",
                column: "ShelfSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_bookPickupSchedules_BorrowingId",
                table: "bookPickupSchedules",
                column: "BorrowingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookReservations_BookItemId",
                table: "bookReservations",
                column: "BookItemId");

            migrationBuilder.CreateIndex(
                name: "IX_bookReservations_usersId",
                table: "bookReservations",
                column: "usersId");

            migrationBuilder.CreateIndex(
                name: "IX_books_BookAuthorId",
                table: "books",
                column: "BookAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_books_BookChapterId",
                table: "books",
                column: "BookChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_books_CategoryId",
                table: "books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_books_PublisherId",
                table: "books",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_bookShelves_RoomId",
                table: "bookShelves",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_borrowingDetails_BookItemId",
                table: "borrowingDetails",
                column: "BookItemId");

            migrationBuilder.CreateIndex(
                name: "IX_borrowingDetails_BorrowingId",
                table: "borrowingDetails",
                column: "BorrowingId");

            migrationBuilder.CreateIndex(
                name: "IX_borrowings_LibrarianId",
                table: "borrowings",
                column: "LibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_fines_BorrowingDetailId",
                table: "fines",
                column: "BorrowingDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_fines_LibrarianId",
                table: "fines",
                column: "LibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_librarians_RoleId",
                table: "librarians",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_notificationToUsers_BorrowingId",
                table: "notificationToUsers",
                column: "BorrowingId");

            migrationBuilder.CreateIndex(
                name: "IX_notificationToUsers_UserId",
                table: "notificationToUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_FloorId",
                table: "rooms",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_shelfSections_ShelfId",
                table: "shelfSections",
                column: "ShelfId");

            migrationBuilder.CreateIndex(
                name: "IX_shelves_BookshelfId",
                table: "shelves",
                column: "BookshelfId");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookCartItems");

            migrationBuilder.DropTable(
                name: "bookExportTransactions");

            migrationBuilder.DropTable(
                name: "bookFiles");

            migrationBuilder.DropTable(
                name: "bookImportTransactions");

            migrationBuilder.DropTable(
                name: "bookPickupSchedules");

            migrationBuilder.DropTable(
                name: "bookReservations");

            migrationBuilder.DropTable(
                name: "fines");

            migrationBuilder.DropTable(
                name: "notificationToUsers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "borrowingDetails");

            migrationBuilder.DropTable(
                name: "bookItems");

            migrationBuilder.DropTable(
                name: "borrowings");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "shelfSections");

            migrationBuilder.DropTable(
                name: "librarians");

            migrationBuilder.DropTable(
                name: "bookAuthors");

            migrationBuilder.DropTable(
                name: "bookCategories");

            migrationBuilder.DropTable(
                name: "bookChapters");

            migrationBuilder.DropTable(
                name: "publishers");

            migrationBuilder.DropTable(
                name: "shelves");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "bookShelves");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "floors");
        }
    }
}
