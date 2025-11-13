-- סקריפט למילוי טבלאות מערכת ההרשאות עם בדיקות קיום

-- מילוי טבלת הרשאות
IF NOT EXISTS (SELECT 1 FROM auth.TAB_Permission WHERE PermissionName = 'View')
BEGIN
    INSERT INTO auth.TAB_Permission (PermissionName, PermissionDesc, ModuleName) VALUES
    ('View', 'צפייה בנתונים', 'General'),
    ('Create', 'יצירת רשומות חדשות', 'General'),
    ('Edit', 'עריכת רשומות קיימות', 'General'),
    ('Delete', 'מחיקת רשומות', 'General'),
    ('Approve', 'אישור פעולות', 'General'),
    ('ViewUsers', 'צפייה במשתמשים', 'UserManagement'),
    ('ManageUsers', 'ניהול משתמשים', 'UserManagement'),
    ('ViewRoles', 'צפייה בתפקידים', 'RoleManagement'),
    ('ManageRoles', 'ניהול תפקידים', 'RoleManagement'),
    ('SystemAdmin', 'ניהול מערכת מלא', 'System');
END

-- מילוי טבלת תפקידים
IF NOT EXISTS (SELECT 1 FROM auth.TAB_Role WHERE RoleName = 'Admin')
BEGIN
    INSERT INTO auth.TAB_Role(RoleName, RoleDesc, IsSystemRole, CreatedDate) VALUES
    ('Admin', 'מנהל מערכת', 1, GETDATE()),
    ('Manager', 'מנהל', 0, GETDATE()),
    ('Employee', 'עובד', 0, GETDATE()),
    ('Viewer', 'צופה בלבד', 0, GETDATE());
END

-- מילוי טבלת משתמשים
IF NOT EXISTS (SELECT 1 FROM auth.TAB_User WHERE UserName = 'admin')
    INSERT INTO auth.TAB_User(UserName, FirstName, LastName, DisplayName, Email, IsActive, CreatedDate) VALUES
    ('admin', 'מנהל', 'מערכת', 'מנהל מערכת', 'admin@company.com', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM auth.TAB_User WHERE UserName = 'manager1')
    INSERT INTO auth.TAB_User(UserName, FirstName, LastName, DisplayName, Email, IsActive, CreatedDate) VALUES
    ('manager1', 'יוסי', 'כהן', 'יוסי כהן', 'yossi@company.com', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM auth.TAB_User WHERE UserName = 'employee1')
    INSERT INTO auth.TAB_User (UserName, FirstName, LastName, DisplayName, Email, IsActive, CreatedDate) VALUES
    ('employee1', 'דנה', 'לוי', 'דנה לוי', 'dana@company.com', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM auth.TAB_User WHERE UserName = 'viewer1')
    INSERT INTO   auth.TAB_User (UserName, FirstName, LastName, DisplayName, Email, IsActive, CreatedDate) VALUES
    ('viewer1', 'משה', 'ישראל', 'משה ישראל', 'moshe@company.com', 1, GETDATE());

 --שיוך הרשאות לתפקידים
IF NOT EXISTS (SELECT 1 FROM auth.TAB_RolePermission WHERE RoleId = 1 AND PermissionId = 1)
BEGIN
    INSERT INTO auth.TAB_RolePermission (RoleId, PermissionId) VALUES
    -- Admin - כל ההרשאות
    (1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 6), (1, 7), (1, 8), (1, 9), (1, 10),
    -- Manager - הרשאות ניהול
    (2, 1), (2, 2), (2, 3), (2, 5), (2, 6), (2, 8),
    -- Employee - הרשאות בסיסיות
    (3, 1), (3, 2), (3, 3),
    -- Viewer - צפייה בלבד
    (4, 1);
END

-- שיוך משתמשים לתפקידים
DECLARE @AdminUserId INT = (SELECT UserId FROM auth.TAB_User WHERE UserName = 'admin');
DECLARE @ManagerUserId INT = (SELECT UserId FROM auth.TAB_User WHERE UserName = 'manager1');
DECLARE @EmployeeUserId INT = (SELECT UserId FROM auth.TAB_User WHERE UserName = 'employee1');
DECLARE @ViewerUserId INT = (SELECT UserId FROM auth.TAB_User WHERE UserName = 'viewer1');

IF NOT EXISTS (SELECT 1 FROM auth.TAB_UserRole WHERE UserId = @AdminUserId AND RoleId = 1)
    INSERT INTO auth.TAB_UserRole (UserId, RoleId, AssignedBy, FromDate) VALUES (@AdminUserId, 1, 'system', CAST(GETDATE() AS DATE));

--IF NOT EXISTS (SELECT 1 FROM auth.TAB_UserRole WHERE UserId = @ManagerUserId AND RoleId = 2)
--    INSERT INTO auth.TAB_UserRole (UserId, RoleId, AssignedBy, FromDate) VALUES (@ManagerUserId, 2, 'admin', CAST(GETDATE() AS DATE));

--IF NOT EXISTS (SELECT 1 FROM auth.TAB_UserRole WHERE UserId = @EmployeeUserId AND RoleId = 3)
--    INSERT INTO auth.TAB_UserRole (UserId, RoleId, AssignedBy, FromDate) VALUES (@EmployeeUserId, 3, 'admin', CAST(GETDATE() AS DATE));

--IF NOT EXISTS (SELECT 1 FROM auth.TAB_UserRole WHERE UserId = @ViewerUserId AND RoleId = 4)
--    INSERT INTO auth.TAB_UserRole (UserId, RoleId, AssignedBy, FromDate) VALUES (@ViewerUserId, 4, 'admin', CAST(GETDATE() AS DATE));