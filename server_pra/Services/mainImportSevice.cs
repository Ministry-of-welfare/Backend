
using Dal.Api;
using Dal.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace server_pra.Services
{
    // שירות מרכזי שמזמנת את תהליך ה־import בעזרת שירותים קיימים (LoadBulkTable, ValidationService, ErrorReportService).
    // יש לרשום ב‑DI: services.AddScoped<mainImportSevice>();
    public class mainImportSevice
    {
        private readonly AppDbContext _db;
        private readonly LoadBulkTable _loadBulk;
        private readonly ValidationService _validationService;
        private readonly ErrorReportService _errorReport;
        private readonly ILogger<mainImportSevice> _logger;
        private readonly IDalImportControl _dalImportControl;
        public mainImportSevice(
       AppDbContext db,
       LoadBulkTable loadBulk,
       ValidationService validationService,
       ErrorReportService errorReport,
       IDalImportControl dalImportControl,
       ILogger<mainImportSevice> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _loadBulk = loadBulk ?? throw new ArgumentNullException(nameof(loadBulk));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _errorReport = errorReport ?? throw new ArgumentNullException(nameof(errorReport));
            _dalImportControl = dalImportControl ?? throw new ArgumentNullException(nameof(dalImportControl));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task RunAsync(int importControlId, CancellationToken ct = default)
        {
            var importControl = await _db.AppImportControls.FindAsync(new object[] { importControlId }, ct);
            if (importControl == null) return;

            var importDataSourceId = importControl.ImportDataSourceId;

            try
            {
                await _loadBulk.LoadBulkData(importDataSourceId, importControlId);

                bool hasErrors = await _validationService.ValidateAsync(importControlId);

                if (hasErrors)
                {
                    await _errorReport.GenerateAndSendErrorReportAsync(importControlId);
                }

                // עדכון סטטוס ריצה
                int totalRows = importControl.TotalRows ?? 0;
                int rowsInvalid = await _dalImportControl.CountImportProblems(importControlId);
                int totalRowsAffected = totalRows - rowsInvalid;
                string status = rowsInvalid > 0 ? "Failed" : "Success";

                await _dalImportControl.UpdateImportStatusAsync(importControlId, rowsInvalid, totalRowsAffected, status);
                _logger.LogInformation("עודכן סטטוס ייבוא ל-{Status} עבור ImportControlId={ImportControlId}", status, importControlId);

                // סימון כסיום ריצה
                importControl.ImportFinishDate = DateTime.Now;
                await _dalImportControl.Update(importControl);
                _logger.LogInformation("ריצה {ImportControlId} סומנה כסגורה ({ImportFinishDate})", importControlId, importControl.ImportFinishDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה בתהליך הייבוא עבור ImportControlId={ImportControlId}", importControlId);
                await _errorReport.GenerateAndSendErrorReportAsync(importControlId);
            }
        }
    }
}