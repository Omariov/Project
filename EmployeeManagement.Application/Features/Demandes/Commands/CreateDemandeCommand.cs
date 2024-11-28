using StockManagement.Core.Entities;
using StockManagement.Infrastructure;
using MediatR;
using StockManagement.Application.Features.Demandes.DTOs;
using Microsoft.EntityFrameworkCore;
using StockManagement.Core.Enums;

namespace StockManagement.Application.Features.Demandes.Commands
{
    public class CreateDemandeCommand : IRequest<Guid>
    {
        public CreateDemandeDTO data { get; set; }

    }

    public class CreateDeamandeHandler : IRequestHandler<CreateDemandeCommand, Guid>
    {
        private readonly ApplicationDbContext _context;

        public CreateDeamandeHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateDemandeCommand request, CancellationToken cancellationToken)
        {
            var demande = new Demande
            {
                Id = Guid.NewGuid(),
                UserId = request.data.UserId,
                CreatedDate = DateTime.Now 
            };

            demande.DemandeProduits = request.data.AvailableProduits.Select(dto => new DemandeProduit
            {
                ProduitId = dto.Id,
                Quantité = (int)dto.Quantité
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
