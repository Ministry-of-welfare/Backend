using System;
using System.Collections.Generic;

namespace Dal.Models;

/// <summary>
/// שיוך בין תפקידים להרשאות (קשר N:N)
/// </summary>
public partial class TabRolePermission
{
    /// <summary>
    /// מזהה ייחודי לשורה
    /// </summary>
    public int RolePermissionId { get; set; }

    /// <summary>
    /// מזהה התפקיד
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// מזהה ההרשאה
    /// </summary>
    public int PermissionId { get; set; }

    public virtual TabPermission Permission { get; set; }

    public virtual TabRole Role { get; set; }
}
