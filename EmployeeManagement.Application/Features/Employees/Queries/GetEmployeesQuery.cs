using StockManagement.Application.Features.Employees.DTOs;
using StockManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace StockManagement.Application.Features.Employees.Queries
{
    public class GetEmployeesQuery : IRequest<List<EmployeeDto>>
    {
    }

    public class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetEmployeesHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Position = e.Position,
                    Salary = e.Salary
                })
                .ToListAsync(cancellationToken);
        }
    }
}
