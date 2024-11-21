namespace StockManagement.Core.Entities
{
    public class Division : BaseEntity<Guid>
    {
        public string Name { get; set; }

        public Guid DirectionId { get; set; }
        public Direction Direction { get; set; }
    }
}
