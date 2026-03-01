using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCuoiKy.Migrations
{
    /// <inheritdoc />
    public partial class userBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserSearchHistoryId",
                table: "books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "bookRecommendations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RecommendationType = table.Column<byte>(type: "tinyint", nullable: false),
                    IsClicked = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_bookRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookRecommendations_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookRecommendations_librarians_UserId",
                        column: x => x.UserId,
                        principalTable: "librarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userBookInteractions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InteractionType = table.Column<byte>(type: "tinyint", nullable: false),
                    InteractionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DurationSeconds = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_userBookInteractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userBookInteractions_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userBookInteractions_librarians_UserId",
                        column: x => x.UserId,
                        principalTable: "librarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userCategoryPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferenceScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    BorrowCount = table.Column<int>(type: "int", nullable: false),
                    ReadingMinutes = table.Column<int>(type: "int", nullable: false),
                    LastInteraction = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_userCategoryPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userCategoryPreferences_bookCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "bookCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userCategoryPreferences_librarians_UserId",
                        column: x => x.UserId,
                        principalTable: "librarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userReadingSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationSeconds = table.Column<int>(type: "int", nullable: false),
                    LastPageNumber = table.Column<int>(type: "int", nullable: false),
                    TotalPages = table.Column<int>(type: "int", nullable: false),
                    ReadingProgress = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userReadingSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userReadingSessions_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userReadingSessions_librarians_UserId",
                        column: x => x.UserId,
                        principalTable: "librarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userSearchHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SearchKeyword = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SearchType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResultCount = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_userSearchHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userSearchHistories_librarians_UserId",
                        column: x => x.UserId,
                        principalTable: "librarians",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_books_UserSearchHistoryId",
                table: "books",
                column: "UserSearchHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRecommendation_Date",
                table: "bookRecommendations",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_BookRecommendation_User_Date",
                table: "bookRecommendations",
                columns: new[] { "UserId", "CreateDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BookRecommendation_User_Score",
                table: "bookRecommendations",
                columns: new[] { "UserId", "Score" });

            migrationBuilder.CreateIndex(
                name: "IX_bookRecommendations_BookId",
                table: "bookRecommendations",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteraction_Book_Type_Date",
                table: "userBookInteractions",
                columns: new[] { "BookId", "InteractionType", "InteractionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteraction_BookId",
                table: "userBookInteractions",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteraction_Date",
                table: "userBookInteractions",
                column: "InteractionDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteraction_User_Type",
                table: "userBookInteractions",
                columns: new[] { "UserId", "InteractionType" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteraction_UserId",
                table: "userBookInteractions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCategoryPreference_User_Category",
                table: "userCategoryPreferences",
                columns: new[] { "UserId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCategoryPreference_User_Score",
                table: "userCategoryPreferences",
                columns: new[] { "UserId", "PreferenceScore" });

            migrationBuilder.CreateIndex(
                name: "IX_userCategoryPreferences_CategoryId",
                table: "userCategoryPreferences",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReadingSession_BookId",
                table: "userReadingSessions",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReadingSession_Composite",
                table: "userReadingSessions",
                columns: new[] { "UserId", "BookId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_UserReadingSession_StartTime",
                table: "userReadingSessions",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserReadingSession_UserId",
                table: "userReadingSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSearchHistory_Keyword",
                table: "userSearchHistories",
                column: "SearchKeyword");

            migrationBuilder.CreateIndex(
                name: "IX_UserSearchHistory_SearchedAt",
                table: "userSearchHistories",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserSearchHistory_User_Date",
                table: "userSearchHistories",
                columns: new[] { "UserId", "CreateDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSearchHistory_UserId",
                table: "userSearchHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_books_userSearchHistories_UserSearchHistoryId",
                table: "books",
                column: "UserSearchHistoryId",
                principalTable: "userSearchHistories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_books_userSearchHistories_UserSearchHistoryId",
                table: "books");

            migrationBuilder.DropTable(
                name: "bookRecommendations");

            migrationBuilder.DropTable(
                name: "userBookInteractions");

            migrationBuilder.DropTable(
                name: "userCategoryPreferences");

            migrationBuilder.DropTable(
                name: "userReadingSessions");

            migrationBuilder.DropTable(
                name: "userSearchHistories");

            migrationBuilder.DropIndex(
                name: "IX_books_UserSearchHistoryId",
                table: "books");

            migrationBuilder.DropColumn(
                name: "UserSearchHistoryId",
                table: "books");
        }
    }
}
