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
        private void ValidateImportDataSource(BlTabImportDataSource item)
        {
            // בדיקת טווח תאריכים
            if (item.StartDate != null && item.EndDate != null && item.EndDate < item.StartDate)
                throw new InvalidOperationException("תאריך הסיום לא יכול להיות לפני תאריך ההתחלה.");

            // בדיקת תקינות כתובות מייל (אם יש)
            if (!string.IsNullOrWhiteSpace(item.ErrorRecipients))
            {
                var emailPattern = @"^[A-Za-z0-9\u0590-\u05FF._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
                var recipients = item.ErrorRecipients
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var email in recipients)
                {
                    var trimmed = email.Trim();
                    if (!Regex.IsMatch(trimmed, emailPattern))
                        throw new InvalidOperationException($"כתובת המייל '{trimmed}' אינה תקינה.");
                }
            }
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
            ValidateImportDataSource(item); // ולידציה לפני יצירה
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
        /// פונקציה ליצירת רשומה חדשה 
        /// </summary>
        /// <returns>importDataSourceId</returns>
        public async Task<int> CreateAndReturnId(BlTabImportDataSource item)
        {
            var dalEntity = ToDal(item);
            await _dal.Create(dalEntity);
            return item.ImportDataSourceId;

        }
        // === פונקציות ייחודיות ליצירת טבלה דינאמית ===
        public string GetTableName(int id) => _dal.GetTableName(id);

        public List<ColumnDef> GetColumns(int id) => _dal.GetColumns(id);

        public bool TableExists(string tableName) => _dal.TableExists(tableName);

        public void ExecuteSql(string sql) => _dal.ExecuteSql(sql);

        /// <summary>
        /// פונקציה ראשית ליצירת הטבלה הדינאמית
        /// </summary>
       
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
        public async Task<BlTabImportDataSource> Update(BlTabImportDataSource item)
        {
            ValidateImportDataSource(item); // ולידציה לפני עדכון
            var entity = await _dal.GetById(item.ImportDataSourceId);
            if (entity == null) return null!;

            entity.EndDate = item.EndDate ?? DateTime.Now;
            entity.StartDate = item.StartDate;
            entity.ErrorRecipients = item.ErrorRecipients;
            await _dal.Update(entity);     // שימוש ב-ICrud, לא מחזיר ערך

            return ToBl(entity);
        }

        public async Task<BlTabImportDataSource> UpdateEndDate(int id)
        {

            var entity = await _dal.GetById(id);
            if (entity == null) return null!;

            entity.EndDate = DateTime.Now; // לוגיקה עסקית
            await _dal.Update(entity);     // שימוש ב-ICrud, לא מחזיר ערך

            return ToBl(entity);            // החזרת BL model ל-Controller
        }
        // פונקציית חיפוש למסך קליטות שבוצעו 
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

            // טעינת נתונים נוספים מטבלאות קשורות
            var enrichedResults = results
      .Select(async x => new
      {
          DataSource = x,
          ImportControl = await _dal.GetImportControlByDataSourceId(x.ImportDataSourceId), // טבלת AppImportControl
          ImportStatus = await _dal.GetImportStatusById(x.ImportDataSourceId), // טבלת TImportStatus
          System = await _dal.GetSystemById(x.SystemId ?? -1),
          ErrorCount = x.TabImportErrors?.Count() ?? 0 // מספר שגיאות
      })
      .Select(t => t.Result);


            return enrichedResults.Select(x => new BlTabImportDataSourceForQuery
            {
                ImportControlId = x.DataSource.ImportDataSourceId, // מזהה קליטה
                ImportDataSourceDesc = x.DataSource.ImportDataSourceDesc ?? string.Empty, // תיאור מקור קליטה
                SystemName = x.System.SystemName ?? string.Empty, // שם מערכת
                FileName = x.DataSource.UrlFile ?? string.Empty, // שם קובץ
                ImportStartDate = x.ImportControl.ImportStartDate != default ? x.ImportControl.ImportStartDate : DateTime.Now, // תאריך התחלת קליטה
                ImportFinishDate = x.ImportControl.ImportFinishDate ?? DateTime.MaxValue, // תאריך סיום קליטה
                TotalRows = x.ImportControl?.TotalRows ?? 0, // סך כל השורות בקובץ
                TotalRowsAffected = x.ImportControl?.TotalRowsAffected ?? 0, // סך השורות שנקלטו
                RowsInvalid = x.ErrorCount, // סך השורות הפגומות
                ImportStatusDesc =  x.ImportStatus?.ImportStatusDesc ?? "", // מזהה סטטוס קליטה
                UrlFileAfterProcess = x.DataSource.UrlFileAfterProcess ?? string.Empty, // נתיב קובץ לאחר עיבוד
                ErrorReportPath = x.ImportControl?.ErrorReportPath ?? string.Empty // נתיב לדוח שגיאות
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