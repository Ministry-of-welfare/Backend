using Dal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Api
{
    public interface IDalImportDataSource : ICrud<TabImportDataSource>
    {
        string GetTableName(int id);
        List<ColumnDef> GetColumns(int id);
        bool TableExists(string tableName);
        void ExecuteSql(string sql);
        Task<TabImportDataSource> GetById(int id);
        Task<AppImportControl> GetImportControlByDataSourceId(int importDataSourceId);
        Task<TImportStatus> GetImportStatusById(int importStatusId);
        Task<Dal.Models.System> GetSystemById(int systemId);
        Task<IEnumerable<TabImportDataSource>> SearchImportDataSourcesAsync(
           DateTime? startDate,
           DateTime? endDate,
           int? systemId,
           string systemName,
           string importDataSourceDesc,
           int? importStatusId,
           string fileName,
           bool showErrorsOnly);
    }
}
