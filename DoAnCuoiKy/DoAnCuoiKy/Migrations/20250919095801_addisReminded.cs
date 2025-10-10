using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class addisReminded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isReminded",
                table: "borrowings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<byte>(
                name: "ExportReason",
                table: "bookExportTransactions",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isReminded",
                table: "borrowings");

            migrationBuilder.AlterColumn<byte>(
                name: "ExportReason",
                table: "bookExportTransactions",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);
        }
    }
}
