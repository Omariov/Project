using System.Collections.Generic;

namespace StockManagement.Core.Entities
{
    public class Demande : BaseEntity<Guid>
    {
      

        public Guid UserId { get; set; }
        public User User { get; set; }
        public string DemandeNumber { get; set; }


        public ICollection<HistoriqueStatusDemande> HistoriqueStatusDemandes { get; set; } = new List<HistoriqueStatusDemande>();
        public ICollection<DemandeProduit> DemandeProduits { get; set; } = new List<DemandeProduit>();
    }
}
