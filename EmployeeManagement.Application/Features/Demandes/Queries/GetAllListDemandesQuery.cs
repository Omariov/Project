using StockManagement.Application.Features.Employees.DTOs;
using StockManagement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Features.Demandes.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace StockManagement.Application.Features.Demandes.Queries
{
    public class GetAllListDemandesQuery : IRequest<List<GetAllListDemandesResponseDTO>>
    {
    }

    public class GetAllListDemandesQueryHandler : IRequestHandler<GetAllListDemandesQuery, List<GetAllListDemandesResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllListDemandesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllListDemandesResponseDTO>> Handle(GetAllListDemandesQuery request, CancellationToken cancellationToken)
        {
            var demandes = await _context.Demandes
                .Include(d => d.User)
                    .ThenInclude(u => u.UserDetails)
                .Include(d => d.User.UserDetails.Service)
                .Include(d => d.User.UserDetails.Service.Division)
                    .ThenInclude(div => div.Direction)
                .Include(d => d.HistoriqueStatusDemandes.OrderByDescending(h => h.CreatedDate).Take(1))
                    .ThenInclude(h => h.StatusDemande)
                .Include(d => d.DemandeProduits)
                    .ThenInclude(dp => dp.Produit)
                .ToListAsync(cancellationToken);
            var DemandeTris = demandes.OrderByDescending(a => a.CreatedDate);
            if (DemandeTris == null || !DemandeTris.Any())
            {
                return new List<GetAllListDemandesResponseDTO>();
            }

            var result = DemandeTris.Select(d => new GetAllListDemandesResponseDTO
            {
                Id = d.Id,
                Nom = d.User.UserDetails.Nom,
                Prenom = d.User.UserDetails.Prenom,
                DirectionNom = d.User.UserDetails.Service.Division.Direction.Name,
                DivisionNom = d.User.UserDetails.Service.Division.Name,
                ServiceNom = d.User.UserDetails.Service.Name,
                StatusDemandeNom = d.HistoriqueStatusDemandes.FirstOrDefault()?.StatusDemande.StatusName,
                DemandeNumber = d.DemandeNumber,
                LstAllProduitQuantiteDTOs = d.DemandeProduits.Select(dp => new AllProduitQuantiteDTO
                {
                    ProduitNom = dp.Produit.Name,
                    Quantité = dp.Quantité
                }).ToList()

            }).ToList();

            return result;
        }
    }
}
