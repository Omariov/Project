using System;

namespace StockManagement.Core.Entities
{
    public class HistoriqueMouvement : BaseEntity<Guid>
    {

        public Guid ArticleId { get; set; }
        public Article Article { get; set; }

        public string Location { get; set; } // Emplacement actuel de l'article
        public DateTime MovementDate { get; set; }
        public bool Instock{ get; set; }

    }
}
