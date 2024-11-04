using DinkToPdf;
using DinkToPdf.Contracts;
using EmployeeManagement.Application.Features.Employees.DTOs;
using System.Collections.Generic;
using System.IO;

namespace EmployeeManagement.Web.Services
{
    public class PdfService
    {
        public byte[] ExportEmployeesToPdf(List<EmployeeDto> employees)
        {
            // Générer le HTML pour la table des employés
            var htmlContent = "<html><head><style>" +
                              "table { width: 100%; border-collapse: collapse; }" +
                              "th, td { border: 1px solid black; padding: 8px; text-align: left; }" +
                              "th { background-color: #f2f2f2; }" +
                              "</style></head><body>" +
                              "<h1>Liste des Employés</h1>" +
                              "<table>" +
                              "<tr><th>ID</th><th>Prénom</th><th>Nom</th><th>Poste</th><th>Salaire</th></tr>";

            foreach (var employee in employees)
            {
                htmlContent += $"<tr><td>{employee.Id}</td><td>{employee.FirstName}</td><td>{employee.LastName}</td><td>{employee.Position}</td><td>{employee.Salary}</td></tr>";
            }

            htmlContent += "</table></body></html>";

            // Utiliser DinkToPdf pour générer le PDF à partir du HTML
            var converter = new SynchronizedConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings()
                    {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            using var memoryStream = new MemoryStream();
            var pdf = converter.Convert(doc);
            memoryStream.Write(pdf, 0, pdf.Length);

            return memoryStream.ToArray();
        }
    }
}
