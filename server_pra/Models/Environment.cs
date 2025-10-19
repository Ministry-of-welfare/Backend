using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת הסביבות (פיתוח, בדיקות, ייצור וכו׳)
/// </summary>
public partial class Environment
{
    /// <summary>
    /// מזהה סביבה (1, 2, 3...)
    /// </summary>
    public int EnvironmentId { get; set; }

    /// <summary>
    /// קוד סביבה (DEV, TEST, PREPROD, PROD)
    /// </summary>
    public string EnvironmentCode { get; set; }

    /// <summary>
    /// שם תצוגה של הסביבה (סביבת פיתוח, ייצור וכו׳)
    /// </summary>
    public string EnvironmentName { get; set; }

    /// <summary>
    /// תיאור חופשי על הסביבה
    /// </summary>
    public string Description { get; set; }

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
