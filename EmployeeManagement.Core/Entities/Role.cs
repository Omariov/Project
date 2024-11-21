using System.Collections.Generic;

namespace StockManagement.Core.Entities
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
