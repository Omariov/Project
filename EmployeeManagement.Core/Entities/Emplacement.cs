using System;

namespace StockManagement.Core.Entities
{
    public class Emplacement : BaseEntity<Guid>
    {

        public string Name { get; set; }

        public ICollection<HistoriqueMouvement> HistoriqueMouvements { get; set; } = new List<HistoriqueMouvement>();


    }
}
