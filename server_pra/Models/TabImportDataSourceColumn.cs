using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace server_pra.Models;

/// <summary>
/// טבלת עמודות לכל סוג קובץ
/// </summary>
[Table("TAB_ImportDataSourceColumns")]
public partial class TabImportDataSourceColumn
{
    /// <summary>
    /// קוד עמודה
    /// </summary>
    [Key]
    public int ImportDataSourceColumnsId { get; set; }

    /// <summary>
    /// קוד קובץ (FK → TAB_ImportDataSource)
    /// </summary>
    public int ImportDataSourceId { get; set; }

    /// <summary>
    /// סידורי
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// שם עמודה
    /// </summary>
    [Required]
    [StringLength(200)]
    [Unicode(false)]
    public string ColumnName { get; set; }

    /// <summary>
    /// פורמט שדה (FK → TAB_FormatColumn)
    /// </summary>
    public int? FormatColumnId { get; set; }

    /// <summary>
    /// שם עמודה בעברית עבור קובץ השגיאות
    /// </summary>
    [StringLength(200)]
    [Unicode(false)]
    public string ColumnNameHebDescription { get; set; }

    [ForeignKey("FormatColumnId")]
    [InverseProperty("TabImportDataSourceColumns")]
    public virtual TabFormatColumn FormatColumn { get; set; }
}
