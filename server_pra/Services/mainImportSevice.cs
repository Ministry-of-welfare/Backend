
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Dal.Models;

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

        public mainImportSevice(
            AppDbContext db,
            LoadBulkTable loadBulk,
            ValidationService validationService,
            ErrorReportService errorReport,
            ILogger<mainImportSevice> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _loadBulk = loadBulk ?? throw new ArgumentNullException(nameof(loadBulk));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _errorReport = errorReport ?? throw new ArgumentNullException(nameof(errorReport));
          //  _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            }
            catch (Exception ex)
            {
              //  _logger.LogError(ex, "שגיאה בתהליך הייבוא עבור ImportControlId={ImportControlId}", importControlId);
                await _errorReport.GenerateAndSendErrorReportAsync(importControlId);
            }
        }
    }
}