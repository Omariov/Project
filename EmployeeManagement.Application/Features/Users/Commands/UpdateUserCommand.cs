using EmployeeManagement.Core.Entities;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Infrastructure;
using MediatR;

namespace EmployeeManagement.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } // Référence au rôle
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHashage _IpasswordHasher;


        public UpdateUserHandler(ApplicationDbContext context, IPasswordHashage IpasswordHasher)
        {
            _context = context;
            _IpasswordHasher = IpasswordHasher;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.Id);

            if (user == null)
            {
                return false; // Utilisateur non trouvé
            }

            user.Username = request.Username;
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = _IpasswordHasher.HashPassword(request.Password); // Hachage si le mot de passe est fourni
            }
            user.RoleId = request.RoleId;

            await _context.SaveChangesAsync(cancellationToken);
            return true; // Mise à jour réussie
        }

    }
}
