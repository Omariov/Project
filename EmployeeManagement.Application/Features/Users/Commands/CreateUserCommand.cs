﻿using EmployeeManagement.Core.Entities;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Infrastructure;
using MediatR;

namespace EmployeeManagement.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<int>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } // Référence au rôle
    }

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHashage _IpasswordHasher;
        public CreateUserHandler(ApplicationDbContext context, IPasswordHashage IpasswordHasher)
        {
            _context = context;
            _IpasswordHasher =IpasswordHasher;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = request.Username,
                PasswordHash = _IpasswordHasher.HashPassword(request.Password), // Assure-toi d'avoir une méthode pour hacher le mot de passe
                RoleId = request.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }

    }
}
