using StockManagement.Application.Features.Users.DTOs;
using System.Collections.Generic;

namespace StockManagement.Application.Features.Roles.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; }                    // Clé primaire
        public string Name { get; set; }                // Nom du rôle

        // Liste des utilisateurs associés à ce rôle
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
