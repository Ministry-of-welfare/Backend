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
using Dal.Models; // use DAL AppDbContext explicitly

namespace server_pra.Services
{
    public class FileCheckerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<FileCheckerBackgroundService> _logger;
        private readonly TimeSpan _delay;

        public FileCheckerBackgroundService(IServiceProvider services, ILogger<FileCheckerBackgroundService> logger, IConfiguration configuration)
        {
            _services = services;
            _logger = logger;
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

                    var sources = await db.TabImportDataSources
                        .Where(x => !string.IsNullOrEmpty(x.UrlFile) || !string.IsNullOrEmpty(x.UrlFileAfterProcess))
                        .ToListAsync(stoppingToken);

                    foreach (var s in sources)
                    {
                        var raw = (!string.IsNullOrWhiteSpace(s.UrlFileAfterProcess) ? s.UrlFileAfterProcess : s.UrlFile) ?? string.Empty;
                        var path = raw.Trim();

                        // וולידציה מהירה: מינימום תווים + rooted path (או UNC)
                        if (path.Length < 3 || !(Path.IsPathRooted(path) || path.StartsWith(@"\\")))
                        {
                            _logger.LogWarning("Malformed path for ImportDataSourceId {Id}: {Raw}", s.ImportDataSourceId, raw);
                            continue;
                        }

                        try
                        {
                            if (File.Exists(path))
                                _logger.LogInformation("File exists for ImportDataSourceId {Id}: {Path}", s.ImportDataSourceId, path);
                            else if (Directory.Exists(path))
                                _logger.LogInformation("Directory exists for ImportDataSourceId {Id}: {Path}", s.ImportDataSourceId, path);
                            else
                                _logger.LogWarning("Not found for ImportDataSourceId {Id}: {Path}", s.ImportDataSourceId, path);
                        }
                        catch (Exception exPath)
                        {
                            _logger.LogError(exPath, "Error checking path for ImportDataSourceId {Id}: {Path}", s.ImportDataSourceId, path);
                        }
                    }
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
    }
}