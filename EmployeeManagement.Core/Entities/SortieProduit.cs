namespace StockManagement.Core.Entities
{
    public class SortieProduit : BaseEntity<Guid>
    {

        public Guid ProduitId { get; set; }
        public Produit Produit { get; set; }
        public int Quantité { get; set; }

    }
}
