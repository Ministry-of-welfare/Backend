using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dal.Api;

namespace server_pra.Services
{
    public class UpdateImportStatusService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<UpdateImportStatusService> _logger;
        private readonly IConfiguration _config;

        public UpdateImportStatusService(
            IServiceScopeFactory scopeFactory,
            ILogger<UpdateImportStatusService> logger,
            IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dalImportControl = scope.ServiceProvider.GetRequiredService<IDalImportControl>();

                    var runs = await dalImportControl.GetAll();
                    var openRuns = runs.Where(r => r.ImportFinishDate == null).ToList();

                    if (!openRuns.Any())
                    {
                        await Task.Delay(TimeSpan.FromSeconds(_config.GetValue("ImportStatusService:DelaySeconds", 60)), stoppingToken);
                        continue;
                    }

                    foreach (var run in openRuns)
                    {
                        try
                        {
                            int totalRows = run.TotalRows ?? 0;
                            int rowsInvalid = await dalImportControl.CountImportProblems(run.ImportControlId);

                            int totalRowsAffected = totalRows - rowsInvalid;
                            string status = rowsInvalid > 0 ? "Failed" : "Success";

                            await dalImportControl.UpdateImportStatusAsync(run.ImportControlId, rowsInvalid, totalRowsAffected, status);
                            _logger.LogInformation("ריצה {ImportControlId} עודכנה לסטטוס {Status}", run.ImportControlId, status);

                            run.ImportFinishDate = DateTime.Now;
                            await dalImportControl.Update(run);
                            _logger.LogInformation("ריצה {ImportControlId} סומנה כסגורה ({ImportFinishDate})", run.ImportControlId, run.ImportFinishDate);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "שגיאה בעיבוד ריצה {ImportControlId}", run.ImportControlId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "שגיאה בלולאה הראשית של UpdateImportStatusService");
                }

                await Task.Delay(TimeSpan.FromSeconds(_config.GetValue("ImportStatusService:DelaySeconds", 60)), stoppingToken);
            }
        }
    }
}