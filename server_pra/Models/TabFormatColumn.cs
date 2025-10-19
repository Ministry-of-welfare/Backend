using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת פורמטי עמודות
/// </summary>
public partial class TabFormatColumn
{
    /// <summary>
    /// קוד פורמט
    /// </summary>
    public int FormatColumnId { get; set; }

    /// <summary>
    /// תיאור פורמט
    /// </summary>
    public string FormatColumnDesc { get; set; }

    public virtual ICollection<TabImportDataSourceColumn> TabImportDataSourceColumns { get; set; } = new List<TabImportDataSourceColumn>();
}
