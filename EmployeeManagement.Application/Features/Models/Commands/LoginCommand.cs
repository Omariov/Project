using EmployeeManagement.Core.Entities;
using EmployeeManagement.Application.Services; // Ajoutez cette ligne pour importer votre service
using EmployeeManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Features.Models.Commands
{
    public class LoginCommand : IRequest<bool>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHashage _passwordHasher; // Déclaration de l'interface pour le hashage

        public LoginCommandHandler(ApplicationDbContext context, IPasswordHashage passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher; // Injection de dépendance
        }

        public async Task<bool> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user != null && _passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
            {
                return true; // Connexion réussie
            }
            return false; // Échec de la connexion
        }
    }
}
