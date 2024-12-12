using StockManagement.Core.Entities;

namespace StockManagement.Application.Features.Demandes.DTOs
{
    public class GetAllListDemandesResponseDTO
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string DirectionNom { get; set; }
        public string DivisionNom { get; set; }
        public string ServiceNom { get; set; }
        public string StatusDemandeNom { get; set; }
        public string? DemandeNumber { get; set; }
        public List<AllProduitQuantiteDTO> LstAllProduitQuantiteDTOs { get; set; }
    }

    public class AllProduitQuantiteDTO
    {
        public string ProduitNom { get; set; }
        public int Quantité { get; set; }
    }

}
