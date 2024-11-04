using System;

namespace EmployeeManagement.Application.Features.Users.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }                    // Clé primaire
        public string Username { get; set; }           // Nom d'utilisateur
        public int RoleId { get; set; }                 // ID du rôle associé
    }
}
