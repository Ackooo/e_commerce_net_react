using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BuyerIdChangedToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerId",
                schema: "Store",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                schema: "Store",
                table: "Basket");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "Store",
                table: "Order",
                type: "uniqueidentifier",
                maxLength: 256,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "Store",
                table: "Basket",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                schema: "Store",
                table: "Order",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_UserId",
                schema: "Store",
                table: "Order",
                column: "UserId",
                principalSchema: "User",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_UserId",
                schema: "Store",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_UserId",
                schema: "Store",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Store",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Store",
                table: "Basket");

            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                schema: "Store",
                table: "Order",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                schema: "Store",
                table: "Basket",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
