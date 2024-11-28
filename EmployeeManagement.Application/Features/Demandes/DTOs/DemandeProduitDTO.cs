using StockManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagement.Application.Features.Demandes.DTOs
{

    public class CreateDemandeDTO
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public List<ProduitDTO> AvailableProduits { get; set; }
    }

    public class ProduitDTO
    {
        public Guid Id { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être un nombre positif.")]
        public int? Quantité { get; set; }
        public string? Name { get; set; }
    }



}
