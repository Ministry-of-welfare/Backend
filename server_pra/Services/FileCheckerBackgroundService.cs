using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Dal.Models;

namespace server_pra.Services
{
    public class FileCheckerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<FileCheckerBackgroundService> _logger;
        private readonly TimeSpan _delay;
        private readonly IHostEnvironment _env;

        public FileCheckerBackgroundService(IServiceProvider services, ILogger<FileCheckerBackgroundService> logger, IConfiguration configuration, IHostEnvironment env)
        {
            _services = services;
            _logger = logger;
            _env = env;
            var seconds = configuration.GetValue<int?>("FileChecker:IntervalSeconds") ?? 10;
            _delay = TimeSpan.FromSeconds(Math.Max(1, seconds));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FileCheckerBackgroundService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<Dal.Models.AppDbContext>();

                    // Pull only active sources: FileStatusId == 1 AND (EndDate IS NULL OR EndDate > now)
                    var sources = await db.TabImportDataSources
                        .Where(x =>
                            x.FileStatusId == 1 &&
                            (x.EndDate == null || x.EndDate > DateTime.UtcNow) &&
                            (!string.IsNullOrEmpty(x.UrlFile) || !string.IsNullOrEmpty(x.UrlFileAfterProcess))
                        )
                        .ToListAsync(stoppingToken);

                    foreach (var s in sources)
                    {
                        var raw = (!string.IsNullOrWhiteSpace(s.UrlFile) ? s.UrlFile : s.UrlFileAfterProcess) ?? string.Empty;
                        var path = raw.Trim();

                        // If path is relative, resolve against content root
                        if (!Path.IsPathRooted(path) && !path.StartsWith(@"\\"))
                        {
                            path = Path.Combine(_env.ContentRootPath ?? Directory.GetCurrentDirectory(), path);
                        }

                        // quick validation: minimum chars + rooted or UNC
                        if (path.Length < 3)
                        {
                            _logger.LogWarning("Malformed path for ImportDataSourceId {Id}: {Raw}", s.ImportDataSourceId, raw);
                            continue;
                        }

                        try
                        {
                            // If path is a directory: enumerate files
                            if (Directory.Exists(path))
                            {
                                var files = Directory.GetFiles(path);
                                foreach (var filePath in files)
                                {
                                    await EnsureImportControlForFileAsync(db, s, filePath, stoppingToken);
                                }
                            }
                            // If path is a file: process single file
                            else if (File.Exists(path))
                            {
                                await EnsureImportControlForFileAsync(db, s, path, stoppingToken);
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

                    // persist created runs in a single SaveChanges call
                    await db.SaveChangesAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "FileCheckerBackgroundService error");
                }

                try
                {
                    await Task.Delay(_delay, stoppingToken);
                }
                catch (TaskCanceledException) { }
            }

            _logger.LogInformation("FileCheckerBackgroundService stopped.");
        }

        // Create AppImportControl only if not already present for this ImportDataSourceId + FileName
        private async Task EnsureImportControlForFileAsync(Dal.Models.AppDbContext db, TabImportDataSource source, string filePath, CancellationToken ct)
        {
            var fileName = Path.GetFileName(filePath) ?? filePath;

            // dedupe check: any existing record for same source+filename
            var exists = await db.AppImportControls
                .AnyAsync(ac => ac.ImportDataSourceId == source.ImportDataSourceId && ac.FileName == fileName, ct);

            if (exists)
            {
                _logger.LogDebug("File already handled (ImportDataSourceId={Id}): {File}", source.ImportDataSourceId, fileName);
                return;
            }

            // determine ImportFromDate candidate (use file last write date local or UTC as business requires)
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

            db.AppImportControls.Add(newRun);
            _logger.LogInformation("Created AppImportControl for ImportDataSourceId {Id}: {File}", source.ImportDataSourceId, fileName);
        }
    }
}