using StockManagement.Core.Entities;
using StockManagement.Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Features.Roles.DTOs;

namespace StockManagement.Application.Features.Roles.Queries
{
    public class GetRolesQuery : IRequest<List<RoleDto>>
    {
    }

    public class GetRolesHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetRolesHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _context.Roles.ToListAsync(cancellationToken);
            var roleDtos = new List<RoleDto>();

            foreach (var role in roles)
            {
                roleDtos.Add(new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    // Vous pouvez éventuellement remplir la liste des utilisateurs ici si nécessaire
                });
            }

            return roleDtos;
        }
    }
}
