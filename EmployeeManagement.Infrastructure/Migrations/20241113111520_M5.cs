using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class M5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produits_StockDetails_StockDetailId",
                table: "Produits");

            migrationBuilder.DropIndex(
                name: "IX_Produits_StockDetailId",
                table: "Produits");

            migrationBuilder.DropColumn(
                name: "StockDetailId",
                table: "Produits");

            migrationBuilder.AddColumn<Guid>(
                name: "ProduitId",
                table: "StockDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_ProduitId",
                table: "StockDetails",
                column: "ProduitId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockDetails_Produits_ProduitId",
                table: "StockDetails",
                column: "ProduitId",
                principalTable: "Produits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockDetails_Produits_ProduitId",
                table: "StockDetails");

            migrationBuilder.DropIndex(
                name: "IX_StockDetails_ProduitId",
                table: "StockDetails");

            migrationBuilder.DropColumn(
                name: "ProduitId",
                table: "StockDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "StockDetailId",
                table: "Produits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Produits_StockDetailId",
                table: "Produits",
                column: "StockDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produits_StockDetails_StockDetailId",
                table: "Produits",
                column: "StockDetailId",
                principalTable: "StockDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
