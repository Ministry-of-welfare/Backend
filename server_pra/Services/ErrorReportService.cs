using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ClosedXML.Excel; // For Excel generation
using Microsoft.Extensions.Logging;
using Dal.Api; // Data Access Layer
using server_pra.Models; // Import for AppImportProblem

namespace server_pra.Services
{
    public class ErrorReportService
    {
        private readonly IDalImportControl _dalImportControl;
        private readonly IDalImportProblem _dalImportProblem;
        private readonly ILogger<ErrorReportService> _logger;

        public ErrorReportService(IDalImportControl dalImportControl, IDalImportProblem dalImportProblem, ILogger<ErrorReportService> logger)
        {
            _dalImportControl = dalImportControl;
            _dalImportProblem = dalImportProblem;
            _logger = logger;
        }

        public async Task GenerateAndSendErrorReportAsync(int importControlId)
        {
            try
            {
                // Fetch import control details
                var importControl = await _dalImportControl.GetByIdAsync(importControlId);
                if (importControl == null)
                {
                    _logger.LogWarning("ImportControlId {ImportControlId} not found.", importControlId);
                    return;
                }

                // Fetch error details
                var errors = await _dalImportProblem.GetErrorsByImportControlIdAsync(importControlId);
                if (!errors.Any())
                {
                    _logger.LogInformation("No errors found for ImportControlId {ImportControlId}.", importControlId);
                    return;
                }

                // Map Dal.Models.AppImportProblem to server_pra.Models.AppImportProblem
                var mappedErrors = errors.Select(e => new AppImportProblem
                {
                    ImportProblemId = e.ImportProblemId,
                    ImportControlId = e.ImportControlId,
                    ErrorColumn = e.ErrorColumn,
                    ErrorValue = e.ErrorValue,
                    ErrorRow = e.ErrorRow,
                    ErrorTableId = e.ErrorTableId,
                    ImportErrorId = e.ImportErrorId,
                    ErrorDetail = e.ErrorDetail
                }).ToList();

                // Fetch output directory from UrlFileAfterProcess
                var importDataSource = await _dalImportControl.GetImportDataSourceByIdAsync(importControl.ImportDataSourceId);
                if (importDataSource == null || string.IsNullOrEmpty(importDataSource.UrlFileAfterProcess))
                {
                    _logger.LogWarning("No UrlFileAfterProcess found for ImportControlId {ImportControlId}.", importControlId);
                    return;
                }

                // Generate Excel file
                string filePath = GenerateExcelReport(importControl.FileName, importDataSource.UrlFileAfterProcess, mappedErrors);

                // Update ErrorReportPath in ImportControl
                await _dalImportControl.UpdateErrorReportPathAsync(importControlId, filePath);

                // Send email with the report
                if (string.IsNullOrEmpty(importDataSource.ErrorRecipients))
                {
                    _logger.LogWarning("No ErrorRecipients found for ImportControlId {ImportControlId}.", importControlId);
                    return;
                }

                await SendErrorReportEmailAsync(importDataSource.ErrorRecipients, filePath);

                _logger.LogInformation("Error report generated and sent for ImportControlId {ImportControlId}.", importControlId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate and send error report for ImportControlId {ImportControlId}.", importControlId);
            }
        }

        private string GenerateExcelReport(string fileName, string outputDirectory, IEnumerable<AppImportProblem> errors)
        {
            Directory.CreateDirectory(outputDirectory);

            string filePath = Path.Combine(outputDirectory, $"Errors_{fileName}_{DateTime.Now:yyyyMMddHHmmss}.xlsx");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Errors");

                // Add headers
                worksheet.Cell(1, 1).Value = "File Name";
                worksheet.Cell(1, 2).Value = "Error Row";
                worksheet.Cell(1, 3).Value = "Error Column";
                worksheet.Cell(1, 4).Value = "Error Value";
                worksheet.Cell(1, 5).Value = "Error Detail";

                // Add data
                int row = 2;
                foreach (var error in errors)
                {
                    worksheet.Cell(row, 1).Value = fileName;
                    worksheet.Cell(row, 2).Value = error.ErrorRow;
                    worksheet.Cell(row, 3).Value = error.ErrorColumn;
                    worksheet.Cell(row, 4).Value = error.ErrorValue;
                    worksheet.Cell(row, 5).Value = error.ErrorDetail;
                    row++;
                }

                // Save the file
                workbook.SaveAs(filePath);
            }

            return filePath;
        }

        private async Task SendErrorReportEmailAsync(string recipients, string filePath)
        {
            // Implement email sending logic here
            // Use System.Net.Mail or MailKit to send the email
            _logger.LogInformation("Sending error report email to {Recipients} with attachment {FilePath}.", recipients, filePath);
        }
    }
}