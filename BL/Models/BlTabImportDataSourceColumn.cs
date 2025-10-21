using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.Models
{
    public class BlTabImportDataSourceColumn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImportDataSourceColumnsId { get; set; }
        public int ImportDataSourceId { get; set; }
        public int OrderId { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public int? FormatColumnId { get; set; }
    }
}
