namespace StockManagement.Core.Entities
{
    public class Direction : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public Guid RegionId { get; set; }
        public Region Region { get; set; }

    }
}
