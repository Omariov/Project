using StockManagement.Core.Entities;
using StockManagement.Infrastructure;
using MediatR;

namespace StockManagement.Application.Features.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }

    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {
        private readonly ApplicationDbContext _context;

        public CreateEmployeeHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Position = request.Position,
                Salary = request.Salary
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync(cancellationToken);

            return employee.Id;
        }
    }

}
