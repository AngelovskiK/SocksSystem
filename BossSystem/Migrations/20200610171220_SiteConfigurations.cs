using Microsoft.EntityFrameworkCore.Migrations;

namespace BossSystem.Migrations
{
    public partial class SiteConfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteConfigurations",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteConfigurations", x => x.Key);
                });

            migrationBuilder.InsertData(
                table: "SiteConfigurations",
                columns: new[] { "Key", "Value" },
                values: new object[] { "AdminPassword", "TheBoss" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteConfigurations");
        }
    }
}
