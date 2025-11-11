using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace server_pra.Dal.Models.ScaffoldEntities;

/// <summary>
/// טבלת משתמשים במערכת ההרשאות
/// </summary>
public partial class TabUser
{
    [Key]
    /// <summary>
    /// מזהה משתמש ייחודי
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// שם משתמש לכניסה למערכת (ייחודי)
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// שם פרטי של המשתמש
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// שם משפחה של המשתמש
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// שם לתצוגה (מלא או מותאם)
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// כתובת מייל של המשתמש
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// האם המשתמש פעיל (1=כן, 0=לא)
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// תאריך יצירת המשתמש במערכת
    /// </summary>
    public DateTime CreatedDate { get; set; }

    public virtual ICollection<TabUserRole> TabUserRoles { get; set; } = new List<TabUserRole>();
}
