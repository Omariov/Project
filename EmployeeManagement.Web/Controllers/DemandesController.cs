using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Features.Demandes.Commands;
using StockManagement.Application.Features.Demandes.DTOs;
using StockManagement.Application.Features.Demandes.Queries;
using StockManagement.Application.Features.Employees.Queries;
using StockManagement.Application.Features.Roles.Queries;
using StockManagement.Core.Entities;
using StockManagement.Infrastructure;
using StockManagement.Web.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("[controller]")]
[ApiController]
public class DemandesController : Controller
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;
    private readonly ExcelService _excelService;
    private readonly PdfService _pdfService;

    public DemandesController(ApplicationDbContext context, IMediator mediator, ExcelService excelService, PdfService pdfService)
    {
        _context = context;
        _mediator = mediator;
        _excelService = excelService;
        _pdfService = pdfService;

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

        return View("DemandesListRequester", demandes);
    }

    [Authorize]
    [HttpGet("GetAllListDemandes")]
    public async Task<IActionResult> GetAllListDemandes()
    {
        var userRoleString = User.FindFirst(ClaimTypes.Role)?.Value;
        if (!int.TryParse(userRoleString, out int userRole))
        {
            return View("AccessDenied");
        }
/*
        var queryRole = new GetRolesQuery();
        var roles = await _mediator.Send(queryRole);

        var role = roles.FirstOrDefault(a => a.Id == userRole);
        string roleName = role?.Name ?? "Unknown";*/

        if (userRole == 1 || userRole==2) 
        {
            var query = new GetAllListDemandesQuery();
            var demandes = await _mediator.Send(query);
            return View("DemandesList", demandes);
        }
        else
        {
            return View("AccessDenied");
        }
    }


    [Authorize]
    [HttpGet("CreateDemande")]
    public IActionResult CreateDemande()
    {
        var model = new CreateDemandeDTO
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            UserName= User.FindFirst(ClaimTypes.Name)?.Value,
            AvailableProduits = _context.Produits.Distinct().Select(p => new ProduitDTO { Id = p.Id, Name = p.Name }).ToList()
        };

        return View(model);
    }

    [Authorize]
    [HttpPost("CreateDemande")]
    public async Task<IActionResult> CreateDemande([FromForm] CreateDemandeDTO model)
    {
        var UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var UserName = User.FindFirst(ClaimTypes.Name)?.Value;

        model.AvailableProduits = model.AvailableProduits?
            .Where(p => p.Quantité.HasValue && p.Quantité > 0) 
            .ToList();

        if (model.AvailableProduits == null || !model.AvailableProduits.Any())
        {
            ModelState.AddModelError(string.Empty, "Veuillez sélectionner au moins un produit avec une quantité.");
            model.AvailableProduits = await _context.Produits
                .Select(p => new ProduitDTO { Id = p.Id, Name = p.Name })
                .ToListAsync();
            model.UserName = UserName;
            return View(model);
        }

        if (!ModelState.IsValid)
        {
            model.AvailableProduits = await _context.Produits
                .Select(p => new ProduitDTO { Id = p.Id, Name = p.Name })
                .ToListAsync();
            return View(model);
        }

        model.UserId = UserId;

        var command = new CreateDemandeCommand { data = model };
        await _mediator.Send(command);

        return RedirectToAction("GetListDemandesByUserId");
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Utilisateur non authentifié ou ID invalide.");
        }
        var query = new GetListDemandesByUserIdQuery { Id = userId };
        var demandes = await _mediator.Send(query);

        var excelData = await _excelService.ExportDemandesToExcel(demandes);
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileName = "Demandes.xlsx";

        return File(excelData, contentType, fileName);
    }


    [HttpGet]
    public async Task<IActionResult> ExportDemandeToPdf(Guid demandeId)
    {
        var query = new GetDemandeByIdQuery { Id = demandeId };
        var demande = await _mediator.Send(query);
        if (demande == null)
        {
            return NotFound("Demande introuvable");
        }

        var pdfBytes = await _pdfService.GenerateDemandePdfAsync(demande);

        return File(pdfBytes, "application/pdf", $"Demande_{demande.DemandeNumber}.pdf");
    }




}
