using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElektraReport.Migrations
{
    public partial class Update14022023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BlackList",
                table: "DepremKayits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BlackListNote",
                table: "DepremKayits",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "DepremKayits",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifyClientIp",
                table: "DepremKayits",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "DepremKayits",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CompanyType",
                table: "Companys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "OnayYetki",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    Tarih = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    ClientIp = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropColumn(
                name: "BlackList",
                table: "DepremKayits");

            migrationBuilder.DropColumn(
                name: "BlackListNote",
                table: "DepremKayits");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "DepremKayits");

            migrationBuilder.DropColumn(
                name: "ModifyClientIp",
                table: "DepremKayits");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "DepremKayits");

            migrationBuilder.DropColumn(
                name: "CompanyType",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "OnayYetki",
                table: "AspNetUsers");
        }
    }
}
