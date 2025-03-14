using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class capnhatbookitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_books_bookChapters_BookChapterId",
                table: "books");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookChapterId",
                table: "books",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "BookCategoryId",
                table: "bookItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookItems_BookCategoryId",
                table: "bookItems",
                column: "BookCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookItems_bookCategories_BookCategoryId",
                table: "bookItems",
                column: "BookCategoryId",
                principalTable: "bookCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_books_bookChapters_BookChapterId",
                table: "books",
                column: "BookChapterId",
                principalTable: "bookChapters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookItems_bookCategories_BookCategoryId",
                table: "bookItems");

            migrationBuilder.DropForeignKey(
                name: "FK_books_bookChapters_BookChapterId",
                table: "books");

            migrationBuilder.DropIndex(
                name: "IX_bookItems_BookCategoryId",
                table: "bookItems");

            migrationBuilder.DropColumn(
                name: "BookCategoryId",
                table: "bookItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookChapterId",
                table: "books",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_books_bookChapters_BookChapterId",
                table: "books",
                column: "BookChapterId",
                principalTable: "bookChapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
