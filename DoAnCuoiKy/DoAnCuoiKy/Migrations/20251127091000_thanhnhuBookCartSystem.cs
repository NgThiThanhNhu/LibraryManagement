using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class thanhnhuBookCartSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookCartItems_bookItems_BookItemId",
                table: "bookCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_bookCartItems_librarians_UserId",
                table: "bookCartItems");

            migrationBuilder.DropIndex(
                name: "IX_bookCartItems_BookItemId",
                table: "bookCartItems");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "bookCartItems",
                newName: "CartId");

            migrationBuilder.RenameColumn(
                name: "BookItemId",
                table: "bookCartItems",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_bookCartItems_UserId",
                table: "bookCartItems",
                newName: "IX_bookCartItems_CartId");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "bookCartItems",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "bookCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CartStatus = table.Column<byte>(type: "tinyint", maxLength: 20, nullable: false),
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
                    table.PrimaryKey("PK_bookCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookCarts_librarians_UserId",
                        column: x => x.UserId,
                        principalTable: "librarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookCartItems_BookId",
                table: "bookCartItems",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCarts_UserId_Status",
                table: "bookCarts",
                columns: new[] { "UserId", "CartStatus" });

            migrationBuilder.AddForeignKey(
                name: "FK_bookCartItems_bookCarts_CartId",
                table: "bookCartItems",
                column: "CartId",
                principalTable: "bookCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookCartItems_books_BookId",
                table: "bookCartItems",
                column: "BookId",
                principalTable: "books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookCartItems_bookCarts_CartId",
                table: "bookCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_bookCartItems_books_BookId",
                table: "bookCartItems");

            migrationBuilder.DropTable(
                name: "bookCarts");

            migrationBuilder.DropIndex(
                name: "IX_bookCartItems_BookId",
                table: "bookCartItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "bookCartItems");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "bookCartItems",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "bookCartItems",
                newName: "BookItemId");

            migrationBuilder.RenameIndex(
                name: "IX_bookCartItems_CartId",
                table: "bookCartItems",
                newName: "IX_bookCartItems_UserId");

            migrationBuilder.AddColumn<byte>(
                name: "PaymentStatus",
                table: "payments",
                type: "tinyint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookCartItems_BookItemId",
                table: "bookCartItems",
                column: "BookItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bookCartItems_bookItems_BookItemId",
                table: "bookCartItems",
                column: "BookItemId",
                principalTable: "bookItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookCartItems_librarians_UserId",
                table: "bookCartItems",
                column: "UserId",
                principalTable: "librarians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
