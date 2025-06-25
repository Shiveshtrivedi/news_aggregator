using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news_aggregator.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingReportFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "NewsArticles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReportCount",
                table: "NewsArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReportArticles",
                columns: table => new
                {
                    ReportArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NewsArticleId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportArticles", x => x.ReportArticleId);
                    table.ForeignKey(
                        name: "FK_ReportArticles_NewsArticles_NewsArticleId",
                        column: x => x.NewsArticleId,
                        principalTable: "NewsArticles",
                        principalColumn: "NewsArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportArticles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportArticles_NewsArticleId",
                table: "ReportArticles",
                column: "NewsArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportArticles_UserId",
                table: "ReportArticles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportArticles");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "ReportCount",
                table: "NewsArticles");
        }
    }
}
