using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class rangbuocName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "bookCategories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "bookAuthors",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_publishers_PublisherName",
                table: "publishers",
                column: "PublisherName",
                unique: true,
                filter: "[PublisherName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_books_Title",
                table: "books",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_bookChapters_TitleChapter",
                table: "bookChapters",
                column: "TitleChapter",
                unique: true,
                filter: "[TitleChapter] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_bookCategories_Name",
                table: "bookCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookAuthors_Name",
                table: "bookAuthors",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_publishers_PublisherName",
                table: "publishers");

            migrationBuilder.DropIndex(
                name: "IX_books_Title",
                table: "books");

            migrationBuilder.DropIndex(
                name: "IX_bookChapters_TitleChapter",
                table: "bookChapters");

            migrationBuilder.DropIndex(
                name: "IX_bookCategories_Name",
                table: "bookCategories");

            migrationBuilder.DropIndex(
                name: "IX_bookAuthors_Name",
                table: "bookAuthors");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "bookCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "bookAuthors",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
