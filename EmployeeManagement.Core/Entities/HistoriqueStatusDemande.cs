using System;

namespace StockManagement.Core.Entities
{
    public class HistoriqueStatusDemande : BaseEntity<Guid>
    {

        public Guid StatusDemandeId { get; set; }
        public StatusDemande StatusDemande { get; set; }

        public Guid DemandeId { get; set; }
        public Demande Demande { get; set; }

    }
}
