using StockManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagement.Application.Features.Demandes.DTOs
{
   
        public class DemandeProduitDTO 
        {
            public Guid ProduitId { get; set; }
            public int Quantité { get; set; }
        }
    

}
