//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Dal.Models;

//namespace server_pra.Services
//{
//    public class FileCheckerBackgroundService : BackgroundService
//    {
//        private readonly IServiceProvider _services;
//        private readonly ILogger<FileCheckerBackgroundService> _logger;
//        private readonly TimeSpan _delay;
//        private readonly IHostEnvironment _env;

//        public FileCheckerBackgroundService(IServiceProvider services, ILogger<FileCheckerBackgroundService> logger, IConfiguration configuration, IHostEnvironment env)
//        {
//            _services = services;
//            _logger = logger;
//            _env = env;
//            var seconds = configuration.GetValue<int?>("FileChecker:IntervalSeconds") ?? 10;
//            _delay = TimeSpan.FromSeconds(Math.Max(1, seconds));
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            _logger.LogInformation("FileCheckerBackgroundService started.");

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                try
//                {
//                    using var scope = _services.CreateScope();
//                    var db = scope.ServiceProvider.GetRequiredService<Dal.Models.AppDbContext>();

//                    // משוך מקורות פעילים בלבד: FileStatusId == 1 && (EndDate == null || EndDate > now)
//                    // וכן לקוחות עם נתיב קובץ או נתיב לאחר עיבוד
//                    var sources = await db.TabImportDataSources
//                        .Where(x =>
//                            x.FileStatusId == 1 &&
//                            (x.EndDate == null || x.EndDate > DateTime.UtcNow) &&
//                            (!string.IsNullOrEmpty(x.UrlFile) || !string.IsNullOrEmpty(x.UrlFileAfterProcess))
//                        )
//                        .ToListAsync(stoppingToken);

//                    foreach (var s in sources)
//                    {
//                        var raw = (!string.IsNullOrWhiteSpace(s.UrlFile) ? s.UrlFile : s.UrlFileAfterProcess) ?? string.Empty;
//                        var path = raw.Trim();

//                        // אם הנתיב יחסית — פתח ביחס ל־ContentRoot (או לתיקיית עבודה נוכחית)
//                        if (!Path.IsPathRooted(path) && !path.StartsWith(@"\\"))
//                        {
//                            path = Path.Combine(_env.ContentRootPath ?? Directory.GetCurrentDirectory(), path);
//                        }

//                        // בדיקה מהירה: אורך מינימלי + נתיב מוחלט או UNC
//                        if (path.Length < 3)
//                        {
//                            _logger.LogWarning("Malformed path for ImportDataSourceId {Id}: {Raw}", s.ImportDataSourceId, raw);
//                            continue;
//                        }

//                        try
//                        {
//                            // אם הנתיב הוא תיקייה — עבור על כל הקבצים שבה
//                            if (Directory.Exists(path))
//                            {
//                                var files = Directory.GetFiles(path);
//                                foreach (var filePath in files)
//                                {
//                                    var result = await EnsureImportControlForFileAsync(db, s, filePath, stoppingToken);
//                                    if (result.importControlId.HasValue)
//                                    {
//                                        _logger.LogInformation("Created AppImportControl ImportDataSourceId={Id} ImportControlId={ControlId}: {File}", result.importDataSourceId, result.importControlId.Value, filePath);
//                                    }
//                                }
//                            }
//                            // אם הנתיב הוא קובץ — עבד קובץ יחיד
//                            else if (File.Exists(path))
//                            {
//                                var result = await EnsureImportControlForFileAsync(db, s, path, stoppingToken);
//                                if (result.importControlId.HasValue)
//                                {
//                                    _logger.LogInformation("Created AppImportControl ImportDataSourceId={Id} ImportControlId={ControlId}: {File}", result.importDataSourceId, result.importControlId.Value, path);
//                                }
//                            }
//                            else
//                            {
//                                _logger.LogWarning("Not found for ImportDataSourceId {Id}: {Path}", s.ImportDataSourceId, path);
//                            }
//                        }
//                        catch (Exception exPath)
//                        {
//                            _logger.LogError(exPath, "Error checking path for ImportDataSourceId {Id}: {Path}", s.ImportDataSourceId, path);
//                        }
//                    }

//                    // שמור את השינויים שנותרו (אם יש) — חלק מהשורות כבר נשמרו ב־EnsureImportControlForFileAsync
//                    await db.SaveChangesAsync(stoppingToken);
//                }
//                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
//                {
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "FileCheckerBackgroundService error");
//                }

//                try
//                {
//                    await Task.Delay(_delay, stoppingToken);
//                }
//                catch (TaskCanceledException) { }
//            }

//            _logger.LogInformation("FileCheckerBackgroundService stopped.");
//        }

//        /// <summary>
//        /// צור AppImportControl רק אם לא קיים עבור אותו ImportDataSourceId + FileName.
//        /// מחזיר את ה־ImportDataSourceId ואת ה־ImportControlId (מאוחסן) אם נוצר; אחרת ImportControlId = null.
//        /// </summary>
//        private async Task<(int importDataSourceId, int? importControlId)> EnsureImportControlForFileAsync(Dal.Models.AppDbContext db, TabImportDataSource source, string filePath, CancellationToken ct)
//        {
//            var fileName = Path.GetFileName(filePath) ?? filePath;

//            // בדיקת כפילויות: האם יש כבר רשומה עם אותו מקור + שם קובץ
//            var exists = await db.AppImportControls
//                .AnyAsync(ac => ac.ImportDataSourceId == source.ImportDataSourceId && ac.FileName == fileName, ct);

//            if (exists)
//            {
//                _logger.LogDebug("File already handled (ImportDataSourceId={Id}): {File}", source.ImportDataSourceId, fileName);
//                return (source.ImportDataSourceId, null);
//            }

//            // קבע תאריך מקור לקליטה (מהתאריך שינוי אחרון של הקובץ ב־UTC)
//            DateTime importFrom = File.GetLastWriteTimeUtc(filePath);

//            var newRun = new AppImportControl
//            {
//                ImportDataSourceId = source.ImportDataSourceId,
//                ImportStatusId = 1,
//                ImportStartDate = DateTime.UtcNow,
//                ImportFromDate = importFrom,
//                FileName = fileName,
//                UrlFileAfterProcess = filePath,
//                EmailSento = source.ErrorRecipients
//            };

//            db.AppImportControls.Add(newRun);

//            // שמור מיד כדי לקבל את ה־ImportControlId שנוצר (נדרש כדי להחזירו מהמתודה)
//            await db.SaveChangesAsync(ct);

//            _logger.LogInformation("Created AppImportControl for ImportDataSourceId {Id}: {File} (ImportControlId={ControlId})", source.ImportDataSourceId, fileName, newRun.ImportControlId);

//            return (source.ImportDataSourceId, newRun.ImportControlId);
//        }
//    }
//}
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace server_pra.Services
{
    // שירות קונקרטי שמריץ סריקה אחת ומחזיר רשימת זוגות (ImportDataSourceId, ImportControlId?)
    public class FileCheckerService
    {
        private readonly AppDbContext _db;
        private readonly IHostEnvironment _env;
        private readonly ILogger<FileCheckerService> _logger;

        public FileCheckerService(AppDbContext db, IHostEnvironment env, ILogger<FileCheckerService> logger)
        {
            _db = db;
            _env = env;
            _logger = logger;
        }

        public async Task<List<(int ImportDataSourceId, int? ImportControlId)>> RunOnceAsync(CancellationToken ct = default)
        {
            var created = new List<(int, int?)>();

            var sources = await _db.TabImportDataSources
                .Where(x =>
                    x.FileStatusId == 1 &&
                    (x.EndDate == null || x.EndDate > DateTime.UtcNow) &&
                    (!string.IsNullOrEmpty(x.UrlFile) || !string.IsNullOrEmpty(x.UrlFileAfterProcess))
                )
                .ToListAsync(ct);

            foreach (var s in sources)
            {
                var raw = (!string.IsNullOrWhiteSpace(s.UrlFile) ? s.UrlFile : s.UrlFileAfterProcess) ?? string.Empty;
                var path = raw.Trim();

                if (!Path.IsPathRooted(path) && !path.StartsWith(@"\\"))
                    path = Path.Combine(_env.ContentRootPath ?? Directory.GetCurrentDirectory(), path);

                if (path.Length < 3)
                {
                    _logger.LogWarning("Malformed path for ImportDataSourceId {Id}: {Raw}", s.ImportDataSourceId, raw);
                    continue;
                }

                try
                {
                    if (Directory.Exists(path))
                    {
                        var files = Directory.GetFiles(path);
                        foreach (var filePath in files)
                        {
                            var result = await EnsureImportControlForFileAsync(s, filePath, ct);
                            if (result.importControlId.HasValue) created.Add(result);
                        }
                    }
                    else if (File.Exists(path))
                    {
                        var result = await EnsureImportControlForFileAsync(s, path, ct);
                        if (result.importControlId.HasValue) created.Add(result);
                    }
                    else
                    {
                        _logger.LogWarning("Not found for ImportDataSourceId {Id}: {Path}", s.ImportDataSourceId, path);
                    }
                }
                catch (Exception exPath)
                {
                    _logger.LogError(exPath, "Error checking path for ImportDataSourceId {Id}: {Path}", s.ImportDataSourceId, path);
                }
            }

            return created;
        }

        private async Task<(int importDataSourceId, int? importControlId)> EnsureImportControlForFileAsync(TabImportDataSource source, string filePath, CancellationToken ct)
        {
            var fileName = Path.GetFileName(filePath) ?? filePath;

            var exists = await _db.AppImportControls
                .AnyAsync(ac => ac.ImportDataSourceId == source.ImportDataSourceId && ac.FileName == fileName, ct);

            if (exists)
            {
                _logger.LogDebug("File already handled (ImportDataSourceId={Id}): {File}", source.ImportDataSourceId, fileName);
                return (source.ImportDataSourceId, null);
            }

            DateTime importFrom = File.GetLastWriteTimeUtc(filePath);

            var newRun = new AppImportControl
            {
                ImportDataSourceId = source.ImportDataSourceId,
                ImportStatusId = 1,
                ImportStartDate = DateTime.UtcNow,
                ImportFromDate = importFrom,
                FileName = fileName,
                UrlFileAfterProcess = filePath,
                EmailSento = source.ErrorRecipients
            };

            _db.AppImportControls.Add(newRun);
            await _db.SaveChangesAsync(ct); // שמירה כדי לקבל ImportControlId

            _logger.LogInformation("Created AppImportControl for ImportDataSourceId {Id}: {File} (ImportControlId={ControlId})",
                source.ImportDataSourceId, fileName, newRun.ImportControlId);

            return (source.ImportDataSourceId, newRun.ImportControlId);
        }
    }
}