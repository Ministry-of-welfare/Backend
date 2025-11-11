using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace server_pra.Dal.Models.ScaffoldEntities;

/// <summary>
/// טבלת תפקידים במערכת (כגון מנהל, עובד וכו׳)
/// </summary>
public partial class TabRole
{
    [Key]
    /// <summary>
    /// מזהה תפקיד ייחודי
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// שם התפקיד (ייחודי)
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// תיאור קצר של התפקיד
    /// </summary>
    public string RoleDesc { get; set; }

    /// <summary>
    /// מסמן אם מדובר בתפקיד מערכת (כמו Admin)
    /// </summary>
    public bool IsSystemRole { get; set; }

    /// <summary>
    /// תאריך יצירת התפקיד
    /// </summary>
    public DateTime CreatedDate { get; set; }

    public virtual ICollection<TabRolePermission> TabRolePermissions { get; set; } = new List<TabRolePermission>();

    public virtual ICollection<TabUserRole> TabUserRoles { get; set; } = new List<TabUserRole>();
}
