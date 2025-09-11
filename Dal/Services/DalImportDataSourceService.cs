using Dal.Api;
using Dal.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

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

        public async Task<List<TabImportDataSource>> GetAll()
        {
            return await _db.TabImportDataSources.ToListAsync();
        }

        public async Task<TabImportDataSource?> GetById(int id)
        {
            return await _db.TabImportDataSources.FindAsync(id);
        }

        public async Task<TabImportDataSource> Create(TabImportDataSource item)
        {
            _db.TabImportDataSources.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<TabImportDataSource> Update(TabImportDataSource item)
        {
            _db.TabImportDataSources.Update(item);
            await _db.SaveChangesAsync();
            return item;
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


    }
}
