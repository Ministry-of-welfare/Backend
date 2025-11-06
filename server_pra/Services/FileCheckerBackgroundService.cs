
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