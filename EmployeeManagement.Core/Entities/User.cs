using StockManagement.Core.Entities;
using System.Collections.Generic;

namespace StockManagement.Core.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public UserDetail? UserDetails { get; set; }
        public ICollection<Demande>? Demandes { get; set; } = new List<Demande>();
    }
}
