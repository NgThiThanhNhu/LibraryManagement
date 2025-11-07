using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class updateFine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysLate",
                table: "fines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FineRate",
                table: "fines",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysLate",
                table: "fines");

            migrationBuilder.DropColumn(
                name: "FineRate",
                table: "fines");
        }
    }
}
