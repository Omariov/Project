using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class M3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Demandes_DemandeId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_StockDetails_StockDetailsId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_StatusDemandes_StatusDemandeId",
                table: "Demandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Directions_Divisions_DivisionId",
                table: "Directions");

            migrationBuilder.DropIndex(
                name: "IX_Directions_DivisionId",
                table: "Directions");

            migrationBuilder.DropIndex(
                name: "IX_Demandes_StatusDemandeId",
                table: "Demandes");

            migrationBuilder.DropIndex(
                name: "IX_Articles_DemandeId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_StockDetailsId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "Directions");

            migrationBuilder.DropColumn(
                name: "StatusDemandeId",
                table: "Demandes");

            migrationBuilder.DropColumn(
                name: "DemandeId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "StockDetailsId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "DemandeId",
                table: "StatusDemandes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StockDetailId",
                table: "Produits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Instock",
                table: "HistoriqueMouvements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DirectionId",
                table: "Divisions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DemandeProduits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProduitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantité = table.Column<int>(type: "int", nullable: false),
                    Livré = table.Column<int>(type: "int", nullable: false),
                    DemandeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandeProduits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandeProduits_Demandes_DemandeId",
                        column: x => x.DemandeId,
                        principalTable: "Demandes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DemandeProduits_Produits_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "Produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusDemandes_DemandeId",
                table: "StatusDemandes",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_StockDetailId",
                table: "Produits",
                column: "StockDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_DirectionId",
                table: "Divisions",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandeProduits_DemandeId",
                table: "DemandeProduits",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandeProduits_ProduitId",
                table: "DemandeProduits",
                column: "ProduitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Directions_DirectionId",
                table: "Divisions",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produits_StockDetails_StockDetailId",
                table: "Produits",
                column: "StockDetailId",
                principalTable: "StockDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StatusDemandes_Demandes_DemandeId",
                table: "StatusDemandes",
                column: "DemandeId",
                principalTable: "Demandes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Directions_DirectionId",
                table: "Divisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Produits_StockDetails_StockDetailId",
                table: "Produits");

            migrationBuilder.DropForeignKey(
                name: "FK_StatusDemandes_Demandes_DemandeId",
                table: "StatusDemandes");

            migrationBuilder.DropTable(
                name: "DemandeProduits");

            migrationBuilder.DropIndex(
                name: "IX_StatusDemandes_DemandeId",
                table: "StatusDemandes");

            migrationBuilder.DropIndex(
                name: "IX_Produits_StockDetailId",
                table: "Produits");

            migrationBuilder.DropIndex(
                name: "IX_Divisions_DirectionId",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "DemandeId",
                table: "StatusDemandes");

            migrationBuilder.DropColumn(
                name: "StockDetailId",
                table: "Produits");

            migrationBuilder.DropColumn(
                name: "Instock",
                table: "HistoriqueMouvements");

            migrationBuilder.DropColumn(
                name: "DirectionId",
                table: "Divisions");

            migrationBuilder.AddColumn<Guid>(
                name: "DivisionId",
                table: "Directions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StatusDemandeId",
                table: "Demandes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DemandeId",
                table: "Articles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StockDetailsId",
                table: "Articles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Directions_DivisionId",
                table: "Directions",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Demandes_StatusDemandeId",
                table: "Demandes",
                column: "StatusDemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_DemandeId",
                table: "Articles",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_StockDetailsId",
                table: "Articles",
                column: "StockDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Demandes_DemandeId",
                table: "Articles",
                column: "DemandeId",
                principalTable: "Demandes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_StockDetails_StockDetailsId",
                table: "Articles",
                column: "StockDetailsId",
                principalTable: "StockDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_StatusDemandes_StatusDemandeId",
                table: "Demandes",
                column: "StatusDemandeId",
                principalTable: "StatusDemandes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Directions_Divisions_DivisionId",
                table: "Directions",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
