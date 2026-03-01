using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class updatePropertiesBookRecommendation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowedAt",
                table: "bookRecommendations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClickedAt",
                table: "bookRecommendations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "bookRecommendations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBorrowed",
                table: "bookRecommendations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsViewed",
                table: "bookRecommendations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModelVersion",
                table: "bookRecommendations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "bookRecommendations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ViewedAt",
                table: "bookRecommendations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowedAt",
                table: "bookRecommendations");

            migrationBuilder.DropColumn(
                name: "ClickedAt",
                table: "bookRecommendations");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "bookRecommendations");

            migrationBuilder.DropColumn(
                name: "IsBorrowed",
                table: "bookRecommendations");

            migrationBuilder.DropColumn(
                name: "IsViewed",
                table: "bookRecommendations");

            migrationBuilder.DropColumn(
                name: "ModelVersion",
                table: "bookRecommendations");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "bookRecommendations");

            migrationBuilder.DropColumn(
                name: "ViewedAt",
                table: "bookRecommendations");
        }
    }
}
