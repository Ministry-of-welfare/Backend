namespace Dal.Models
{
    public class ColumnDef
    {
        public ColumnDef(string columnName, string dataType)
        {
            ColumnName = columnName;
            DataType = dataType;
        }

        public string ColumnName { get; set; }
        public string DataType { get; set; }
    }
}
