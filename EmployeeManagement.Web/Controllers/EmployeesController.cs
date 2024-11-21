using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockManagement.Application.Features.Employees.Queries;
using StockManagement.Core.Models;
using StockManagement.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using StockManagement.Web.Services;
using StockManagement.Application.Services;
using StockManagement.Infrastructure;
using StockManagement.Application.Features.Employees.DTOs;
using StockManagement.Application.Features.Employees.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using StockManagement.Application.Features.Models.Commands;
using StockManagement.Core.Enums;

namespace StockManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ExcelService _excelService;
        private readonly PdfService _pdfService;
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHashage _passwordHasher;

        public EmployeesController(ApplicationDbContext context, IPasswordHashage passwordHasher, IMediator mediator, ExcelService excelService, PdfService pdfService)
        {
            _mediator = mediator;
            _excelService = excelService;
            _pdfService = pdfService;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        #region Account


        [HttpGet("", Name = "AcceuilRoute")]
        public IActionResult Acceuil()
        {
            return View("Acceuil");
        }


        [HttpGet("Login")]
        public IActionResult Login()
        {
            var model = new LoginModel();
            return View("Login");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            ViewBag.Roles = _context.Roles.ToList();
            return View();
        }




        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterCommand model)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(model);
            }

            try
            {
                await _mediator.Send(model); // Envoie directement la commande
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Username", ex.Message);
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(model);
            }
            return RedirectToAction("Acceuil");

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginCommand model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isAuthenticated;
            string userRole = "user"; // Par exemple, récupérer le rôle de l'utilisateur depuis la base de données
            try
            {
                isAuthenticated = await _mediator.Send(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            if (isAuthenticated)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user != null)
                {
                    userRole = user.RoleId.ToString();
                }

                var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, model.Username),
        new Claim(ClaimTypes.Role, userRole),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // ID utilisateur comme NameIdentifier
    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("GetListDemandesByUserId", "Demandes");
            }


            ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
            return View(model);
        }


        [Authorize]
        [HttpGet("EmployeesList")]
        public async Task<IActionResult> EmployeesList()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "1") // 1 correspond à SuperAdmin
            {
                var query = new GetEmployeesQuery();
                var employees = await _mediator.Send(query);
                return View(employees);
            }
            else
            {
                return View("AccessDenied"); // Assurez-vous que cette vue existe
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Déconnecter l'utilisateur en supprimant les cookies d'authentification
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Rediriger l'utilisateur vers la page d'accueil
            return RedirectToAction("Acceuil", "Employees");
        }















        // POST: api/employees/register


        #endregion Account

        #region Crud Employees

        // GET: api/employees/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
        {
            var query = new GetEmployeesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // GET: api/employees/get-by-id/{id}
        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(Guid id)
        {
            var query = new GetEmployeeByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(); // Employé non trouvé
            }

            return Ok(result);
        }

        // POST: api/employees/create
        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateEmployee(CreateEmployeeCommand command)
        {
            var employeeId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, employeeId);
        }

        // PUT: api/employees/update/{id}
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateEmployee(Guid id, UpdateEmployeeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(); // ID ne correspond pas
            }

            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound(); // Employé non trouvé
            }

            return NoContent(); // Mise à jour réussie
        }

        // DELETE: api/employees/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            var command = new DeleteEmployeeCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound(); // Employé non trouvé
            }

            return NoContent(); // Suppression réussie
        }

        #endregion Crud Employees

        #region PDF and EXCEL

        // GET: api/employees/export
        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            var employees = await _mediator.Send(new GetEmployeesQuery());
            var excelData = await _excelService.ExportEmployeesToExcel(employees);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Employees.xlsx";

            return File(excelData, contentType, fileName);
        }

        // POST: api/employees/import
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

            // Logique pour ajouter les employés à la base de données avec MediatR

            return Ok(employees); // Retourne la liste importée pour vérification
        }

        // GET: api/employees/export/pdf
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            try
            {
                var employees = await _mediator.Send(new GetEmployeesQuery());
                var pdfData = _pdfService.ExportEmployeesToPdf(employees); // Appel synchronisé

                var contentType = "application/pdf";
                var fileName = "Employees.pdf";

                return File(pdfData, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion PDF and EXCEL
    }
}
