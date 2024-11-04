using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Application.Features.Employees.Queries;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Web.Services;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Infrastructure;
using EmployeeManagement.Application.Features.Employees.DTOs;
using EmployeeManagement.Application.Features.Employees.Commands;

namespace EmployeeManagement.Web.Controllers
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

        // Page d'accueil redirigeant vers la page de connexion
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Login"); // Redirige vers la vue de connexion
        }

        // GET: api/employees/login
        [HttpGet("login")]
        public IActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        // POST: api/employees/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user != null && _passwordHasher.VerifyPassword(user.PasswordHash, model.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Redirection vers la page EmployeesList après la connexion
                    return RedirectToAction("EmployeesList", "Employees");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        // GET: api/employees/employeesList
        [HttpGet("employeesList")]
        public async Task<IActionResult> EmployeesList()
        {
            var query = new GetEmployeesQuery();
            var result = await _mediator.Send(query);
            return View("EmployeesList", result); // Envoie la liste des employés à la vue EmployeesList
        }

        // POST: api/employees/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: api/employees/register
        [HttpGet("register")]
        public IActionResult Register()
        {
            ViewBag.Roles = _context.Roles.ToList();
            return View();
        }

        // POST: api/employees/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Username already exists.");
                    return View(model);
                }

                var passwordHash = _passwordHasher.HashPassword(model.Password);

                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = passwordHash,
                    RoleId = model.RoleId
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }

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
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
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
        public async Task<ActionResult> UpdateEmployee(int id, UpdateEmployeeCommand command)
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
        public async Task<ActionResult> DeleteEmployee(int id)
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
