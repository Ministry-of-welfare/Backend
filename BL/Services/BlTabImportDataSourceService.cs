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

        // Convert DAL -> BL
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
                StartDate = dto.StartDate,
                FileStatusId = dto.FileStatusId, // ×—×•×‘×” ×©×™×”×™×”

            };
        }

        // Convert BL -> DAL
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
                StartDate = bl.StartDate,
                FileStatusId = bl.FileStatusId, // ×—×•×‘×” ×©×™×”×™×”

            };
        }

        private void ValidateImportDataSource(BlTabImportDataSource item)
        {
            // Validate dates
            if (item.StartDate != null && item.EndDate != null && item.EndDate < item.StartDate)
                throw new InvalidOperationException("End date cannot be before start date.");

            // Validate email addresses
            if (!string.IsNullOrWhiteSpace(item.ErrorRecipients))
            {
                var emailPattern = @"^[A-Za-z0-9\u0590-\u05FF._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
                var recipients = item.ErrorRecipients
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var email in recipients)
                {
                    var trimmed = email.Trim();
                    if (!Regex.IsMatch(trimmed, emailPattern))
                        throw new InvalidOperationException($"Invalid email address: '{trimmed}'.");
                }
            }
        }

        // === CRUD Operations ===
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
            ValidateImportDataSource(item);
            var dalEntity = ToDal(item);
            await _dal.Create(dalEntity);
        }

        public async Task Delete(int id)
        {
            await _dal.Delete(id);
        }

        public async Task<int> CreateAndReturnId(BlTabImportDataSource item)
        {
            var dalEntity = ToDal(item);
            var result = await _dal.CreateAndReturnId(dalEntity);
            return result;
        }

        // === Dynamic Table Operations ===
        public string GetTableName(int id) => _dal.GetTableName(id);

        public List<ColumnDef> GetColumns(int id) => _dal.GetColumns(id);

        public bool TableExists(string tableName) => _dal.TableExists(tableName);

        public void ExecuteSql(string sql) => _dal.ExecuteSql(sql);

        public void CreateDynamicTable(int importDataSourceId)
        {
            var tableName = GetTableName(importDataSourceId);
            if (string.IsNullOrWhiteSpace(tableName))
                throw new Exception($"Table name not found for ImportDataSourceId {importDataSourceId}");

            var cleanTableName = tableName.Replace("BULK_", "", StringComparison.OrdinalIgnoreCase)
                                          .Replace("APP_", "", StringComparison.OrdinalIgnoreCase)
                                          .Replace("App_", "", StringComparison.OrdinalIgnoreCase)
                                          .Replace("app_", "", StringComparison.OrdinalIgnoreCase);
            tableName = "BULK_" + cleanTableName;

            if (TableExists(tableName))
                throw new Exception($"Table {tableName} already exists");

            var columns = _dal.GetColumns(importDataSourceId);
            if (columns == null || !columns.Any())
                throw new Exception("No columns found");

            var columnsDef = new List<string>
            {
                $"[{cleanTableName}Id] INT IDENTITY(1,1) PRIMARY KEY",
                "[ImportControlId] INT"
            };

            columnsDef.AddRange(columns
                .Where(c => !string.IsNullOrWhiteSpace(c.ColumnName))
                .Select(c => $"[{c.ColumnName}] VARCHAR(MAX)"));

            if (columnsDef.Count == 0)
                throw new InvalidOperationException("No valid columns found to create the table.");

            var sql = $@"
            CREATE TABLE [{tableName}] (
                {string.Join(",\r\n                ", columnsDef)},
                CONSTRAINT FK_{cleanTableName}_ImportControl FOREIGN KEY (ImportControlId) REFERENCES APP_ImportControl(ImportControlId)
            )";

            ExecuteSql(sql);
            _dal.ExecuteSql($"UPDATE TAB_ImportDataSource SET StartDate = GETDATE() WHERE ImportDataSourceId = {importDataSourceId}");
        }

        public async Task<BlTabImportDataSource> Update(BlTabImportDataSource item)
        {
            ValidateImportDataSource(item);
            var entity = await _dal.GetById(item.ImportDataSourceId);
            if (entity == null) return null!;

            entity.EndDate = item.EndDate ?? DateTime.Now;
            entity.StartDate = item.StartDate;
            entity.ErrorRecipients = item.ErrorRecipients;
            await _dal.Update(entity);

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
            }).Where(query => query.ImportControlId != 0); 
        }

        public async Task AdditionalMethod1()
        {
            await Task.CompletedTask;
        }

        public async Task AdditionalMethod2()
        {
            await Task.CompletedTask;
        }
    }
}