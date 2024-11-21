using StockManagement.Core.Entities;

namespace StockManagement.Core.Entities {
    public class Employee : BaseEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }
}