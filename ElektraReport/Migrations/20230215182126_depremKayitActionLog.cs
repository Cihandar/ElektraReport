using Microsoft.EntityFrameworkCore.Migrations;

namespace ElektraReport.Migrations
{
    public partial class depremKayitActionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionLog",
                table: "DepremKayits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionLog",
                table: "DepremKayits");
        }
    }
}
