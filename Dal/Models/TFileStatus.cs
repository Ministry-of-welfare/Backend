using System;
using System.Collections.Generic;

namespace Dal.Models;

/// <summary>
/// טבלת סטטוס קבצים
/// </summary>
public partial class TFileStatus
{
    /// <summary>
    /// קוד סטטוס (1=פעיל, 2=לא פעיל, 3=בהקמה)
    /// </summary>
    public int FileStatusId { get; set; }

    /// <summary>
    /// תיאור סטטוס
    /// </summary>
    public string FileStatusDesc { get; set; }

    public virtual ICollection<TabImportDataSource> TabImportDataSources { get; set; } = new List<TabImportDataSource>();
}
