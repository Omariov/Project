namespace StockManagement.Core.Entities
{
    public class Article : BaseEntity<Guid>
    {

        public Guid ProduitId { get; set; }
        public Produit Produit { get; set; }
        public ICollection<HistoriqueMouvement> HistoriqueMouvements { get; set; } = new List<HistoriqueMouvement>();
    }
}
