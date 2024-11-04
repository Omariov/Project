using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Entities
{
    public class User
    {
        public int Id { get; set; }                    // Clé primaire
        public string Username { get; set; }           // Nom d'utilisateur pour la connexion
        public string PasswordHash { get; set; }       // Mot de passe haché

        // Relation avec Role (clé étrangère)
        public int RoleId { get; set; }
        public Role Role { get; set; }                 // Navigation vers le rôle
    }
}
