-- בדיקת הרשאות משתמש
SELECT 
    u.UserName,
    u.DisplayName,
    r.RoleName,
    p.PermissionName,
    p.ModuleName
FROM auth.TAB_User u
JOIN auth.TAB_UserRole ur ON u.UserId = ur.UserId
JOIN auth.TAB_Role r ON ur.RoleId = r.RoleId
JOIN auth.TAB_RolePermission rp ON r.RoleId = rp.RoleId
JOIN auth.TAB_Permission p ON rp.PermissionId = p.PermissionId
WHERE u.UserName = 'admin'  -- החלף עם שם המשתמש שרוצה לבדוק
AND u.IsActive = 1
AND (ur.FromDate IS NULL OR ur.FromDate <= CAST(GETDATE() AS DATE))
AND (ur.ToDate IS NULL OR ur.ToDate >= CAST(GETDATE() AS DATE))
ORDER BY r.RoleName, p.ModuleName, p.PermissionName;