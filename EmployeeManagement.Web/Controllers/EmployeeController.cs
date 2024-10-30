using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Application.Features.Employees.Queries;
using EmployeeManagement.Web.Services;
using System.Threading.Tasks;
using MediatR;

[Route("Employee")]
public class EmployeeController : Controller
{
    private readonly IMediator _mediator;
    private readonly ExcelService _excelService;
    private readonly PdfService _pdfService;

    public EmployeeController(IMediator mediator, ExcelService excelService, PdfService pdfService)
    {
        _mediator = mediator;
        _excelService = excelService;
        _pdfService = pdfService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var employees = await _mediator.Send(new GetEmployeesQuery());
        return View(employees); // Passez la liste des employés au modèle de la vue
    }


    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        var employees = await _mediator.Send(new GetEmployeesQuery());
        var excelData = await _excelService.ExportEmployeesToExcel(employees);
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileName = "Employees.xlsx";

        return File(excelData, contentType, fileName);
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Aucun fichier sélectionné.");
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;

        var employees = await _excelService.ImportEmployeesFromExcel(stream);

        // Vous pouvez maintenant ajouter des employés à la base de données ici en utilisant MediatR

        return Ok(employees); // Retourner la liste importée pour vérification
    }


    [HttpGet("export/pdf")]
    public async Task<IActionResult> ExportPdf()
    {
        try
        {
            var employees = await _mediator.Send(new GetEmployeesQuery());
            var pdfData = await _pdfService.ExportEmployeesToPdf(employees); // Attendez ici si ExportEmployeesToPdf est une tâche
            var contentType = "application/pdf";
            var fileName = "Employees.pdf";

            // La méthode File attend un tableau d'octets, donc assurez-vous de passer pdfData comme un byte[]
            return File(pdfData, contentType, fileName, false); // Ajoutez false pour indiquer que c'est un fichier à servir
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }



}
