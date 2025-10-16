using BL.Models;
using Dal.Models;

namespace BL.Api
{
    public interface IBlTabImportDataSource
    {
        Task<List<BlTabImportDataSource>> GetAll();
        Task<BlTabImportDataSource> GetById(int id);
        Task Create(BlTabImportDataSource item);
        Task<BlTabImportDataSource> Update(BlTabImportDataSource item);
        Task<BlTabImportDataSource> UpdateEndDate(int id);

        Task Delete(int id);


        // === פונקציות ייחודיות ליצירת טבלה דינאמית ===
        string GetTableName(int id);
        List<ColumnDef> GetColumns(int id);
        bool TableExists(string tableName);
        void ExecuteSql(string sql);
        void CreateDynamicTable(int importDataSourceId);

        // פונקציית חיפוש למסך קליטות שבוצעו 
        Task<IEnumerable<BlTabImportDataSourceForQuery>> SearchImportDataSourcesAsync(
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




