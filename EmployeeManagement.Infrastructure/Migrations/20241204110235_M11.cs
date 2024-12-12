using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class M11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emplacements_Directions_DirectionId",
                table: "Emplacements");

            migrationBuilder.DropIndex(
                name: "IX_Emplacements_DirectionId",
                table: "Emplacements");

            migrationBuilder.DropColumn(
                name: "DirectionId",
                table: "Emplacements");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Emplacements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                table: "Directions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Directions_RegionId",
                table: "Directions",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Directions_Region_RegionId",
                table: "Directions",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directions_Region_RegionId",
                table: "Directions");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Directions_RegionId",
                table: "Directions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Emplacements");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Directions");

            migrationBuilder.AddColumn<Guid>(
                name: "DirectionId",
                table: "Emplacements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Emplacements_DirectionId",
                table: "Emplacements",
                column: "DirectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emplacements_Directions_DirectionId",
                table: "Emplacements",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
