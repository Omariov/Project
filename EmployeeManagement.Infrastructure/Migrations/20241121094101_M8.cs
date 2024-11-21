using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class M8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instock",
                table: "HistoriqueMouvements");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "HistoriqueMouvements");

            migrationBuilder.AddColumn<bool>(
                name: "Instock",
                table: "StockDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "EmplacementId",
                table: "HistoriqueMouvements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Emplacements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DirectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emplacements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emplacements_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueMouvements_EmplacementId",
                table: "HistoriqueMouvements",
                column: "EmplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_Emplacements_DirectionId",
                table: "Emplacements",
                column: "DirectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoriqueMouvements_Emplacements_EmplacementId",
                table: "HistoriqueMouvements",
                column: "EmplacementId",
                principalTable: "Emplacements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoriqueMouvements_Emplacements_EmplacementId",
                table: "HistoriqueMouvements");

            migrationBuilder.DropTable(
                name: "Emplacements");

            migrationBuilder.DropIndex(
                name: "IX_HistoriqueMouvements_EmplacementId",
                table: "HistoriqueMouvements");

            migrationBuilder.DropColumn(
                name: "Instock",
                table: "StockDetails");

            migrationBuilder.DropColumn(
                name: "EmplacementId",
                table: "HistoriqueMouvements");

            migrationBuilder.AddColumn<bool>(
                name: "Instock",
                table: "HistoriqueMouvements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "HistoriqueMouvements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
