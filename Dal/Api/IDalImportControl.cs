using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Api
{
    public interface IDalImportControl:ICrud<AppImportControl>
    {
        Task<Dal.Models.AppImportControl> GetByIdAsync(int id);
        Task UpdateImportStatusAsync(int importId, int rowsInvalid, int totalRowsAffected, string status);
        Task<int> CountImportProblems(int importControlId);
        Task UpdateErrorReportPathAsync(int importControlId, string filePath);
        Task<TabImportDataSource> GetImportDataSourceByIdAsync(int importDataSourceId);
        Task<Dictionary<string, string>> GetColumnDescriptionsAsync(int importDataSourceId);
    }
}
