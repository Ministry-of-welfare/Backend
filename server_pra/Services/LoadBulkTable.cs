using System;
using System.Data;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Linq;
using Dal.Models;

namespace server_pra.Services
{
    public class LoadBulkTable
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoadBulkTable> _logger;

        public LoadBulkTable(AppDbContext context, ILogger<LoadBulkTable> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LoadBulkData(int importDataSourceId, int importControlId)
        {
            try
            {
                // שליפת נתוני המקור
                var dataSource = await _context.TabImportDataSources
                    .FirstOrDefaultAsync(x => x.ImportDataSourceId == importDataSourceId);

                if (dataSource == null)
                {
                    throw new ArgumentException($"Data source not found for ImportDataSourceId: {importDataSourceId}");
                }
                
                _logger.LogInformation($"Found data source - UrlFile: '{dataSource.UrlFile}', TableName: '{dataSource.TableName}'");
                
                if (string.IsNullOrEmpty(dataSource.UrlFile))
                {
                    throw new ArgumentException($"UrlFile is empty for ImportDataSourceId: {importDataSourceId}");
                }
                
                if (string.IsNullOrEmpty(dataSource.TableName))
                {
                    throw new ArgumentException($"TableName is empty for ImportDataSourceId: {importDataSourceId}");
                }

                // עדכון סטטוס לבעיבוד (2)
                dataSource.FileStatusId = 2;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated FileStatusId to 2 for ImportDataSourceId: {importDataSourceId}");

                // קריאת קובץ Excel
                var dataTable = ReadExcelFile(dataSource.UrlFile);

                // חיפוש הטבלה המתאימה
                var bulkTableName = await FindBulkTableName(dataSource.TableName);

                // מילוי הטבלה
                await PopulateBulkTable(bulkTableName, dataTable, importDataSourceId);

                _logger.LogInformation($"Successfully loaded data for ImportDataSourceId: {importDataSourceId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading bulk data for ImportDataSourceId: {importDataSourceId}");
                throw;
            }
        }

        private DataTable ReadExcelFile(string filePath)
        {
            _logger.LogInformation($"Processing file: {filePath}");
            
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path is empty");
            }
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
            
            var extension = Path.GetExtension(filePath).ToLower();
            _logger.LogInformation($"File extension: {extension}");
            
            if (extension == ".csv")
            {
                return ReadCsvFile(filePath);
            }
            else
            {
                throw new NotSupportedException($"File '{Path.GetFileName(filePath)}' has extension '{extension}'. Only CSV files are supported. Please convert to CSV format.");
            }
        }
        
        private DataTable ReadCsvFile(string filePath)
        {
            var dataTable = new DataTable();
            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            
            if (lines.Length <= 1) return dataTable; // אם אין נתונים או רק כותרת
            
            // קביעת מספר העמודות לפי שורת הכותרת (שורה ראשונה)
            var headerLine = lines[0].Split(',');
            for (int i = 0; i < headerLine.Length; i++)
            {
                dataTable.Columns.Add($"Column{i + 1}", typeof(string));
            }
            
            // קריאת שורות הנתונים (מדלגים על שורת הכותרת)
            for (int lineIndex = 1; lineIndex < lines.Length; lineIndex++)
            {
                var values = lines[lineIndex].Split(',');
                var row = dataTable.NewRow();
                
                for (int i = 0; i < Math.Min(values.Length, dataTable.Columns.Count); i++)
                {
                    row[i] = values[i].Trim('"').Trim();
                }
                
                dataTable.Rows.Add(row);
            }
            
            return dataTable;
        }

        private async Task<string> FindBulkTableName(string tableName)
        {
            var connectionString = _context.Database.GetConnectionString();
            
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                
                var query = @"
                    SELECT TABLE_NAME 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME LIKE 'BULK%' AND TABLE_NAME LIKE @TableName
                    AND TABLE_SCHEMA = 'dbo'";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableName", $"%{tableName}%");
                    var result = await command.ExecuteScalarAsync();
                    
                    if (result == null)
                    {
                        throw new InvalidOperationException($"No BULK table found containing: {tableName}");
                    }
                    
                    return result.ToString();
                }
            }
        }

        private async Task PopulateBulkTable(string tableName, DataTable dataTable, int importDataSourceId)
        {
            var connectionString = _context.Database.GetConnectionString();
            
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                
                // יצירת DataTable זמני בזיכרון לצורך SqlBulkCopy
                var bulkDataTable = new DataTable();
                bulkDataTable.Columns.Add("Col0", typeof(int)); // ImportDataSourceId
                
                // הוספת עמודות נתוני הקובץ
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    bulkDataTable.Columns.Add($"Col{i + 1}", typeof(string));
                }

                // מילוי הנתונים
                foreach (DataRow sourceRow in dataTable.Rows)
                {
                    var newRow = bulkDataTable.NewRow();
                    newRow[0] = importDataSourceId; // עמודה 1: ImportDataSourceId
                    
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        newRow[i + 1] = sourceRow[i]; // עמודות 2,3,4...: נתוני הקובץ
                    }
                    
                    bulkDataTable.Rows.Add(newRow);
                }

                // שימוש ב-SqlBulkCopy - מדלג על עמודה 0 (אוטומטי)
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    
                    // מיפוי: עמודה 0 שלנו → עמודה 1 בטבלה, עמודה 1 שלנו → עמודה 2 בטבלה וכו'
                    for (int i = 0; i < bulkDataTable.Columns.Count; i++)
                    {
                        bulkCopy.ColumnMappings.Add(i, i + 1); // מדלג על עמודה 0 (אוטומטי)
                    }
                    
                    await bulkCopy.WriteToServerAsync(bulkDataTable);
                }
            }
        }
    }
}
