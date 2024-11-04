namespace EmployeeManagement.Core.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        // Ajoutez cette propriété pour permettre le rappel
        public bool RememberMe { get; set; }
    }
}
