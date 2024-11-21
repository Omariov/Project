using System;

namespace StockManagement.Core.Entities
{
    public class HistoriqueMouvement : BaseEntity<Guid>
    {

        public Guid ArticleId { get; set; }
        public Article Article { get; set; }

        public Guid EmplacementId { get; set; }
        public Emplacement Emplacement { get; set; }
        public DateTime MovementDate { get; set; }

    }
}
