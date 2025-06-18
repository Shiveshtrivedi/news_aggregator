using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news_aggregator.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removingUnusedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenExpirationTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ContentPath",
                table: "ExternalSources");

            migrationBuilder.DropColumn(
                name: "PublishedAtPath",
                table: "ExternalSources");

            migrationBuilder.DropColumn(
                name: "RootListPath",
                table: "ExternalSources");

            migrationBuilder.DropColumn(
                name: "SourcePath",
                table: "ExternalSources");

            migrationBuilder.DropColumn(
                name: "TitlePath",
                table: "ExternalSources");

            migrationBuilder.DropColumn(
                name: "UrlPath",
                table: "ExternalSources");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.AddColumn<string>(
                name: "ContentPath",
                table: "ExternalSources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublishedAtPath",
                table: "ExternalSources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RootListPath",
                table: "ExternalSources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourcePath",
                table: "ExternalSources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitlePath",
                table: "ExternalSources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlPath",
                table: "ExternalSources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
