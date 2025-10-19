using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת המערכות
/// </summary>
public partial class System
{
    /// <summary>
    /// מזהה מערכת
    /// </summary>
    public int SystemId { get; set; }

    /// <summary>
    /// קוד מערכת (HRM, WELFARE, PAYMENTS)
    /// </summary>
    public string SystemCode { get; set; }

    /// <summary>
    /// שם תצוגה של המערכת
    /// </summary>
    public string SystemName { get; set; }

    /// <summary>
    /// אימייל של בעל המערכת
    /// </summary>
    public string OwnerEmail { get; set; }

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
