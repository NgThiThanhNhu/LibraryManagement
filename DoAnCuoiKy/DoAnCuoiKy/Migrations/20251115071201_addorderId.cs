using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class addorderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VnpBankCode",
                table: "payments",
                newName: "vnpBankCode");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "vnpBankCode",
                table: "payments",
                newName: "VnpBankCode");
        }
    }
}
