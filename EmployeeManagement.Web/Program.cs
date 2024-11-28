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

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile("Logs/log-{Date}.txt"); 

var pdfTools = new PdfTools();
var converter = new SynchronizedConverter(pdfTools);

builder.Services.AddSingleton<IConverter>(converter);
builder.Services.AddScoped<RazorViewToStringRenderer>();

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPasswordHashage, PasswordHashage>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/employees/login";
        options.AccessDeniedPath = "/employees/accessdenied";
    });

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateEmployeeCommand>());

builder.Services.AddTransient<ExcelService>();
builder.Services.AddTransient<PdfService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API V1");
    c.RoutePrefix = "swagger";
});

app.MapGet("/", context =>
{
    context.Response.Redirect("/employees");
    return Task.CompletedTask;
});

app.MapRazorPages();
app.MapControllers();
app.Run();
