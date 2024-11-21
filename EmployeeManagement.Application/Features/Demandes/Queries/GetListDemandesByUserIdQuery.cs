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
    public class GetListDemandesByUserIdQuery : IRequest<List<GetListDemandesByUserIdResponseDTO>>
    {
        public Guid Id { get; set; }
    }

    public class GetListDemandesByUserIdHandler : IRequestHandler<GetListDemandesByUserIdQuery, List<GetListDemandesByUserIdResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetListDemandesByUserIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetListDemandesByUserIdResponseDTO>> Handle(GetListDemandesByUserIdQuery request, CancellationToken cancellationToken)
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
                .Where(d => d.UserId == request.Id)
                .ToListAsync(cancellationToken);

            if (demandes == null || !demandes.Any())
            {
                return new List<GetListDemandesByUserIdResponseDTO>(); 
            }

            var result = demandes.Select(d => new GetListDemandesByUserIdResponseDTO
            {
                Id = d.Id,
                Nom = d.User.UserDetails.Nom,
                Prenom = d.User.UserDetails.Prenom,
                DirectionNom = d.User.UserDetails.Service.Division.Direction.Name,
                DivisionNom = d.User.UserDetails.Service.Division.Name,
                ServiceNom = d.User.UserDetails.Service.Name,
                StatusDemandeNom = d.HistoriqueStatusDemandes.FirstOrDefault()?.StatusDemande.StatusName,

                LstProduitQuantiteDTOs = d.DemandeProduits.Select(dp => new ProduitQuantiteDTO
                {
                    ProduitNom = dp.Produit.Name,
                    Quantité = dp.Quantité
                }).ToList()

            }).ToList();

            return result;
        }
    }
}
