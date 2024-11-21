namespace StockManagement.Core.Entities
{
    public class Service : BaseEntity<Guid>
    {
        public string Name { get; set; }

        public Guid DivisionId { get; set; }
        public Division Division { get; set; }
    }
}
