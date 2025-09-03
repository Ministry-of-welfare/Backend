using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

/// <summary>
/// טבלת רישומי Audit עבור תבניות (TemplateAuditLog)
/// </summary>
[Table("TemplateAuditLog")]
public partial class TemplateAuditLog
{
    /// <summary>
    /// מזהה רישום
    /// </summary>
    [Key]
    public int LogId { get; set; }

    /// <summary>
    /// מזהה תבנית (קישור ל- Templates)
    /// </summary>
    public int TemplateId { get; set; }

    /// <summary>
    /// סוג פעולה (View, Edit, Delete, Duplicate, PermissionChange)
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Action { get; set; }

    /// <summary>
    /// המשתמש שביצע פעולה (user@domain.gov.il)
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ActionBy { get; set; }

    /// <summary>
    /// תאריך ביצוע הפעולה
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime ActionAt { get; set; }

    /// <summary>
    /// ערך קודם (JSON)
    /// </summary>
    public string OldValue { get; set; }

    /// <summary>
    /// ערך חדש (JSON)
    /// </summary>
    public string NewValue { get; set; }

    [ForeignKey("TemplateId")]
    [InverseProperty("TemplateAuditLogs")]
    public virtual Template Template { get; set; }
}
