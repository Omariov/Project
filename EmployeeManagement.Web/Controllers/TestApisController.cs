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
public class TestApisController : Controller
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;
    private readonly ExcelService _excelService;
    private readonly PdfService _pdfService;

    public TestApisController(ApplicationDbContext context, IMediator mediator, ExcelService excelService, PdfService pdfService)
    {
        _context = context;
        _mediator = mediator;
        _excelService = excelService;
        _pdfService = pdfService;

    }

    [Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetAllListDemandes()
    {
        var query = new GetAllListDemandesQuery{};
        var demandes = await _mediator.Send(query);
        return Ok(demandes);
    }

    [Authorize]
    [HttpPost("annuler/{id}")]
    public async Task<IActionResult> AnnulerDemande(Guid id)
    {
        var command = new AnnulerDemandeCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("accepter/{id}")]
    public async Task<IActionResult> AccepterDemande(Guid id)
    {
        var command = new AccepterDemandeCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("refuser/{id}")]
    public async Task<IActionResult> RefuserDemande(Guid id)
    {
        var command = new RefuserDemandeCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("valider/{id}")]
    public async Task<IActionResult> ValiderDemande(Guid id)
    {
        var command = new ValiderDemandeCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

}
