using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news_aggregator.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingTokenField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTokenActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpirationTime",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalSourceId",
                table: "NewsArticles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_ExternalSourceId",
                table: "NewsArticles",
                column: "ExternalSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsArticles_ExternalSources_ExternalSourceId",
                table: "NewsArticles",
                column: "ExternalSourceId",
                principalTable: "ExternalSources",
                principalColumn: "ExternalSourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsArticles_ExternalSources_ExternalSourceId",
                table: "NewsArticles");

            migrationBuilder.DropIndex(
                name: "IX_NewsArticles_ExternalSourceId",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "IsTokenActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenExpirationTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExternalSourceId",
                table: "NewsArticles");
        }
    }
}
