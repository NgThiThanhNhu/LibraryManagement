using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class updateProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserBookInteraction_Book_Type_Date",
                table: "userBookInteractions");

            migrationBuilder.DropIndex(
                name: "IX_UserBookInteraction_Date",
                table: "userBookInteractions");

            migrationBuilder.DropColumn(
                name: "InteractionDate",
                table: "userBookInteractions");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteraction_Book_Type_Date",
                table: "userBookInteractions",
                columns: new[] { "BookId", "InteractionType", "CreateDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteraction_Date",
                table: "userBookInteractions",
                column: "CreateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserBookInteraction_Book_Type_Date",
                table: "userBookInteractions");

            migrationBuilder.DropIndex(
                name: "IX_UserBookInteraction_Date",
                table: "userBookInteractions");

            migrationBuilder.AddColumn<DateTime>(
                name: "InteractionDate",
                table: "userBookInteractions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteraction_Book_Type_Date",
                table: "userBookInteractions",
                columns: new[] { "BookId", "InteractionType", "InteractionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteraction_Date",
                table: "userBookInteractions",
                column: "InteractionDate");
        }
    }
}
