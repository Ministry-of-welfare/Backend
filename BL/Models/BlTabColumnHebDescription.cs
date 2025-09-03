namespace BL.Models
{
    public class BlTabColumnHebDescription
    {
        public int ColumnHebDescriptionId { get; set; }
        public int ImportDataSourceId { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string? ColumnDescription { get; set; }
    }
}
