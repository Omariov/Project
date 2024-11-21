using StockManagement.Core.Entities;
using StockManagement.Application.Services; // Ajoutez cette ligne pour importer votre service
using StockManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace StockManagement.Application.Features.Models.Commands
{
    public class RegisterCommand : IRequest<Guid>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public int RoleId { get; set; } // Si vous gérez les rôles
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHashage _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor; // Ajout pour accéder au contexte HTTP


        public RegisterCommandHandler(ApplicationDbContext context, IPasswordHashage passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
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

            var createdBy = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            // Création de l'utilisateur
            var user = new User
            {
                Id = new Guid(),
                Username = request.Username,
                PasswordHash = passwordHash,
                RoleId = request.RoleId,
                CreatedBy = createdBy
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }
    }

}
