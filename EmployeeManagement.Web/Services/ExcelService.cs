using StockManagement.Application.Features.Employees.DTOs;
using OfficeOpenXml;
using StockManagement.Application.Features.Demandes.DTOs;

namespace StockManagement.Web.Services
{
    public class ExcelService
    {
        public async Task<byte[]> ExportDemandesToExcel(List<GetListDemandesByUserIdResponseDTO> demandes)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Ajout de la licence pour EPPlus
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Demandes");

            // En-têtes de colonnes
            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Nom";
            worksheet.Cells[1, 3].Value = "Prenom";
            worksheet.Cells[1, 4].Value = "DirectionNom";
            worksheet.Cells[1, 5].Value = "DivisionNom";
            worksheet.Cells[1, 6].Value = "ServiceNom";
            worksheet.Cells[1, 7].Value = "StatusDemandeNom";
            worksheet.Cells[1, 8].Value = "LstProduitQuantiteDTOs";

            // Remplissage des données
            for (int i = 0; i < demandes.Count; i++)
            {
                var demande = demandes[i];

                worksheet.Cells[i + 2, 1].Value = demande.Id;
                worksheet.Cells[i + 2, 2].Value = demande.Nom;
                worksheet.Cells[i + 2, 3].Value = demande.Prenom;
                worksheet.Cells[i + 2, 4].Value = demande.DirectionNom;
                worksheet.Cells[i + 2, 5].Value = demande.DivisionNom;
                worksheet.Cells[i + 2, 6].Value = demande.ServiceNom;
                worksheet.Cells[i + 2, 7].Value = demande.StatusDemandeNom;

                // Conversion de LstProduitQuantiteDTOs en une chaîne avec sauts de ligne
                if (demande.LstProduitQuantiteDTOs != null && demande.LstProduitQuantiteDTOs.Any())
                {
                    worksheet.Cells[i + 2, 8].Value = string.Join(",\n", demande.LstProduitQuantiteDTOs
                        .Select(p => $"{p.ProduitNom}: {p.Quantité}"));
                    worksheet.Cells[i + 2, 8].Style.WrapText = true; // Activer le retour à la ligne
                }
                else
                {
                    worksheet.Cells[i + 2, 8].Value = "Aucun produit";
                }
            }

            // Formatage des colonnes pour un meilleur affichage
            worksheet.Cells.AutoFitColumns();

            return await Task.FromResult(package.GetAsByteArray());
        }




        public async Task<byte[]> ExportEmployeesToExcel(List<EmployeeDto> employees)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Ajout de la licence pour EPPlus
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Employees");

            // En-têtes de colonnes
            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "First Name";
            worksheet.Cells[1, 3].Value = "Last Name";
            worksheet.Cells[1, 4].Value = "Position";
            worksheet.Cells[1, 5].Value = "Salary";

            // Remplissage des données
            for (int i = 0; i < employees.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = employees[i].Id;
                worksheet.Cells[i + 2, 2].Value = employees[i].FirstName;
                worksheet.Cells[i + 2, 3].Value = employees[i].LastName;
                worksheet.Cells[i + 2, 4].Value = employees[i].Position;
                worksheet.Cells[i + 2, 5].Value = employees[i].Salary;
            }

            return await Task.FromResult(package.GetAsByteArray());
        }


        public async Task<List<EmployeeDto>> ImportEmployeesFromExcel(Stream fileStream)
        {
            var employees = new List<EmployeeDto>();

            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets[0]; // Première feuille

            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++) // Commencer à la ligne 2 pour ignorer les en-têtes
            {
                var employee = new EmployeeDto
                {
                    Id = new Guid(), // ID auto-incrémenté, pas besoin de le définir ici
                    FirstName = worksheet.Cells[row, 2].Value?.ToString(),
                    LastName = worksheet.Cells[row, 3].Value?.ToString(),
                    Position = worksheet.Cells[row, 4].Value?.ToString(),
                    Salary = decimal.TryParse(worksheet.Cells[row, 5].Value?.ToString(), out var salary) ? salary : 0
                };

                employees.Add(employee);
            }

            return await Task.FromResult(employees);
        }
    }
}
