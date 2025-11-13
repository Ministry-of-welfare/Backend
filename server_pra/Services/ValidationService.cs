using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace server_pra.Services
{
    public class ValidationService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(AppDbContext dbContext, ILogger<ValidationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> ValidateAsync(int importControlId)
        {
            try
            {
          //      Console.WriteLine("Starting validation for ImportControlId {ImportControlId}", importControlId);

                var rules = await GetValidationRulesAsync(importControlId);
                var bulkData = await LoadBulkDataAsync(importControlId);
          //      Console.WriteLine($"Rules count: {rules?.Count ?? 0}");
           //     Console.WriteLine($"Bulk data rows count: {bulkData?.Rows.Count ?? 0}");

                var errors = ExecuteValidationRules(rules, bulkData);

                try
                {
                    await WriteErrorsAsync(importControlId, errors);
                }
                catch (Exception ex)
                {
                 //   Console.WriteLine("Failed to write errors to the database.");
                    throw;
                }

                await UpdateImportControlStatusAsync(importControlId, errors.Any());
              //  Console.WriteLine("Validation completed for ImportControlId {ImportControlId}", importControlId);
                if (errors == null)
                {
                    throw new InvalidOperationException("The errors collection is null.");
                }


              //  Console.WriteLine($"Errors count: {errors?.Count ?? 0}");
                return errors.Any();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation failed for ImportControlId {ImportControlId}", importControlId);
                throw;
            }
        }

        private async Task<List<TabValidationRule>> GetValidationRulesAsync(int importControlId)
        {
            var importDataSourceId = await _dbContext.AppImportControls
                .Where(ic => ic.ImportControlId == importControlId)
                .Select(ic => ic.ImportDataSourceId)
                .FirstOrDefaultAsync();

            if (importDataSourceId == 0)
                throw new InvalidOperationException($"ImportControlId {importControlId} not found.");

            var rules = await _dbContext.TabValidationRules
                .Where(r => r.ImportDataSourceId == importDataSourceId && r.IsEnabled)
                .Include(r => r.Conditions)
                .Include(r => r.Asserts)
                .ToListAsync();
         //   Console.WriteLine($"Validation rules count: {rules.Count}");
            //foreach (var rule in rules)
            //{
          //      Console.WriteLine($"Rule: {rule.RuleName}, Enabled: {rule.IsEnabled}");
           // }


            return rules;
        }

        private async Task<DataTable> LoadBulkDataAsync(int importControlId)
        {
            var importDataSourceId = await _dbContext.AppImportControls
                .Where(x => x.ImportControlId == importControlId)
                .Select(x => x.ImportDataSourceId)
                .FirstAsync();

            var tableName = await _dbContext.TabImportDataSources
                .Where(x => x.ImportDataSourceId == importDataSourceId)
                .Select(x => x.TableName)
                .FirstAsync();
           // Console.WriteLine("Loading bulk data from table name: {TableName}", tableName);

            var sql = $"SELECT * FROM [BULK_{tableName}]"; var connection = _dbContext.Database.GetDbConnection();

            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = sql;

            var dt = new DataTable();
            using (var reader = await command.ExecuteReaderAsync())
                dt.Load(reader);

            await connection.CloseAsync();
            _logger.LogInformation($"Bulk data rows count: {dt.Rows.Count}");
            foreach (DataRow row in dt.Rows)
            {
                _logger.LogInformation($"Row data: {string.Join(", ", row.ItemArray)}");
            }

            return dt;
        }

        private List<ValidationError> ExecuteValidationRules(List<TabValidationRule> rules, DataTable bulkData)
        {

            var errors = new List<ValidationError>();

            foreach (var rule in rules)
            {
             //   Console.WriteLine($"Processing rule: {rule.RuleName}");
              //  Console.WriteLine($"Errors count after rule: {errors.Count}");

                foreach (DataRow row in bulkData.Rows)
                {
                    bool conditionOk = !rule.Conditions.Any() || EvaluateConditions((List<TabValidationRuleCondition>)rule.Conditions, row);

                    if (conditionOk)
                    {
                        bool assertOk = EvaluateAsserts((List<TabValidationRuleAssert>)rule.Asserts, row);
                        if (!assertOk)
                        {
                            var err = new ValidationError
                            {
                                ErrorRow = row.Table.Columns.Contains("RowNumber") ? row["RowNumber"].ToString() : "0",
                                ErrorColumn = rule.ErrorColumnName,
                                ErrorValue = row.Table.Columns.Contains(rule.ErrorColumnName) ? row[rule.ErrorColumnName]?.ToString() : "",
                                ErrorDetail = $"Rule '{rule.RuleName}' failed for column '{rule.ErrorColumnName}'",
                                ImportErrorId = rule.ImportErrorId
                            };
                            _logger.LogWarning($"? Rule failed: {rule.RuleName}, Column={rule.ErrorColumnName}");

                            errors.Add(err);
                        }
                      //  Console.WriteLine($"Processing rule: {rule.RuleName}");
                      //  Console.WriteLine($"Errors count after rule: {errors.Count}");

                    }
                }
            }

            return errors;
        }

        private bool EvaluateConditions(List<TabValidationRuleCondition> conditions, DataRow row)
        {
            var grouped = conditions.GroupBy(c => c.GroupNo);
            var groupResults = new List<bool>();

            foreach (var group in grouped)
            {
                bool groupResult = group.First().GroupOperator?.ToUpper() == "OR" ? false : true;
                foreach (var cond in group)
                {
                    bool condResult = EvaluateCondition(cond, row);
                    groupResult = cond.GroupOperator?.ToUpper() == "OR"
                        ? (groupResult || condResult)
                        : (groupResult && condResult);
                }
                groupResults.Add(groupResult);
            }

            return groupResults.Any(r => r);
        }

        private bool EvaluateAsserts(List<TabValidationRuleAssert> asserts, DataRow row)
        {
            Console.WriteLine($"Asserts count: {asserts?.Count() ?? 0}");

            if (asserts == null)
            {
                throw new InvalidOperationException("The asserts collection is null.");
            }


            var grouped = asserts.Where(a => a.GroupNo != null).GroupBy(a => a.GroupNo);
            Console.WriteLine($"Grouped asserts count: {grouped.Count()}");

            foreach (var assert in asserts)
            {
                Console.WriteLine($"As sert: GroupNo={assert.GroupNo}, Operator={assert.Operator}, GroupOperator={assert.GroupOperator}");
            }

            var groupResults = new List<bool>();

            foreach (var group in grouped)
            {
                bool groupResult = group.First().GroupOperator?.ToUpper() == "OR" ? false : true;
                foreach (var a in group)
                {
                    bool aResult = EvaluateAssertion(a, row);
                    Console.WriteLine($"Evaluating assertion: GroupNo={a.GroupNo}, Result={aResult}");
                    groupResult = a.GroupOperator?.ToUpper() == "OR"
                        ? (groupResult || aResult)
                        : (groupResult && aResult);
                }
                groupResults.Add(groupResult);
            }
            _logger.LogInformation($"Group results: {string.Join(", ", groupResults)}");

            return groupResults.All(r => r);
        }

        private bool EvaluateCondition(TabValidationRuleCondition cond, DataRow row)
        {
            var leftValue = GetValue(cond.LeftRefType, cond.LeftColumnName, cond.LeftConstValue, cond.LeftFunc, row);
            var rightValue = GetValue(cond.RightRefType, cond.RightColumnName, cond.RightConstValue, cond.RightFunc, row);
            return EvaluateOperator(cond.Operator, leftValue, rightValue);
        }

        private bool EvaluateAssertion(TabValidationRuleAssert a, DataRow row)
        {
            var leftValue = GetValue(a.LeftRefType, a.LeftColumnName, a.LeftConstValue, a.LeftFunc, row);
            var rightValue = GetValue(a.RightRefType, a.RightColumnName, a.RightConstValue, a.RightFunc, row);
            return EvaluateOperator(a.Operator, leftValue, rightValue);
        }

        private object GetValue(string refType, string? columnName, string? constValue, string? func, DataRow row)
        {
            if (string.IsNullOrEmpty(refType))
                throw new InvalidOperationException("RefType is required.");

            switch (refType.ToLower())
            {
                case "column":
                    return row[columnName ?? throw new ArgumentNullException(nameof(columnName))];
                case "constant":
                    return constValue ?? "";
                case "function":
                    return ApplyFunction(func, row, columnName);
                case "lookup":
                    return LookupValueExists(constValue, columnName, row);
                default:
                    throw new InvalidOperationException($"Unsupported RefType: {refType}");
            }
        }

        private object ApplyFunction(string? func, DataRow row, string? columnName)
        {
            if (string.IsNullOrEmpty(func)) throw new ArgumentNullException(nameof(func));

            switch (func.ToLower())
            {
                case "len":
                    var val = columnName != null ? row[columnName]?.ToString() : row.ToString();
                    return val?.Length ?? 0;
                case "isdate":
                    var str = columnName != null ? row[columnName]?.ToString() : row.ToString();
                    return DateTime.TryParse(str, out _);
                case "getdate":
                    return DateTime.Now;
                default:
                    throw new InvalidOperationException($"Unsupported function: {func}");
            }
        }

        private bool LookupValueExists(string? lookupTable, string? lookupColumn, DataRow row)
        {
            _logger.LogInformation($"Row data: {string.Join(", ", row.ItemArray)}");

            if (string.IsNullOrEmpty(lookupTable) || string.IsNullOrEmpty(lookupColumn))
                return false;

            var value = row[lookupColumn]?.ToString();
            if (string.IsNullOrEmpty(value))
                return false;

            var sql = $"SELECT COUNT(1) FROM {lookupTable} WHERE {lookupColumn} = @p0";
            using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            var param = command.CreateParameter();
            param.ParameterName = "@p0";
            param.Value = value;
            command.Parameters.Add(param);

            _dbContext.Database.OpenConnection();
            var count = Convert.ToInt32(command.ExecuteScalar());
            _dbContext.Database.CloseConnection();
            Console.WriteLine($"Executing lookup SQL: {sql}, Param: {param.Value}");

            _logger.LogInformation($"Executing lookup SQL: {sql}, Param: {param.Value}");

            return count > 0;
        }

        private bool EvaluateOperator(string opType, object left, object right)
        {
            if (string.IsNullOrEmpty(opType))
                throw new ArgumentNullException(nameof(opType));

            string op = opType.ToUpper();
            string leftStr = left?.ToString() ?? "";
            string rightStr = right?.ToString() ?? "";

            // טיפוסי עזר מספריים
            bool leftIsNum = double.TryParse(leftStr, out double leftNum);
            bool rightIsNum = double.TryParse(rightStr, out double rightNum);

            switch (op)
            {
                case "=":
                    return leftStr == rightStr;

                case "!=":
                    return leftStr != rightStr;

                case ">":
                    return (leftIsNum && rightIsNum) && (leftNum > rightNum);

                case "<":
                    return (leftIsNum && rightIsNum) && (leftNum < rightNum);

                case ">=":
                    return (leftIsNum && rightIsNum) && (leftNum >= rightNum);

                case "<=":
                    return (leftIsNum && rightIsNum) && (leftNum <= rightNum);

                case "IS NULL":
                    return string.IsNullOrWhiteSpace(leftStr);

                case "IS NOT NULL":
                    return !string.IsNullOrWhiteSpace(leftStr);

                case "IN":
                    return rightStr.Split(',').Select(s => s.Trim()).Contains(leftStr);

                case "NOT IN":
                    return !rightStr.Split(',').Select(s => s.Trim()).Contains(leftStr);

                case "BETWEEN":
                    {
                        var range = rightStr.Split(',');
                        if (range.Length != 2)
                            throw new InvalidOperationException("BETWEEN requires two comma-separated values.");

                        if (double.TryParse(leftStr, out double val)
                            && double.TryParse(range[0], out double min)
                            && double.TryParse(range[1], out double max))
                            return val >= min && val <= max;

                        return false;
                    }

                case "MATCHES":
                    return Regex.IsMatch(leftStr, rightStr);

                default:
                    throw new InvalidOperationException($"Unsupported operator: {opType}");
            }
        }

        private async Task WriteErrorsAsync(int importControlId, List<ValidationError> errors)
        {
            _logger.LogInformation($"Writing {errors.Count} errors to the database.");

            if (errors == null || errors.Count == 0)
            {
                _logger.LogWarning("No errors to write.");
                return;
            }

            try
            {
                foreach (var e in errors)
                {
                    int errorRowNumber = 0;
                    if (!int.TryParse(e.ErrorRow, out errorRowNumber))
                    {
                        errorRowNumber = 0;
                    }

                    _dbContext.AppImportProblems.Add(new AppImportProblem
                    {
                        ImportControlId = importControlId,
                        ErrorColumn = e.ErrorColumn ?? "",
                        ErrorValue = e.ErrorValue ?? "",
                        ErrorRow = errorRowNumber,
                        ImportErrorId = e.ImportErrorId,
                        ErrorDetail = e.ErrorDetail ?? ""
                    });
                }

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Errors successfully written to AppImportProblems.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save AppImportProblems.");
                throw;
            }
        }
        private async Task UpdateImportControlStatusAsync(int importControlId, bool hasErrors)
        {
            var ic = await _dbContext.AppImportControls.FindAsync(importControlId);
            if (ic != null)
            {
                ic.ImportStatusId = hasErrors ? 2 : 3;
                await _dbContext.SaveChangesAsync();
            }
            _logger.LogInformation($"Updating status for ImportControlId={importControlId}, HasErrors={hasErrors}");

        }
    }

    public class ValidationError
    {
        public string ErrorRow { get; set; }
        public string ErrorColumn { get; set; }
        public string ErrorValue { get; set; }
        public string ErrorDetail { get; set; }
        public int ImportErrorId { get; set; }
        public int? ErrorTableId { get; set; }
    }
}
