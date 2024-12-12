namespace StockManagement.Core.Entities
{
    public class Sortie : BaseEntity<Guid>
    {

        public Guid DemandeId { get; set; }
        public Demande Demande { get; set; }
        public DateTime DateLivraison{ get; set; }
        public string? Objet { get; set; }
        public string SortieNumber { get; set; }


        public ICollection<SortieProduit> SortieProduits { get; set; } = new List<SortieProduit>();

    }
}
