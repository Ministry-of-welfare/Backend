using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת סטטוסי קליטה
/// </summary>
public partial class TImportStatus
{
    /// <summary>
    /// קוד סטטוס קליטה
    /// </summary>
    public int ImportStatusId { get; set; }

    /// <summary>
    /// תיאור סטטוס קליטה
    /// </summary>
    public string ImportStatusDesc { get; set; }

    public virtual ICollection<AppImportControl> AppImportControls { get; set; } = new List<AppImportControl>();
}
