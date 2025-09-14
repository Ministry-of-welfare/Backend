using BL.Models;
using Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}


  

