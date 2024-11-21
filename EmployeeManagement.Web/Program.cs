using DinkToPdf;
using DinkToPdf.Contracts;
using StockManagement.Application.Features.Employees.Commands;
using StockManagement.Application.Services;
using StockManagement.Infrastructure;
using StockManagement.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog pour écrire les logs dans un fichier
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile("Logs/log-{Date}.txt"); // Les logs seront enregistrés dans Logs avec un fichier par jour

// Créez une instance de PdfTools
var pdfTools = new PdfTools();
var converter = new SynchronizedConverter(pdfTools);

builder.Services.AddSingleton<IConverter>(converter);
builder.Services.AddSingleton<RazorViewToStringRenderer>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Enregistrement du PasswordHasher
builder.Services.AddScoped<IPasswordHashage, PasswordHashage>();

// Configure Entity Framework avec SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration de l'authentification par cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/employees/login";
        options.AccessDeniedPath = "/employees/accessdenied";
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
    c.RoutePrefix = "swagger";
});

// Set default route to load Index.cshtml
app.MapGet("/", context =>
{
    context.Response.Redirect("/employees");
    return Task.CompletedTask;
});

app.MapRazorPages();
app.MapControllers();
app.Run();
