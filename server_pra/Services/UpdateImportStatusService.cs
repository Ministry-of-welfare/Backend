using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dal.Api;

namespace server_pra.Services
{
    public class UpdateImportStatusService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateImportStatusService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var dalImportControl = scope.ServiceProvider.GetRequiredService<IDalImportControl>();

                var runs = await dalImportControl.GetAll();

                foreach (var run in runs)
                {
                    if (run.ImportFinishDate == null)
                    {
                        int totalRows = run.TotalRows ?? 0;
                        int rowsInvalid = await dalImportControl.CountImportProblems(run.ImportControlId);

                        int totalRowsAffected = totalRows - rowsInvalid;
                        string status = rowsInvalid > 0 ? "Failed" : "Success";

                        await dalImportControl.UpdateImportStatusAsync(run.ImportControlId, rowsInvalid, totalRowsAffected, status);

                        Console.WriteLine($"[LOG] ריצה {run.ImportControlId} עודכנה לסטטוס {status}");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
