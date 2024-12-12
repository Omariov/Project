using StockManagement.Application.Features.Demandes.DTOs;
using StockManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StockManagement.Application.Features.Demandes.Queries
{
    public class GetDemandeByIdQuery : IRequest<GetDemandeByIdResponseDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetDemandeByIdHandler : IRequestHandler<GetDemandeByIdQuery, GetDemandeByIdResponseDTO>
    {
        private readonly ApplicationDbContext _context;

        public GetDemandeByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetDemandeByIdResponseDTO> Handle(GetDemandeByIdQuery request, CancellationToken cancellationToken)
        {
            // Récupérer la demande correspondante à l'ID
            var demande = await _context.Demandes
                .Include(d => d.User)
                    .ThenInclude(u => u.UserDetails)
                .Include(d => d.User.UserDetails.Service)
                .Include(d => d.User.UserDetails.Service.Division)
                    .ThenInclude(div => div.Direction)
                .Include(d => d.HistoriqueStatusDemandes.OrderByDescending(h => h.CreatedDate).Take(1))
                    .ThenInclude(h => h.StatusDemande)
                .Include(d => d.DemandeProduits)
                    .ThenInclude(dp => dp.Produit)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            // Vérifier si la demande existe
            if (demande == null)
            {
                return null; // ou vous pouvez lever une exception personnalisée.
            }

            // Mapper la demande sur le DTO
            var result = new GetDemandeByIdResponseDTO
            {
                Id = demande.Id,
                Nom = demande.User.UserDetails.Nom,
                Prenom = demande.User.UserDetails.Prenom,
                DirectionNom = demande.User.UserDetails.Service.Division.Direction.Name,
                DivisionNom = demande.User.UserDetails.Service.Division.Name,
                ServiceNom = demande.User.UserDetails.Service.Name,
                StatusDemandeNom = demande.HistoriqueStatusDemandes.FirstOrDefault()?.StatusDemande.StatusName,
                DemandeNumber = demande.DemandeNumber,
                DateDemande=demande.CreatedDate,
                LstProduitQuantiteDTOs = demande.DemandeProduits.Select(dp => new ProduitQuantiteDemandeDTO
                {
                    ProduitNom = dp.Produit.Name,
                    Quantité = dp.Quantité,
                    Unites=dp.Produit.Unites
                }).ToList()
            };

            return result;
        }
    }
}
