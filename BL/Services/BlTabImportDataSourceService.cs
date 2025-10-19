using System.Text.RegularExpressions;
using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
namespace BL.Services

{
    public class BlTabImportDataSourceService : IBlTabImportDataSource
    {
        private readonly IDalImportDataSource _dal;

        public BlTabImportDataSourceService(IDalImportDataSource dal)
        {
            _dal = dal;
        }

        // ���� DAL -> BL
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

        // ���� BL -> DAL
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
            // ����� ���� �������
            if (item.StartDate != null && item.EndDate != null && item.EndDate < item.StartDate)
                throw new InvalidOperationException("����� ����� �� ���� ����� ���� ����� ������.");

            // ����� ������ ������ ���� (�� ��)
            if (!string.IsNullOrWhiteSpace(item.ErrorRecipients))
            {
                var emailPattern = @"^[A-Za-z0-9\u0590-\u05FF._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
                var recipients = item.ErrorRecipients
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var email in recipients)
                {
                    var trimmed = email.Trim();
                    if (!Regex.IsMatch(trimmed, emailPattern))
                        throw new InvalidOperationException($"����� ����� '{trimmed}' ���� �����.");
                }
            }
        }
        // === CRUD ���� ===
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
            ValidateImportDataSource(item); // ������� ���� �����
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
        /// ������� ������ ����� ���� 
        /// </summary>
        /// <returns>importDataSourceId</returns>
        public async Task<int> CreateAndReturnId(BlTabImportDataSource item)
        {
            var dalEntity = ToDal(item);
            await _dal.Create(dalEntity);
            return item.ImportDataSourceId;

        }
        // === �������� �������� ������ ���� ������� ===
        public string GetTableName(int id) => _dal.GetTableName(id);

        public List<ColumnDef> GetColumns(int id) => _dal.GetColumns(id);

        public bool TableExists(string tableName) => _dal.TableExists(tableName);

        public void ExecuteSql(string sql) => _dal.ExecuteSql(sql);

        /// <summary>
        /// ������� ����� ������ ����� ��������
        /// </summary>
       
        public void CreateDynamicTable(int importDataSourceId)
        {
            var tableName = GetTableName(importDataSourceId);
            if (string.IsNullOrWhiteSpace(tableName))
                throw new Exception($"�� ���� �� ���� ��ImportDataSourceId {importDataSourceId}");

            tableName = tableName + "_BULK"; // ����� ������

            if (TableExists(tableName))
                throw new Exception($"����� {tableName} ��� �����");

            var columns = _dal.GetColumns(importDataSourceId);
            if (columns == null || !columns.Any())
                throw new Exception("�� ������ ������ �����");

            // ����� ��SQL ��� ������ �����


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
        public async Task<BlTabImportDataSource> Update(BlTabImportDataSource item)
        {
            ValidateImportDataSource(item); // ������� ���� �����
            var entity = await _dal.GetById(item.ImportDataSourceId);
            if (entity == null) return null!;

            entity.EndDate = item.EndDate ?? DateTime.Now;
            entity.StartDate = item.StartDate;
            entity.ErrorRecipients = item.ErrorRecipients;
            await _dal.Update(entity);     // ����� �-ICrud, �� ����� ���

            return ToBl(entity);
        }

        public async Task<BlTabImportDataSource> UpdateEndDate(int id)
        {

            var entity = await _dal.GetById(id);
            if (entity == null) return null!;

            entity.EndDate = DateTime.Now; // ������ �����
            await _dal.Update(entity);     // ����� �-ICrud, �� ����� ���

            return ToBl(entity);            // ����� BL model �-Controller
        }
        // �������� ����� ���� ������ ������ 
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
            var results = await _dal.SearchImportDataSourcesAsync(
                startDate, endDate, systemId, systemName, importDataSourceDesc, importStatusId, fileName, showErrorsOnly);

            // ����� ������ ������ ������� ������
            var enrichedResults = results
      .Select(async x => new
      {
          DataSource = x,
          ImportControl = await _dal.GetImportControlByDataSourceId(x.ImportDataSourceId), // ���� AppImportControl
          ImportStatus = await _dal.GetImportStatusById(x.ImportDataSourceId), // ���� TImportStatus
          System = await _dal.GetSystemById(x.SystemId ?? -1),
          ErrorCount = x.TabImportErrors?.Count() ?? 0 // ���� ������
      })
      .Select(t => t.Result);


            return enrichedResults.Select(x => new BlTabImportDataSourceForQuery
            {
                ImportControlId = x.DataSource.ImportDataSourceId, // ���� �����
                ImportDataSourceDesc = x.DataSource.ImportDataSourceDesc ?? string.Empty, // ����� ���� �����
                SystemName = x.System.SystemName ?? string.Empty, // �� �����
                FileName = x.DataSource.UrlFile ?? string.Empty, // �� ����
                ImportStartDate = x.ImportControl.ImportStartDate != default ? x.ImportControl.ImportStartDate : DateTime.Now, // ����� ����� �����
                ImportFinishDate = x.ImportControl.ImportFinishDate ?? DateTime.MaxValue, // ����� ���� �����
                TotalRows = x.ImportControl?.TotalRows ?? 0, // �� �� ������ �����
                TotalRowsAffected = x.ImportControl?.TotalRowsAffected ?? 0, // �� ������ ������
                RowsInvalid = x.ErrorCount, // �� ������ �������
                ImportStatusDesc =  x.ImportStatus?.ImportStatusDesc ?? "", // ���� ����� �����
                UrlFileAfterProcess = x.DataSource.UrlFileAfterProcess ?? string.Empty, // ���� ���� ���� �����
                ErrorReportPath = x.ImportControl?.ErrorReportPath ?? string.Empty // ���� ���� ������
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