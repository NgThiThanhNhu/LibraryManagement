using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyIsFined : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFined",
                table: "borrowingDetails",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFined",
                table: "borrowingDetails");
        }
    }
}
