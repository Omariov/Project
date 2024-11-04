using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services
{
    public interface IPasswordHashage
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }
}
