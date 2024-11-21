using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.Features.Demandes.Commands;
using StockManagement.Application.Features.Demandes.DTOs;
using StockManagement.Application.Features.Demandes.Queries;
using StockManagement.Core.Entities;
using StockManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DemandesController : Controller
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;

    public DemandesController(ApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet("GetListDemandesByUserId")]
    public async Task<IActionResult> GetListDemandesByUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Utilisateur non authentifié ou ID invalide.");
        }

        var query = new GetListDemandesByUserIdQuery { Id = userId };
        var demandes = await _mediator.Send(query);

        if (demandes == null || !demandes.Any())
        {
            return NotFound("Aucune demande trouvée pour cet utilisateur.");
        }

        return Ok(demandes); 
    }

    [Authorize]
    [HttpPost("CreateDemande")]
    public async Task<IActionResult> CreateDemande([FromBody] CreateDeamandeCommand request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Utilisateur non authentifié ou ID invalide.");
        }

        if (request.UserId != userId)
        {
            return BadRequest("L'ID utilisateur dans la demande ne correspond pas à l'utilisateur authentifié.");
        }

        if (request.ListDemandeProduitDTOs == null || !request.ListDemandeProduitDTOs.Any())
        {
            return BadRequest("La liste des produits est vide.");
        }

        var demandeId = await _mediator.Send(request);

        return CreatedAtAction(nameof(GetListDemandesByUserId), new { id = demandeId }, null);
    }
}
