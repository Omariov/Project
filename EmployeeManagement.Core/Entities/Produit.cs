namespace StockManagement.Core.Entities
{
    public class Produit : BaseEntity<Guid>
    {
        public string Name { get; set; }

        public ICollection<Article> Articles { get; set; } = new List<Article>();
        

    }
}
