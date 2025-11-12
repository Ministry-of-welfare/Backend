-- סקירה של כל המשתמשים והתפקידים שלהם
SELECT 
    u.UserName,
    u.DisplayName,
    STRING_AGG(r.RoleName, ', ') AS Roles,
    COUNT(DISTINCT p.PermissionId) AS PermissionCount
FROM auth.TAB_User u
LEFT JOIN auth.TAB_UserRole ur ON u.UserId = ur.UserId
LEFT JOIN auth.TAB_Role r ON ur.RoleId = r.RoleId
LEFT JOIN auth.TAB_RolePermission rp ON r.RoleId = rp.RoleId
LEFT JOIN auth.TAB_Permission p ON rp.PermissionId = p.PermissionId
WHERE u.IsActive = 1
GROUP BY u.UserId, u.UserName, u.DisplayName
ORDER BY u.UserName;