using DinkToPdf;
using DinkToPdf.Contracts;
using EmployeeManagement.Application.Features.Employees.DTOs;
using Microsoft.AspNetCore.Hosting; // Ajoutez ceci
using System;
using System.Collections.Generic;
using System.IO; // Ajoutez ceci
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Services
{
    public class PdfService
    {
        private readonly RazorViewToStringRenderer _razorRenderer;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _hostingEnvironment; // Ajoutez ce champ

        public PdfService(RazorViewToStringRenderer razorRenderer, IConverter converter, IWebHostEnvironment hostingEnvironment)
        {
            _razorRenderer = razorRenderer;
            _converter = converter;
            _hostingEnvironment = hostingEnvironment; // Initialisez le champ
        }

        public async Task<byte[]> ExportEmployeesToPdf(List<EmployeeDto> employees)
        {
            try
            {
                // Utilisez un chemin relatif
                string templatePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Views", "Employee", "EmployeePDF.cshtml");

                // Journaliser le chemin d'accès de la vue
                Console.WriteLine($"Tentative de rendu de la vue à l'emplacement : {templatePath}");

                // Rendre le contenu HTML à partir de la vue Razor
                var htmlContent = await _razorRenderer.RenderViewToStringAsync(templatePath, employees);

                // Configuration du document PDF
                var doc = new HtmlToPdfDocument
                {
                    GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                    },
                    Objects = {
                        new ObjectSettings
                        {
                            HtmlContent = htmlContent,
                            WebSettings = { DefaultEncoding = "utf-8" }
                        }
                    }
                };

                // Conversion du document en tableau d'octets
                return _converter.Convert(doc);
            }
            catch (Exception ex)
            {
                // Journaliser l'erreur pour un débogage ultérieur
                Console.WriteLine($"Erreur lors de la génération du PDF : {ex}");
                throw new Exception($"Erreur lors de la génération du PDF : {ex.Message}", ex);
            }
        }
    }
}
