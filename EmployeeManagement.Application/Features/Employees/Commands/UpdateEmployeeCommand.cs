using EmployeeManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Features.Employees.Commands
{
    public class UpdateEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }

    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public UpdateEmployeeHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (employee == null)
            {
                return false; // Employé non trouvé
            }

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Position = request.Position;
            employee.Salary = request.Salary;

            await _context.SaveChangesAsync(cancellationToken);

            return true; // Mise à jour réussie
        }
    }

}
