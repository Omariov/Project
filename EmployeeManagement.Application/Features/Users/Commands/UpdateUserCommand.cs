using StockManagement.Core.Entities;
using StockManagement.Application.Services;
using StockManagement.Infrastructure;
using MediatR;

namespace StockManagement.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } 
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
                return false; 
            }

            user.Username = request.Username;
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = _IpasswordHasher.HashPassword(request.Password); 
            }
            user.RoleId = request.RoleId;

            await _context.SaveChangesAsync(cancellationToken);
            return true; 
        }

    }
}
