using DinkToPdf;
using DinkToPdf.Contracts;
using EmployeeManagement.Application.Features.Employees.Commands;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Infrastructure;
using EmployeeManagement.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Créez une instance de PdfTools
var pdfTools = new PdfTools();

// Créez une instance de SynchronizedConverter en utilisant PdfTools
var converter = new SynchronizedConverter(pdfTools);

builder.Services.AddSingleton<IConverter>(converter);
builder.Services.AddSingleton<RazorViewToStringRenderer>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Enregistrement du PasswordHasher
builder.Services.AddScoped<IPasswordHashage, PasswordHashage>();

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration de l'authentification par cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/employees/login"; // Chemin de la page de connexion
        options.AccessDeniedPath = "/employees/accessdenied"; // Chemin pour l'accès refusé
    });

// Add MediatR for dependency injection
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateEmployeeCommand>());

// Register ExcelService and PdfService for dependency injection
builder.Services.AddTransient<ExcelService>();
builder.Services.AddTransient<PdfService>();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Activez l'authentification
app.UseAuthentication();
app.UseAuthorization();

// Enable middleware to serve generated Swagger as a JSON endpoint at /swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API V1");
    c.RoutePrefix = "swagger"; // Met `swagger` comme préfixe de l'URL pour éviter la redirection automatique vers Swagger
});

// Set default route to load Index.cshtml
app.MapGet("/", context =>
{
    context.Response.Redirect("/employees"); // Modifiez pour rediriger vers la page des employés par défaut
    return Task.CompletedTask;
});

app.MapRazorPages();
app.MapControllers();
app.Run();
