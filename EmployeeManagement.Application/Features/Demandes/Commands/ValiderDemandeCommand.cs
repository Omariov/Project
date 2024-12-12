using StockManagement.Core.Entities;
using StockManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Enums;

namespace StockManagement.Application.Features.Demandes.Commands
{
    public class ValiderDemandeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class ValiderDemandeHandler : IRequestHandler<ValiderDemandeCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public ValiderDemandeHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ValiderDemandeCommand request, CancellationToken cancellationToken)
        {
            var demande = await _context.Demandes
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (demande == null)
                throw new Exception("La demande spécifiée n'existe pas.");

            var dernierHistorique = await _context.HistoriqueStatusDemandes
                .Where(h => h.DemandeId == demande.Id)
                .OrderByDescending(h => h.CreatedDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (dernierHistorique == null)
                throw new Exception("Aucun historique de statut trouvé pour cette demande.");

            var statusEnCoursDePreparation = await _context.StatusDemandes
                .FirstOrDefaultAsync(s => s.StatusName == StatusNameDemande.EnCoursDePreparation, cancellationToken);

            if (dernierHistorique.StatusDemandeId != statusEnCoursDePreparation?.Id)
                throw new Exception("Le statut actuel n'est pas 'En cours de Preparation'.");

            var statusValide = await _context.StatusDemandes
                .FirstOrDefaultAsync(s => s.StatusName == StatusNameDemande.Valide, cancellationToken);

            if (statusValide == null)
                throw new Exception("Le statut 'Annulé' n'existe pas dans la base de données.");

            var historiqueStatus = new HistoriqueStatusDemande
            {
                Id = Guid.NewGuid(),
                DemandeId = demande.Id,
                StatusDemandeId = statusValide.Id,
                CreatedDate = DateTime.Now
            };

            _context.HistoriqueStatusDemandes.Add(historiqueStatus);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
