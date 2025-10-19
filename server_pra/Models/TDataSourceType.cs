using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת סוגי מקור הקלטה
/// </summary>
public partial class TDataSourceType
{
    /// <summary>
    /// קוד מקור
    /// </summary>
    public int DataSourceTypeId { get; set; }

    /// <summary>
    /// תיאור מקור
    /// </summary>
    public string DataSourceTypeDesc { get; set; }

    public virtual ICollection<TabImportDataSource> TabImportDataSources { get; set; } = new List<TabImportDataSource>();
}
