using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Core.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } // Cette propriété doit être booléenne
    }


}
