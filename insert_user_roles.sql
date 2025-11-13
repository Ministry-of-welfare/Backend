-- שיוך משתמשים לתפקידים - סקריפט מתוקן

-- שיוך admin לתפקיד Admin (1)
IF NOT EXISTS (
    SELECT 1 FROM auth.TAB_UserRole ur 
    JOIN auth.TAB_User u ON ur.UserId = u.UserId 
    JOIN auth.TAB_Role r ON ur.RoleId = r.RoleId
    WHERE u.UserName = 'admin' AND r.RoleName = 'Admin'
)
INSERT INTO auth.TAB_UserRole (UserId, RoleId, AssignedBy, FromDate) 
SELECT u.UserId, r.RoleId, 'system', CAST(GETDATE() AS DATE) 
FROM auth.TAB_User u, auth.TAB_Role r 
WHERE u.UserName = 'admin' AND r.RoleName = 'Admin';

-- שיוך manager1 לתפקיד Manager (2)
IF NOT EXISTS (
    SELECT 1 FROM auth.TAB_UserRole ur 
    JOIN auth.TAB_User u ON ur.UserId = u.UserId 
    JOIN auth.TAB_Role r ON ur.RoleId = r.RoleId
    WHERE u.UserName = 'manager1' AND r.RoleName = 'Manager'
)
INSERT INTO auth.TAB_UserRole (UserId, RoleId, AssignedBy, FromDate) 
SELECT u.UserId, r.RoleId, 'admin', CAST(GETDATE() AS DATE) 
FROM auth.TAB_User u, auth.TAB_Role r 
WHERE u.UserName = 'manager1' AND r.RoleName = 'Manager';

-- שיוך employee1 לתפקיד Employee (3)
IF NOT EXISTS (
    SELECT 1 FROM auth.TAB_UserRole ur 
    JOIN auth.TAB_User u ON ur.UserId = u.UserId 
    JOIN auth.TAB_Role r ON ur.RoleId = r.RoleId
    WHERE u.UserName = 'employee1' AND r.RoleName = 'Employee'
)
INSERT INTO auth.TAB_UserRole (UserId, RoleId, AssignedBy, FromDate) 
SELECT u.UserId, r.RoleId, 'admin', CAST(GETDATE() AS DATE) 
FROM auth.TAB_User u, auth.TAB_Role r 
WHERE u.UserName = 'employee1' AND r.RoleName = 'Employee';

-- שיוך viewer1 לתפקיד Viewer (4)
IF NOT EXISTS (
    SELECT 1 FROM auth.TAB_UserRole ur 
    JOIN auth.TAB_User u ON ur.UserId = u.UserId 
    JOIN auth.TAB_Role r ON ur.RoleId = r.RoleId
    WHERE u.UserName = 'viewer1' AND r.RoleName = 'Viewer'
)
INSERT INTO auth.TAB_UserRole (UserId, RoleId, AssignedBy, FromDate) 
SELECT u.UserId, r.RoleId, 'admin', CAST(GETDATE() AS DATE) 
FROM auth.TAB_User u, auth.TAB_Role r 
WHERE u.UserName = 'viewer1' AND r.RoleName = 'Viewer';