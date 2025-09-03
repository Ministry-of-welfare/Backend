namespace BL.Models
{
    public class BlTabImportDataSource
    {
        public int ImportDataSourceId { get; set; }
        public string ImportDataSourceDesc { get; set; } = string.Empty;
        public int DataSourceTypeId { get; set; }
        public int? SystemId { get; set; }
        public string? JobName { get; set; }
        public string? TableName { get; set; }
        public string UrlFile { get; set; } = string.Empty;
        public string UrlFileAfterProcess { get; set; } = string.Empty;
        public DateTime? EndDate { get; set; }
        public string? ErrorRecipients { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
