﻿namespace EmployeeManagement.Core.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; } // Pour sélectionner le rôle lors de l'inscription
    }
}