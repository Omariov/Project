using StockManagement.Core.Entities;
using StockManagement.Application.Services; // Ajoutez cette ligne pour importer votre service
using StockManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace StockManagement.Application.Features.Models.Commands
{
    public class LoginCommand : IRequest<bool>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }


    public class LoginCommandHandler : IRequestHandler<LoginCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHashage _passwordHasher;

        public LoginCommandHandler(ApplicationDbContext context, IPasswordHashage passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            return user != null && _passwordHasher.VerifyPassword(user.PasswordHash, request.Password);
        }
    }

}
