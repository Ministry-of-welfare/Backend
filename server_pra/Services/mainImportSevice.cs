
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
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // הפונקציה הראשית שמריצה את תהליך הייבוא עבור importControlId.
        public async Task RunAsync(int importControlId, CancellationToken ct = default)
        {
            _logger.LogInformation("מתחיל תהליך ייבוא עבור ImportControlId={ImportControlId}", importControlId);

            // שלב 1: שליפת AppImportControl
            var importControl = await _db.AppImportControls.FindAsync(new object[] { importControlId }, ct);
            if (importControl == null)
            {
                _logger.LogWarning("לא נמצא AppImportControl עבור ImportControlId={ImportControlId}", importControlId);
                return;
            }

            var importDataSourceId = importControl.ImportDataSourceId;
            var dataSource = await _db.TabImportDataSources.FindAsync(new object[] { importDataSourceId }, ct);
            if (dataSource == null)
            {
                _logger.LogWarning("לא נמצא TabImportDataSource עבור ImportDataSourceId={ImportDataSourceId}", importDataSourceId);
                return;
            }

            // החלטה על נתיב הקובץ: עדיפות ל־UrlFileAfterProcess שב‑ImportControl, אחרת UrlFile מה‑DataSource
            var filePath = !string.IsNullOrWhiteSpace(importControl.UrlFileAfterProcess)
                ? importControl.UrlFileAfterProcess
                : dataSource.UrlFile;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                _logger.LogWarning("נתיב הקובץ ריק עבור ImportControlId={ImportControlId}", importControlId);
                return;
            }

            try
            {
                // שלב 2: עדכון סטטוס ל"בתהליך" (ImportStatusId = 2)
                importControl.ImportStatusId = 2;
                importControl.ImportStartDate = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);
                _logger.LogInformation("עדכון סטטוס ל־Processing עבור ImportControlId={ImportControlId}", importControlId);

                // שלב 3: קריאה לשירות הטעינה (LoadBulkTable)
                await _loadBulk.LoadBulkData(importDataSourceId, importControlId);

                // שלב 4: הרצת וולידציות
                bool hasValidationErrors = false;
                try
                {
                    hasValidationErrors = await _validationService.ValidateAsync(importControlId);
                    _logger.LogInformation("וולידציה הושלמה עבור ImportControlId={ImportControlId} — HasErrors={HasErrors}", importControlId, hasValidationErrors);
                }
                catch (Exception valEx)
                {
                    _logger.LogError(valEx, "שגיאה בהרצת וולידציות עבור ImportControlId={ImportControlId}", importControlId);
                    throw;
                }

                if (hasValidationErrors)
                {
                    // עדכון תאריך סיום ושיגור דוח שגיאות
                    importControl.ImportFinishDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync(ct);

                    try
                    {
                        await _errorReport.GenerateAndSendErrorReportAsync(importControlId);
                    }
                    catch (Exception emailEx)
                    {
                        _logger.LogError(emailEx, "שגיאה בשליחת דוח שגיאות לאחר וולידציה עבור ImportControlId={ImportControlId}", importControlId);
                    }

                    _logger.LogInformation("וולידציה זיהתה שגיאות; סיום תהליך ייבוא עבור ImportControlId={ImportControlId} עם שגיאות", importControlId);
                    return;
                }

                // שלב 5: עדכון סטטוס לסיום מוצלח (ImportStatusId = 3)
                importControl.ImportStatusId = 3;
                importControl.ImportFinishDate = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);

                _logger.LogInformation("תהליך הייבוא הושלם בהצלחה עבור ImportControlId={ImportControlId}", importControlId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה בתהליך הייבוא עבור ImportControlId={ImportControlId}", importControlId);

                try
                {
                    importControl.ImportStatusId = 5; // סטטוס שגיאה — שנה לפי הצורך
                    importControl.ImportFinishDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync(ct);
                }
                catch (Exception saveEx)
                {
                    _logger.LogError(saveEx, "שגיאה בעדכון סטטוס שגיאה ל־ImportControlId={ImportControlId}", importControlId);
                }

                try
                {
                    await _errorReport.GenerateAndSendErrorReportAsync(importControlId);
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "שגיאה בשליחת דוח שגיאות עבור ImportControlId={ImportControlId}", importControlId);
                }
            }
        }
    }
}