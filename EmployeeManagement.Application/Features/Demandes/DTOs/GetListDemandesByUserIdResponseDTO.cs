using StockManagement.Core.Entities;

namespace StockManagement.Application.Features.Demandes.DTOs
{
    public class GetListDemandesByUserIdResponseDTO
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string DirectionNom { get; set; }
        public string DivisionNom { get; set; }
        public string ServiceNom { get; set; } 
        public string StatusDemandeNom { get; set; }
        public List<ProduitQuantiteDTO> LstProduitQuantiteDTOs { get; set; }
    }

    public class ProduitQuantiteDTO
    {
        public string ProduitNom { get; set; }
        public int Quantité { get; set; }
    }

}
