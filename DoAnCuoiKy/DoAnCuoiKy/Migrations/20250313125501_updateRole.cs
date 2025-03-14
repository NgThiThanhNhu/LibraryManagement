using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class updateRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "roles");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateUser",
                table: "roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "roles",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateUser",
                table: "roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleteUser",
                table: "roles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "CreateUser",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "deleteUser",
                table: "roles");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "roles",
                type: "nvarchar(526)",
                maxLength: 526,
                nullable: true);
        }
    }
}
