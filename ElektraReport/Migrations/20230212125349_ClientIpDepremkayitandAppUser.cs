using Microsoft.EntityFrameworkCore.Migrations;

namespace ElektraReport.Migrations
{
    public partial class ClientIpDepremkayitandAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientIp",
                table: "DepremKayits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientIp",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientIp",
                table: "DepremKayits");

            migrationBuilder.DropColumn(
                name: "ClientIp",
                table: "AspNetUsers");
        }
    }
}
