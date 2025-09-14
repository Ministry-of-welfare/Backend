using Dal.Models;
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
        


        Task<TabImportDataSource> GetById( int id);
        
    }
}
