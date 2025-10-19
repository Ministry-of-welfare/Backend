using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// תיאור עמודות בעברית עבור קבצים
/// </summary>
public partial class TabColumnHebDescription
{
    /// <summary>
    /// מזהה שדה
    /// </summary>
    public int ColumnHebDescriptionId { get; set; }

    /// <summary>
    /// מזהה קובץ (FK → TAB_ImportDataSource)
    /// </summary>
    public int ImportDataSourceId { get; set; }

    /// <summary>
    /// שם שדה
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// תאור שדה
    /// </summary>
    public string ColumnDescription { get; set; }

    public virtual TabImportDataSource ImportDataSource { get; set; }
}
