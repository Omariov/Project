namespace StockManagement.Core.Entities
{
    public class StockDetail : BaseEntity<Guid>
    {
        public int Quantity { get; set; }
        public Guid ProduitId { get; set; }
        public Produit Produit { get; set; }

    }
}
