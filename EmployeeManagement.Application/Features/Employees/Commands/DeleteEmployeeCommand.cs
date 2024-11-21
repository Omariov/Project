using StockManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace StockManagement.Application.Features.Employees.Commands
{
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteEmployeeHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (employee == null)
            {
                return false; // Employé non trouvé
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync(cancellationToken);

            return true; // Suppression réussie
        }
    }

}
