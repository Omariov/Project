using EmployeeManagement.Core.Entities;
using EmployeeManagement.Application.Services; // Ajoutez cette ligne pour importer votre service
using EmployeeManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Features.Models.Commands
{
    public class RegisterCommand : IRequest<int>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } // Si vous gérez les rôles
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHashage _passwordHasher; // Déclaration de l'interface pour le hashage

        public RegisterCommandHandler(ApplicationDbContext context, IPasswordHashage passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher; // Injection de dépendance
        }

        public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (existingUser != null)
            {
                throw new Exception("Username already exists."); // Gérer les erreurs de manière appropriée
            }

            // Utilisation de votre PasswordHasher pour hasher le mot de passe
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                RoleId = request.RoleId // Ajoutez une logique par défaut si nécessaire
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id; // Retourner l'ID de l'utilisateur nouvellement créé
        }
    }
}
