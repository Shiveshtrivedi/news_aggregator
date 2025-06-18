using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news_aggregator.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakingNotificationScalable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessEnabled",
                table: "NotificationConfigs");

            migrationBuilder.DropColumn(
                name: "EntertainmentEnabled",
                table: "NotificationConfigs");

            migrationBuilder.DropColumn(
                name: "SportsEnabled",
                table: "NotificationConfigs");

            migrationBuilder.DropColumn(
                name: "TechnologyEnabled",
                table: "NotificationConfigs");

            migrationBuilder.CreateTable(
                name: "NotificationCategorySetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    NotificationConfigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCategorySetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationCategorySetting_NotificationConfigs_NotificationConfigId",
                        column: x => x.NotificationConfigId,
                        principalTable: "NotificationConfigs",
                        principalColumn: "NotificationConfigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationCategorySetting_NotificationConfigId",
                table: "NotificationCategorySetting",
                column: "NotificationConfigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationCategorySetting");

            migrationBuilder.AddColumn<bool>(
                name: "BusinessEnabled",
                table: "NotificationConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EntertainmentEnabled",
                table: "NotificationConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SportsEnabled",
                table: "NotificationConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TechnologyEnabled",
                table: "NotificationConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
