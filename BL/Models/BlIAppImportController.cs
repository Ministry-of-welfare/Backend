namespace BL.Models
{
    public class BlAppImportControl
    {
        public int ImportControlId { get; set; }
        public int ImportDataSourceId { get; set; }
        public DateTime ImportStartDate { get; set; }
        public DateTime? ImportFinishDate { get; set; }
        public int? TotalRows { get; set; }
        public int? TotalRowsAffected { get; set; }
        public int? RowsInvalid { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? ErrorReportPath { get; set; }
    }
}
