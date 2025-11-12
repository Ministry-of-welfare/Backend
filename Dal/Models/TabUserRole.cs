using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dal.Models;

/// <summary>
/// שיוך משתמשים לתפקידים (קשר N:N)
/// </summary>
public partial class TabUserRole
{
    [Key]
    /// <summary>
    /// מזהה ייחודי לשיוך
    /// </summary>
    public int UserRoleId { get; set; }

    /// <summary>
    /// מזהה המשתמש
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// מזהה התפקיד
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// שם המשתמש שהקצה את ההרשאה
    /// </summary>
    public string AssignedBy { get; set; }

    /// <summary>
    /// תאריך התחלת ההרשאה
    /// </summary>
    public DateOnly? FromDate { get; set; }

    /// <summary>
    /// תאריך סיום ההרשאה
    /// </summary>
    public DateOnly? ToDate { get; set; }

    public virtual TabRole Role { get; set; }

    public virtual TabUser User { get; set; }
}
