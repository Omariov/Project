using EmployeeManagement.Application.Features.Users.DTOs;
using EmployeeManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
    }

    public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetUsersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                RoleId = user.RoleId
            }).ToListAsync(cancellationToken);
        }
    }
}
