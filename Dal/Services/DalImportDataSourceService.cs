using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.Api;
using Dal.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    public class DalImportDataSourceService : IDalImportDataSource
    {
        private readonly AppDbContext _db;

        private readonly string _connectionString;

        public DalImportDataSourceService(AppDbContext db)
        {
            _db = db;
            _connectionString = _db.Database.GetConnectionString();
        }
        public IQueryable<TabImportDataSource> GetTabImportDataSourcesQuery()
        {
            return _db.TabImportDataSources
                .Include(x => x.AppImportControls)
                .ThenInclude(control => control.ImportStatus)
               // .Include(x => x.System) // טעינת המערכת
                .Include(x => x.TabImportErrors);
        }


        public async Task<List<TabImportDataSource>> GetAll()
        {
            return await _db.TabImportDataSources.ToListAsync();
        }

        public async Task<TabImportDataSource?> GetById(int id)
        {
            return await _db.TabImportDataSources.FindAsync(id);
        }

        public async Task Create(TabImportDataSource item)
        {
            _db.TabImportDataSources.Add(item);
            await _db.SaveChangesAsync();

        }
        public async Task<int> CreateAndReturnId(TabImportDataSource item)
        {
            _db.TabImportDataSources.Add(item);
            await _db.SaveChangesAsync();
            return item.ImportDataSourceId;
        }


        public async Task Update(TabImportDataSource item)
        {
            _db.TabImportDataSources.Update(item);
            await _db.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var entity = await _db.TabImportDataSources.FindAsync(id);
            if (entity != null)
            {
                _db.TabImportDataSources.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }



        Task ICrud<TabImportDataSource>.Update(TabImportDataSource item)
        {
            return Update(item);
        }

        Task ICrud<TabImportDataSource>.Create(TabImportDataSource item)
        {
            return Create(item);
        }



        /// <summary>
        /// הוספת טבלה בצורה דינאמית
        //
        /// </summary>

        public string GetTableName(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT TableName FROM TAB_ImportDataSource WHERE ImportDataSourceId = @Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            return cmd.ExecuteScalar()?.ToString() ?? "";
        }



        //public List<string> GetColumns(int id)
        //{
        //    var list = new List<string>();
        //    using var conn = new SqlConnection(_connectionString);
        //    conn.Open();

        //    var sql = "SELECT ColumnName FROM TAB_ImportDataSourceColumns WHERE ImportDataSourceId = @Id";
        //    using var cmd = new SqlCommand(sql, conn);
        //    cmd.Parameters.AddWithValue("@Id", id);

        //    using var reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //        list.Add(reader.GetString(0));

        //    return list;
        //}
        private string MapToSqlType(string formatColumnDesc)
        {
            return formatColumnDesc switch
            {
                "int" => "INT",
                "varchar" => "NVARCHAR(200)",
                "DECIMAL" => "DECIMAL(18,2)",
                "DATE" => "DATE",
                "DATETIME" => "DATETIME",
                // ערכים "ידידותיים" מהממשק -> טיפוס SQL
                "מספר" => "INT",
                "טקסט" => "NVARCHAR(200)",
                "תאריך" => "DATE",
                _ => "NVARCHAR(200)" // ברירת מחדל בטוחה
            };
        }
        public List<ColumnDef> GetColumns(int id)
        {
            var list = new List<ColumnDef>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var sql = @"
        SELECT c.ColumnName, f.FormatColumnDesc
        FROM TAB_ImportDataSourceColumns c
        INNER JOIN TAB_FormatColumn f ON c.FormatColumnId = f.FormatColumnId
        WHERE c.ImportDataSourceId = @Id
        ORDER BY c.OrderId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var sqlType = MapToSqlType(reader.GetString(1));
                list.Add(new ColumnDef(reader.GetString(0), sqlType));
            }

            return list;
        }

        public T GetFieldValue<T>(string table, string field, int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var sql = $"SELECT {field} FROM {table} WHERE ImportDataSourceId = @Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            var result = cmd.ExecuteScalar();
            if (result == null || result == DBNull.Value) return default!;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public bool SystemTypeExists(int systemTypeId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT COUNT(*) FROM SystemTypes WHERE Id = @Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", systemTypeId);

            return (int)cmd.ExecuteScalar() > 0;
        }

        public bool TableExists(string tableName)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(@Name) AND type = 'U'";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", tableName);

            return (int)cmd.ExecuteScalar() > 0;
        }

        public void ExecuteSql(string sql)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        // פונקציית חיפוש למסך קליטות שבוצעו 

        public async Task<IEnumerable<TabImportDataSource>> SearchImportDataSourcesAsync(
      DateTime? startDate,
      DateTime? endDate,
      int? systemId,
      string systemName,
      string importDataSourceDesc,
      int? importStatusId,
      string fileName,
      bool showErrorsOnly)
        {
            // התחלת שאילתה על בסיס DbSet
            var query = _db.TabImportDataSources
                .Include(x => x.DataSourceType) // טבלת סוגי מקור קליטה
                .Include(x => x.TabImportErrors) // טבלת שגיאות קליטה
                .AsQueryable();

            // סינון לפי תאריך התחלה
            if (startDate.HasValue)
                query = query.Where(x => x.StartDate.HasValue && x.StartDate.Value >= startDate.Value);

            // סינון לפי תאריך סיום
            if (endDate.HasValue)
                query = query.Where(x => x.EndDate.HasValue && x.EndDate.Value <= endDate.Value);

            // סינון לפי מזהה מערכת
            if (systemId.HasValue)
                query = query.Where(x => x.SystemId.HasValue && x.SystemId.Value == systemId.Value);

            // סינון לפי שם מערכת (טבלת System)
            if (!string.IsNullOrEmpty(systemName))
            {
                query = query.Join(
                    _db.Systems,
                    dataSource => dataSource.SystemId,
                    system => system.SystemId,
                    (dataSource, system) => new { dataSource, system }
                )
                .Where(joined => joined.system.SystemName.Contains(systemName))
                .Select(joined => joined.dataSource);
            }

            // סינון לפי תיאור מקור קליטה
            if (!string.IsNullOrEmpty(importDataSourceDesc))
                query = query.Where(x => !string.IsNullOrEmpty(x.ImportDataSourceDesc) && x.ImportDataSourceDesc.Contains(importDataSourceDesc));

            // סינון לפי סטטוס קליטה (טבלת TImportStatus)
            if (importStatusId.HasValue)
            {
                query = query.Join(
                    _db.AppImportControls,
                    dataSource => dataSource.ImportDataSourceId,
                    control => control.ImportDataSourceId,
                    (dataSource, control) => new { dataSource, control }
                )
                .Where(joined => joined.control.ImportStatusId == importStatusId.Value)
                .Select(joined => joined.dataSource);
            }

            // סינון לפי שם קובץ
            if (!string.IsNullOrEmpty(fileName))
                query = query.Where(x => !string.IsNullOrEmpty(x.UrlFile) && x.UrlFile.Contains(fileName));

            // סינון לפי שורות פגומות
            if (showErrorsOnly)
                query = query.Where(x => x.TabImportErrors.Any(error => error.ImportErrorId > 0));

            // החזרת התוצאות
            return await query.ToListAsync();
        }
        public async Task<AppImportControl?> GetImportControlByDataSourceId(int importDataSourceId)
        {
            return await _db.AppImportControls
                .FirstOrDefaultAsync(control => control.ImportDataSourceId == importDataSourceId);
        }

        public async Task<TImportStatus> GetImportStatusById(int importStatusId)
        {
            return await _db.TImportStatuses
                .FirstOrDefaultAsync(status => status.ImportStatusId == importStatusId);
        }


        public async Task<Dal.Models.System> GetSystemById(int systemId)
        {
            return await _db.Systems
                .FirstOrDefaultAsync(system => system.SystemId == systemId);
        }







    }
}
