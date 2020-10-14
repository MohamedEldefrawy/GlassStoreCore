using Microsoft.EntityFrameworkCore.Migrations;

namespace GlassStoreCore.Data.Migrations
{
    public partial class addSerialNumberRoWholeSaleProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "WholeSaleProducts",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "WholeSaleProducts");
        }
    }
}
