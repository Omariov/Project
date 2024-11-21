namespace StockManagement.Core.Entities
{
    public class Sortie : BaseEntity<Guid>
    {

        public Guid DemandeId { get; set; }
        public Demande Demande { get; set; }
    }
}
