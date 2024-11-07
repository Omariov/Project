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
        public string ConfirmPassword { get; set; }

        public int RoleId { get; set; } // Si vous gérez les rôles
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHashage _passwordHasher;

        public RegisterCommandHandler(ApplicationDbContext context, IPasswordHashage passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Vérifier si l'utilisateur existe déjà
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (existingUser != null)
            {
                throw new Exception("Le nom d'utilisateur existe déjà.");
            }

            // Vérifier que le mot de passe n'est pas vide
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new Exception("Le mot de passe ne peut pas être vide.");
            }


            if (request.Password != request.ConfirmPassword)
            {
                throw new Exception("Le mot de passe ne peut pas être vide.");
            }
            // Hachage du mot de passe
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // Création de l'utilisateur
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                RoleId = request.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }
    }

}
