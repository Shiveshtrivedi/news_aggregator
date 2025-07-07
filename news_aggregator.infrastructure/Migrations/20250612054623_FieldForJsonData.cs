using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news_aggregator.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FieldForJsonData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
