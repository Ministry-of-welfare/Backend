using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ClosedXML.Excel; // For Excel generation
using Microsoft.Extensions.Logging;
using Dal.Api; // Data Access Layer
using server_pra.Models; // Import for AppImportProblem
using System.Net.Mail; // For email sending
using System.Text.Json; // For reading email settings

namespace server_pra.Services
{
    public class ErrorReportService
    {
        private readonly IDalImportControl _dalImportControl;
        private readonly IDalImportProblem _dalImportProblem;
        private readonly ILogger<ErrorReportService> _logger;
        private readonly string _emailSettingsPath = "Config/EmailSettings.json";

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

                // Fetch column descriptions
                var columnDescriptions = await _dalImportControl.GetColumnDescriptionsAsync(importControl.ImportDataSourceId);

                // Generate Excel file
                string filePath = GenerateExcelReport(importControl.FileName, importDataSource.UrlFileAfterProcess, mappedErrors, columnDescriptions);

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

        private string GenerateExcelReport(string fileName, string outputDirectory, IEnumerable<AppImportProblem> errors, Dictionary<string, string> columnDescriptions)
        {
            Directory.CreateDirectory(outputDirectory);

            string filePath = Path.Combine(outputDirectory, $"Errors_{fileName}_{DateTime.Now:yyyyMMddHHmmss}.xlsx");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Errors");

                // Add headers
                worksheet.Cell(1, 1).Value = "שם קובץ";
                worksheet.Cell(1, 2).Value = "מספר שורה";
                worksheet.Cell(1, 3).Value = "עמודת שגיאה";
                worksheet.Cell(1, 4).Value = "הערך השגוי";
                worksheet.Cell(1, 5).Value = "תיאור עמודה";

                // Add data
                int row = 2;
                foreach (var error in errors)
                {
                    worksheet.Cell(row, 1).Value = fileName;
                    worksheet.Cell(row, 2).Value = error.ErrorRow;
                    worksheet.Cell(row, 3).Value = columnDescriptions.ContainsKey(error.ErrorColumn) ? columnDescriptions[error.ErrorColumn] : "תיאור לא נמצא";
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
            try
            {
                _logger.LogInformation("Preparing to send email to recipients: {Recipients}", recipients);

                // Load email settings
                var emailSettings = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(_emailSettingsPath));

                _logger.LogInformation("Loaded email settings: {EmailSettings}", emailSettings);
                _logger.LogInformation("SMTP Server: {SmtpServer}, Port: {Port}, SenderEmail: {SenderEmail}", emailSettings["SmtpServer"], emailSettings["Port"], emailSettings["SenderEmail"]);
                _logger.LogInformation("Recipients: {Recipients}", recipients);
                _logger.LogInformation("FilePath: {FilePath}", filePath);

                var smtpClient = new SmtpClient(emailSettings["SmtpServer"])
                {
                    Port = int.Parse(emailSettings["Port"]),
                    Credentials = new System.Net.NetworkCredential(emailSettings["SenderEmail"], emailSettings["SenderPassword"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSettings["SenderEmail"]),
                    Subject = "Error Report",
                    Body = "Please find the attached error report.",
                    IsBodyHtml = true,
                };

                foreach (var recipient in recipients.Split(','))
                {
                    var trimmedRecipient = recipient.Trim();
                    _logger.LogInformation("Adding recipient: {Recipient}", trimmedRecipient);
                    mailMessage.To.Add(trimmedRecipient);
                }

                mailMessage.Attachments.Add(new Attachment(filePath));

                _logger.LogInformation("Attempting to send email with attachment: {FilePath}", filePath);

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation("Email sent successfully to {Recipients} with attachment {FilePath}.", recipients, filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipients} with attachment {FilePath}.", recipients, filePath);
            }
        }
    }
}