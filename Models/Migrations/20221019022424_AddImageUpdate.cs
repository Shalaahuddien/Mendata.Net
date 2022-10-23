using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mendata.Net.Models.Migrations
{
    public partial class AddImageUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Barangs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Barangs",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Barangs");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Barangs");
        }
    }
}
