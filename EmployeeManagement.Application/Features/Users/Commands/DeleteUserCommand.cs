using StockManagement.Infrastructure;
using MediatR;

namespace StockManagement.Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteUserHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null)
            {
                return false; // Utilisateur non trouvé
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
            return true; // Suppression réussie
        }
    }
}
