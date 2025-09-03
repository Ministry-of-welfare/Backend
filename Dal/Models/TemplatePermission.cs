using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

/// <summary>
/// טבלת הרשאות עבור תבניות (TemplatePermissions)
/// </summary>
public partial class TemplatePermission
{
    /// <summary>
    /// מזהה הרשאה
    /// </summary>
    [Key]
    public int PermissionId { get; set; }

    /// <summary>
    /// מזהה תבנית (קישור לטבלת Templates)
    /// </summary>
    public int TemplateId { get; set; }

    /// <summary>
    /// סוג ישות (User או Group)
    /// </summary>
    [Required]
    [StringLength(20)]
    public string PrincipalType { get; set; }

    /// <summary>
    /// שם משתמש או קבוצה (user@domain.gov.il או Developers)
    /// </summary>
    [Required]
    [StringLength(100)]
    public string PrincipalName { get; set; }

    /// <summary>
    /// הרשאת צפייה (1 = מותר, 0 = אסור)
    /// </summary>
    public bool CanView { get; set; }

    /// <summary>
    /// הרשאת עריכה (1 = מותר, 0 = אסור)
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// הרשאת מחיקה (1 = מותר, 0 = אסור)
    /// </summary>
    public bool CanDelete { get; set; }

    /// <summary>
    /// הרשאת שכפול (1 = מותר, 0 = אסור)
    /// </summary>
    public bool CanDuplicate { get; set; }

    [ForeignKey("TemplateId")]
    [InverseProperty("TemplatePermissions")]
    public virtual Template Template { get; set; }
}
