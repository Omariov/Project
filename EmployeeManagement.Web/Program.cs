using EmployeeManagement.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Application.Features.Employees.Commands;
using MediatR;
using EmployeeManagement.Web.Services;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO; // Assurez-vous d'importer System.IO

var builder = WebApplication.CreateBuilder(args);

// Créez une instance de PdfTools
var pdfTools = new PdfTools();

// Créez une instance de SynchronizedConverter en utilisant PdfTools
var converter = new SynchronizedConverter(pdfTools);


builder.Services.AddSingleton(typeof(IConverter), converter);
builder.Services.AddSingleton<RazorViewToStringRenderer>();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();


// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MediatR for dependency injection
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateEmployeeCommand>());

// Register ExcelService and PdfService for dependency injection
builder.Services.AddTransient<ExcelService>();
builder.Services.AddTransient<PdfService>();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor(); // Assurez-vous d'ajouter ceci pour utiliser IHttpContextAccessor

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
    context.Response.Redirect("/Index");
    return Task.CompletedTask;
});

app.MapRazorPages();
app.MapControllers();

app.Run();
