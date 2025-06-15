using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news_aggregator.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingApiFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsArticles_ExternalSources_ExternalSourceId",
                table: "NewsArticles");

            migrationBuilder.AlterColumn<int>(
                name: "ExternalSourceId",
                table: "NewsArticles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseUrl",
                table: "ExternalSources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserKeywords",
                columns: table => new
                {
                    UserKeywordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserKeywords", x => x.UserKeywordId);
                    table.ForeignKey(
                        name: "FK_UserKeywords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserKeywords_UserId",
                table: "UserKeywords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsArticles_ExternalSources_ExternalSourceId",
                table: "NewsArticles",
                column: "ExternalSourceId",
                principalTable: "ExternalSources",
                principalColumn: "ExternalSourceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsArticles_ExternalSources_ExternalSourceId",
                table: "NewsArticles");

            migrationBuilder.DropTable(
                name: "UserKeywords");

            migrationBuilder.DropColumn(
                name: "BaseUrl",
                table: "ExternalSources");

            migrationBuilder.AlterColumn<int>(
                name: "ExternalSourceId",
                table: "NewsArticles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsArticles_ExternalSources_ExternalSourceId",
                table: "NewsArticles",
                column: "ExternalSourceId",
                principalTable: "ExternalSources",
                principalColumn: "ExternalSourceId");
        }
    }
}
