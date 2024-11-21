using System;

namespace StockManagement.Application.Features.Users.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }                    // Clé primaire
        public string Username { get; set; }           // Nom d'utilisateur
        public int RoleId { get; set; }                 // ID du rôle associé
    }
}
