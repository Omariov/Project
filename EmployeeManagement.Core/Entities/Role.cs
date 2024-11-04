using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Entities
{
    public class Role
    {
        public int Id { get; set; }             // Clé primaire
        public string Name { get; set; }         // Nom du rôle (ex. "SuperAdmin", "Manager", "Requester")

        // Navigation property
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
