namespace StockManagement.Core.Entities
{
    public class DemandeProduit : BaseEntity<Guid>
    {

        public Guid ProduitId { get; set; }
        public Produit Produit { get; set; }
        public int Quantité { get; set; }
        public int Livré { get; set; }

    }
}
