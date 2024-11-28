using StockManagement.Core.Entities;

namespace StockManagement.Application.Features.Demandes.DTOs
{
    public class GetDemandeByIdResponseDTO
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string DirectionNom { get; set; }
        public string DivisionNom { get; set; }
        public string ServiceNom { get; set; }
        public string StatusDemandeNom { get; set; }
        public string? DemandeNumber { get; set; }
        public List<ProduitQuantiteDemandeDTO> LstProduitQuantiteDTOs { get; set; }
    }

    public class ProduitQuantiteDemandeDTO
    {
        public string ProduitNom { get; set; }
        public string Unites { get; set; }
        public int Quantité { get; set; }
    }

}
