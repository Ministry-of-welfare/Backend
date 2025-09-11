
using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BL.Services
{
    public class BlTabImportDataSourceService : IBlTabImportDataSource
    {
        private readonly IDalImportDataSource _dal;

        public BlTabImportDataSourceService(IDalImportDataSource dal)
        {
            _dal = dal;
        }

        // ממיר DAL -> BL
        public static BlTabImportDataSource ToBl(TabImportDataSource dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new BlTabImportDataSource
            {
                ImportDataSourceId = dto.ImportDataSourceId,
                ImportDataSourceDesc = dto.ImportDataSourceDesc ?? string.Empty,
                DataSourceTypeId = dto.DataSourceTypeId,
                SystemId = dto.SystemId,
                JobName = dto.JobName,
                TableName = dto.TableName,
                UrlFile = dto.UrlFile ?? string.Empty,
                UrlFileAfterProcess = dto.UrlFileAfterProcess ?? string.Empty,
                EndDate = dto.EndDate,
                ErrorRecipients = dto.ErrorRecipients,
                InsertDate = dto.InsertDate,
                StartDate = dto.StartDate
            };
        }

        // ממיר BL -> DAL
        public static TabImportDataSource ToDal(BlTabImportDataSource bl)
        {
            if (bl == null) throw new ArgumentNullException(nameof(bl));

            return new TabImportDataSource
            {
                ImportDataSourceId = bl.ImportDataSourceId,
                ImportDataSourceDesc = bl.ImportDataSourceDesc ?? string.Empty,
                DataSourceTypeId = bl.DataSourceTypeId,
                SystemId = bl.SystemId,
                JobName = bl.JobName,
                TableName = bl.TableName,
                UrlFile = bl.UrlFile ?? string.Empty,
                UrlFileAfterProcess = bl.UrlFileAfterProcess ?? string.Empty,
                EndDate = bl.EndDate,
                ErrorRecipients = bl.ErrorRecipients,
                InsertDate = bl.InsertDate,
                StartDate = bl.StartDate
            };
        }

        // === CRUD רגיל ===
        public async Task<List<BlTabImportDataSource>> GetAll()
        {
            var data = await _dal.GetAll();
            return data.Select(ToBl).ToList();
        }

        public async Task<BlTabImportDataSource> GetById(int id)
        {
            var entity = await _dal.GetById(id);
            return entity == null ? null : ToBl(entity);
        }

        public async Task Create(BlTabImportDataSource item)
        {
            var dalEntity = ToDal(item);
            await _dal.Create(dalEntity);
        }

        //public async Task<BlTabImportDataSource> Update(BlTabImportDataSource item)
        //{
        //    var dalEntity = ToDal(item);
        //    var updated = await _dal.Update(dalEntity);
        //    return ToBl(updated);
        //}

        public async Task Delete(int id)
        {
            await _dal.Delete(id);
        }

        // === פונקציות ייחודיות ליצירת טבלה דינאמית ===
        public string GetTableName(int id) => _dal.GetTableName(id);

        public List<ColumnDef> GetColumns(int id) => _dal.GetColumns(id);

        public bool TableExists(string tableName) => _dal.TableExists(tableName);

        public void ExecuteSql(string sql) => _dal.ExecuteSql(sql);

        /// <summary>
        /// פונקציה ראשית ליצירת הטבלה הדינאמית
        /// </summary>
        //public void CreateDynamicTable(int importDataSourceId)
        //{
        //    var tableName = GetTableName(importDataSourceId);
        //    if (string.IsNullOrWhiteSpace(tableName))
        //        throw new Exception($"לא נמצא שם טבלה ל־ImportDataSourceId {importDataSourceId}");

        //    tableName = $"{tableName}_BULK";



        //    if (TableExists(tableName))
        //        throw new Exception($"הטבלה {tableName} כבר קיימת");

        //    var columns = GetColumns(importDataSourceId);
        //    if (columns == null || !columns.Any())
        //        throw new Exception("לא הוגדרו עמודות לטבלה");

        //    // בניית SQL
        //    var sql = $"CREATE TABLE {tableName} ({string.Join(",", columns.Select(c => $"[{c}] NVARCHAR(MAX)"))})";

        //    ExecuteSql(sql);
        //}

        public void CreateDynamicTable(int importDataSourceId)
        {
            var tableName = GetTableName(importDataSourceId);
            if (string.IsNullOrWhiteSpace(tableName))
                throw new Exception($"לא נמצא שם טבלה ל־ImportDataSourceId {importDataSourceId}");

            tableName = tableName + "_BULK"; // הוספת הסיומת

            if (TableExists(tableName))
                throw new Exception($"הטבלה {tableName} כבר קיימת");

            var columns = _dal.GetColumns(importDataSourceId);
            if (columns == null || !columns.Any())
                throw new Exception("לא הוגדרו עמודות לטבלה");

            // בניית ה־SQL לפי הפורמט הנכון
        

                    var columnsDef = columns
                .Where(c => !string.IsNullOrWhiteSpace(c.ColumnName) && !string.IsNullOrWhiteSpace(c.DataType))
                .Select(c => $"[{c.ColumnName}] {c.DataType}")
                .ToList();

            if (columnsDef.Count == 0)
                throw new InvalidOperationException("No valid columns found to create the table.");

                    var sql = $@"
            CREATE TABLE {tableName} (
                {string.Join(",", columnsDef)}
            )";

            ExecuteSql(sql);
        }
        public Task<BlTabImportDataSource> Update(BlTabImportDataSource item)
        {
            throw new NotImplementedException();
        }

       
    }
}