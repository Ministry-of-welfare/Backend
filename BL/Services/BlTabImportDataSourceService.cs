using System.Text.RegularExpressions;
using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
namespace BL.Services

{
    public class BlTabImportDataSourceService : IBlTabImportDataSource
    {
        private readonly IDalImportDataSource _dal;

        public BlTabImportDataSourceService(IDalImportDataSource dal)
        {
            _dal = dal;
        }

        // ï¿½ï¿½ï¿½ï¿½ DAL -> BL
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

        // ï¿½ï¿½ï¿½ï¿½ BL -> DAL
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
        private void ValidateImportDataSource(BlTabImportDataSource item)
        {
            // ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            if (item.StartDate != null && item.EndDate != null && item.EndDate < item.StartDate)
                throw new InvalidOperationException("ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½.");

            // ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ (ï¿½ï¿½ ï¿½ï¿½)
            if (!string.IsNullOrWhiteSpace(item.ErrorRecipients))
            {
                var emailPattern = @"^[A-Za-z0-9\u0590-\u05FF._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
                var recipients = item.ErrorRecipients
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var email in recipients)
                {
                    var trimmed = email.Trim();
                    if (!Regex.IsMatch(trimmed, emailPattern))
                        throw new InvalidOperationException($"ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ '{trimmed}' ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½.");
                }
            }
        }
        // === CRUD ï¿½ï¿½ï¿½ï¿½ ===
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
            ValidateImportDataSource(item); // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½
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
        /// <summary>
        /// ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ 
        /// </summary>
        /// <returns>importDataSourceId</returns>
        public async Task<int> CreateAndReturnId(BlTabImportDataSource item)
        {
            var dalEntity = ToDal(item);
            var result= await _dal.CreateAndReturnId(dalEntity);
            
            return result;

        }
        // === ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ===
        public string GetTableName(int id) => _dal.GetTableName(id);

        public List<ColumnDef> GetColumns(int id) => _dal.GetColumns(id);

        public bool TableExists(string tableName) => _dal.TableExists(tableName);

        public void ExecuteSql(string sql) => _dal.ExecuteSql(sql);

        /// <summary>
        /// ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
        /// </summary>
       
        public void CreateDynamicTable(int importDataSourceId)
        {
            var tableName = GetTableName(importDataSourceId);
            if (string.IsNullOrWhiteSpace(tableName))
                throw new Exception($"ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ImportDataSourceId {importDataSourceId}");

            tableName = tableName + "_BULK"; // ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½

            if (TableExists(tableName))
                throw new Exception($"ï¿½ï¿½ï¿½ï¿½ï¿½ {tableName} ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½");

            var columns = _dal.GetColumns(importDataSourceId);
            if (columns == null || !columns.Any())
                throw new Exception("ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½");

            // ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½SQL ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½


            var columnsDef = columns
        .Where(c => !string.IsNullOrWhiteSpace(c.ColumnName) && !string.IsNullOrWhiteSpace(c.DataType))
        .SelectMany(c => new[] {
            $"[{c.ColumnName}] {c.DataType}",
            !string.IsNullOrWhiteSpace(c.ColumnNameHeb) ? $"[{c.ColumnNameHeb}] {c.DataType}" : null
        })
        .Where(col => col != null)
        .ToList();

            if (columnsDef.Count == 0)
                throw new InvalidOperationException("No valid columns found to create the table.");

            var sql = $@"
            CREATE TABLE {tableName} (
                {string.Join(",", columnsDef)}
            )";

            ExecuteSql(sql);
        }
        public async Task<BlTabImportDataSource> Update(BlTabImportDataSource item)
        {
            ValidateImportDataSource(item); // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½
            var entity = await _dal.GetById(item.ImportDataSourceId);
            if (entity == null) return null!;

            entity.EndDate = item.EndDate ?? DateTime.Now;
            entity.StartDate = item.StartDate;
            entity.ErrorRecipients = item.ErrorRecipients;
            await _dal.Update(entity);     // ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½-ICrud, ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½

            return ToBl(entity);
        }

        public async Task<BlTabImportDataSource> UpdateEndDate(int id)
        {

            var entity = await _dal.GetById(id);
            if (entity == null) return null!;

            entity.EndDate = DateTime.Now; // ìåâé÷ä òñ÷éú
            entity.FileStatusId = 2; // îòáéø àú äøùåîä ìîöá 'ìà ôòéì'

            await _dal.Update(entity);     // ùéîåù á-ICrud, ìà îçæéø òøê


            return ToBl(entity);            // äçæøú BL model ì-Controller

        }
        public async Task<IEnumerable<BlTabImportDataSourceForQuery>> SearchImportDataSourcesAsync(
      DateTime? startDate,
      DateTime? endDate,
      int? systemId,
      string systemName,
      string importDataSourceDesc,
      int? importStatusId,
      string fileName,
      bool showErrorsOnly)
        {
            var query = _dal.GetTabImportDataSourcesQuery();

            if (startDate.HasValue)
                query = query.Where(x => x.StartDate.HasValue && x.StartDate.Value >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(x => x.EndDate.HasValue && x.EndDate.Value <= endDate.Value);

            if (systemId.HasValue)
                query = query.Where(x => x.SystemId.HasValue && x.SystemId.Value == systemId.Value);

            if (!string.IsNullOrEmpty(systemName))
               query = query.Where(x => x.System.SystemName.Contains(systemName));

            if (!string.IsNullOrEmpty(importDataSourceDesc))
                query = query.Where(x => x.ImportDataSourceDesc.Contains(importDataSourceDesc));

            if (importStatusId.HasValue)
                query = query.Where(x => x.AppImportControls.Any(control => control.ImportStatusId == importStatusId.Value));

            if (!string.IsNullOrEmpty(fileName))
                query = query.Where(x => x.UrlFile.Contains(fileName));

            if (showErrorsOnly)
                query = query.Where(x => x.TabImportErrors.Any());

            var results = await query.ToListAsync();


            // ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            return results.Select(x => new BlTabImportDataSourceForQuery
            {
                ImportControlId = x.AppImportControls.FirstOrDefault()?.ImportControlId ?? 0,
                ImportDataSourceDesc = x.ImportDataSourceDesc ?? string.Empty,
                SystemName = x.System?.SystemName ?? string.Empty,
                FileName = x.UrlFile ?? string.Empty,
                ImportStartDate = x.AppImportControls.FirstOrDefault()?.ImportStartDate ?? DateTime.Now,
                ImportFinishDate = x.AppImportControls.FirstOrDefault()?.ImportFinishDate ?? DateTime.MaxValue,
                TotalRows = x.AppImportControls.FirstOrDefault()?.TotalRows ?? 0,
                TotalRowsAffected = x.AppImportControls.FirstOrDefault()?.TotalRowsAffected ?? 0,
                RowsInvalid = x.TabImportErrors.Count,
                ImportStatusDesc = x.AppImportControls.FirstOrDefault()?.ImportStatus?.ImportStatusDesc ?? string.Empty,
                UrlFileAfterProcess = x.UrlFileAfterProcess ?? string.Empty,
                ErrorReportPath = x.AppImportControls.FirstOrDefault()?.ErrorReportPath ?? string.Empty
            });
        }

        // Adding implementations for the missing methods 'AdditionalMethod1' and 'AdditionalMethod2' to resolve the errors.

        public async Task AdditionalMethod1()
        {
            // Implementation for AdditionalMethod1
            // Add your logic here or leave it as a placeholder if no specific functionality is required yet.
            await Task.CompletedTask;
        }

        public async Task AdditionalMethod2()
        {
            // Implementation for AdditionalMethod2
            // Add your logic here or leave it as a placeholder if no specific functionality is required yet.
            await Task.CompletedTask;
        }


    }
}