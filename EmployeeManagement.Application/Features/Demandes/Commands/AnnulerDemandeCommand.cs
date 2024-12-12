﻿using StockManagement.Core.Entities;
using StockManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Enums;

namespace StockManagement.Application.Features.Demandes.Commands
{
    public class AnnulerDemandeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class AnnulerDemandeHandler : IRequestHandler<AnnulerDemandeCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public AnnulerDemandeHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AnnulerDemandeCommand request, CancellationToken cancellationToken)
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

            var statusEnCours = await _context.StatusDemandes
                .FirstOrDefaultAsync(s => s.StatusName == StatusNameDemande.DemandeEnvoye, cancellationToken);

            if (dernierHistorique.StatusDemandeId != statusEnCours?.Id)
                throw new Exception("Le statut actuel n'est pas 'En cours de traitement'.");

            var statusAnnule = await _context.StatusDemandes
                .FirstOrDefaultAsync(s => s.StatusName == StatusNameDemande.Annulee, cancellationToken);

            if (statusAnnule == null)
                throw new Exception("Le statut 'Annulé' n'existe pas dans la base de données.");

            var historiqueStatus = new HistoriqueStatusDemande
            {
                Id = Guid.NewGuid(),
                DemandeId = demande.Id,
                StatusDemandeId = statusAnnule.Id,
                CreatedDate = DateTime.Now
            };

            _context.HistoriqueStatusDemandes.Add(historiqueStatus);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
