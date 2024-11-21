using System;

namespace StockManagement.Core.Entities
{
    public class Emplacement : BaseEntity<Guid>
    {

        public Guid DirectionId { get; set; }
        public Direction Direction { get; set; }

        public ICollection<HistoriqueMouvement> HistoriqueMouvements { get; set; } = new List<HistoriqueMouvement>();


    }
}
