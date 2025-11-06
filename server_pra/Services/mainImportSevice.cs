using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Dal.Models;

namespace server_pra.Services
{
    // שירות מרכזי שמזמנת את תהליך ה־import בעזרת שירותים קיימים (LoadBulkTable, ErrorReportService).
    // שים לב: הרשמה ב‑DI: services.AddScoped<mainImportSevice>();
    public class mainImportSevice
    {
        private readonly AppDbContext _db;
        private readonly LoadBulkTable _loadBulk;
        private readonly ErrorReportService _errorReport;
        private readonly ILogger<mainImportSevice> _logger;

        public mainImportSevice(AppDbContext db, LoadBulkTable loadBulk, ErrorReportService errorReport, ILogger<mainImportSevice> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _loadBulk = loadBulk ?? throw new ArgumentNullException(nameof(loadBulk));
            _errorReport = errorReport ?? throw new ArgumentNullException(nameof(errorReport));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // הפונקציה הראשית שמריצה את תהליך הייבוא עבור importControlId.
        // שלבי הפעולה:
        // 1. שליפה של AppImportControl ו‑TabImportDataSource
        // 2. עדכון סטטוס ל"בתהליך"
        // 3. קריאה ל־LoadBulkTable.LoadBulkData
        // 4. עדכון סטטוס לסיום או טיפול בשגיאות + הפעלת ErrorReportService במקרה של שגיאה
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
                // שלב 2: עדכון סטטוס ל"בתהליך" (ImportStatusId = 2) — אם המודל שלך משתמש בערכים שונים, שנה בהתאם
                importControl.ImportStatusId = 2;
                importControl.ImportStartDate = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);
                _logger.LogInformation("עדכון סטטוס ל־Processing עבור ImportControlId={ImportControlId}", importControlId);

                // שלב 3: קריאה לשירות הטעינה (LoadBulkTable)
                await _loadBulk.LoadBulkData(importDataSourceId, importControlId);

                // שלב 4: עדכון סטטוס לסיום מוצלח (לדוגמה ImportStatusId = 3)
                importControl.ImportStatusId = 3;
                importControl.ImportFinishDate = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);

                _logger.LogInformation("תהליך הייבוא הושלם בהצלחה עבור ImportControlId={ImportControlId}", importControlId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה בתהליך הייבוא עבור ImportControlId={ImportControlId}", importControlId);

                // עדכון סטטוס לשגיאה (בחר את הערך המתאים במערכת שלך)
                try
                {
                    importControl.ImportStatusId = 5; // לדוגמה: סטטוס שגיאה
                    importControl.ImportFinishDate = DateTime.UtcNow;
                    //importControl. = ex.Message.Length <= 4000 ? ex.Message : ex.Message.Substring(0, 4000); // אם יש שדה כזה
                    await _db.SaveChangesAsync(ct);
                }
                catch (Exception saveEx)
                {
                    _logger.LogError(saveEx, "שגיאה בעדכון סטטוס שגיאה ל־ImportControlId={ImportControlId}", importControlId);
                }

                // שליחת דוח שגיאות במידת הצורך
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