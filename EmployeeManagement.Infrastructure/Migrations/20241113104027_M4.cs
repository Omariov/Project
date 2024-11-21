using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class M4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusDemandes_Demandes_DemandeId",
                table: "StatusDemandes");

            migrationBuilder.DropIndex(
                name: "IX_StatusDemandes_DemandeId",
                table: "StatusDemandes");

            migrationBuilder.DropColumn(
                name: "DemandeId",
                table: "StatusDemandes");

            migrationBuilder.CreateTable(
                name: "HistoriqueStatusDemandes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusDemandeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DemandeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriqueStatusDemandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoriqueStatusDemandes_Demandes_DemandeId",
                        column: x => x.DemandeId,
                        principalTable: "Demandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoriqueStatusDemandes_StatusDemandes_StatusDemandeId",
                        column: x => x.StatusDemandeId,
                        principalTable: "StatusDemandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueStatusDemandes_DemandeId",
                table: "HistoriqueStatusDemandes",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueStatusDemandes_StatusDemandeId",
                table: "HistoriqueStatusDemandes",
                column: "StatusDemandeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoriqueStatusDemandes");

            migrationBuilder.AddColumn<Guid>(
                name: "DemandeId",
                table: "StatusDemandes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusDemandes_DemandeId",
                table: "StatusDemandes",
                column: "DemandeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusDemandes_Demandes_DemandeId",
                table: "StatusDemandes",
                column: "DemandeId",
                principalTable: "Demandes",
                principalColumn: "Id");
        }
    }
}
