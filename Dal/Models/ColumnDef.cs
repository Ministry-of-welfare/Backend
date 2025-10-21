namespace Dal.Models
{
    public class ColumnDef
    {
        public ColumnDef(string columnName, string dataType, string columnNameHeb = "")
        {
            ColumnName = columnName;
            DataType = dataType;
            ColumnNameHeb = columnNameHeb;
        }

        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string ColumnNameHeb { get; set; }
    }
}
