using Microsoft.EntityFrameworkCore.Migrations;

namespace ElektraReport.Migrations
{
    public partial class checkout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCheckOut",
                table: "DepremKayits",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCheckOut",
                table: "DepremKayits");
        }
    }
}
