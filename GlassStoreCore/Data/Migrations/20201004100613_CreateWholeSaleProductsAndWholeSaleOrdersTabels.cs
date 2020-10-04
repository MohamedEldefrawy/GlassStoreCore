using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GlassStoreCore.Data.Migrations
{
    public partial class CreateWholeSaleProductsAndWholeSaleOrdersTabels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WholeSaleProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    UnitsInStock = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholeSaleProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WholeSaleSellingOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholeSaleSellingOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WholeSaleSellingOrders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WholeSaleSellingOrders_UserId",
                table: "WholeSaleSellingOrders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WholeSaleProducts");

            migrationBuilder.DropTable(
                name: "WholeSaleSellingOrders");
        }
    }
}
