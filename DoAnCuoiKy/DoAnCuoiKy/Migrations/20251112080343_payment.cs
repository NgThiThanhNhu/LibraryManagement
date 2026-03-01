using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "fines");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "fines");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "fines");

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VnpBankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VnpText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VnpResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    PaymentType = table.Column<byte>(type: "tinyint", nullable: true),
                    TransactionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorrowingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BorrowAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_borrowings_BorrowingId",
                        column: x => x.BorrowingId,
                        principalTable: "borrowings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BorrowingId",
                table: "Payment",
                column: "BorrowingId",
                unique: true,
                filter: "[BorrowingId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.AddColumn<byte>(
                name: "PaymentStatus",
                table: "fines",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "PaymentType",
                table: "fines",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "fines",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
