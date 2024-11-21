using StockManagement.Core.Entities;
using StockManagement.Infrastructure;
using MediatR;
using StockManagement.Application.Features.Demandes.DTOs;
using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Enums;

namespace StockManagement.Application.Features.Demandes.Commands
{
    public class CreateDeamandeCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }

        public List<DemandeProduitDTO> ListDemandeProduitDTOs { get; set; }

    }

    public class CreateDeamandeHandler : IRequestHandler<CreateDeamandeCommand, Guid>
    {
        private readonly ApplicationDbContext _context;

        public CreateDeamandeHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateDeamandeCommand request, CancellationToken cancellationToken)
        {
            var demande = new Demande
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                CreatedDate = DateTime.Now 
            };

            demande.DemandeProduits = request.ListDemandeProduitDTOs.Select(dto => new DemandeProduit
            {
                ProduitId = dto.ProduitId,
                Quantité = dto.Quantité
            }).ToList();

            int existingDemandesForYear = 0;
            int currentYear = DateTime.Now.Year;
            try
            {
                existingDemandesForYear = await _context.Demandes
                    .Where(d => d.CreatedDate.HasValue && d.CreatedDate.Value.Year == currentYear)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du comptage des demandes : {ex.Message}");
            }

            demande.DemandeNumber = $"{existingDemandesForYear + 1}/{currentYear}";


            var statusEnCours = await _context.StatusDemandes
                .FirstOrDefaultAsync(s => s.StatusName == StatusNameDemande.EnCoursDeTraitement, cancellationToken);

            if (statusEnCours == null)
                throw new Exception("Le statut 'En cours de traitement' n'existe pas.");

            var historiqueStatus = new HistoriqueStatusDemande
            {
                Id = Guid.NewGuid(),
                DemandeId = demande.Id,
                StatusDemandeId = statusEnCours.Id,
                CreatedDate = DateTime.Now
            };

            _context.Demandes.Add(demande);
            _context.HistoriqueStatusDemandes.Add(historiqueStatus);

            await _context.SaveChangesAsync(cancellationToken);

            return demande.Id;
        }
    }

}
