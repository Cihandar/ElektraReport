using Microsoft.EntityFrameworkCore.Migrations;

namespace ElektraReport.Migrations
{
    public partial class admin2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Companys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Companys",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Companys");
        }
    }
}
