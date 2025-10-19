using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת עמודות לכל סוג קובץ
/// </summary>
public partial class TabImportDataSourceColumn
{
    /// <summary>
    /// קוד עמודה
    /// </summary>
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
    public string ColumnName { get; set; }

    /// <summary>
    /// פורמט שדה (FK → TAB_FormatColumn)
    /// </summary>
    public int? FormatColumnId { get; set; }

    /// <summary>
    /// שם עמודה בעברית עבור קובץ השגיאות
    /// </summary>
    public string ColumnNameHebDescription { get; set; }

    public virtual TabFormatColumn FormatColumn { get; set; }

    public virtual TabImportDataSource ImportDataSource { get; set; }
}
