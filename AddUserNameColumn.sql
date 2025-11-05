-- סקריפט להוספת עמודת UserName לטבלת SerilogLogs
USE Logs;
GO

-- בדיקה אם העמודה כבר קיימת
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'SerilogLogs' 
    AND COLUMN_NAME = 'UserName'
    AND TABLE_SCHEMA = 'dbo'
)
BEGIN
    -- הוספת העמודה
    ALTER TABLE dbo.SerilogLogs 
    ADD UserName NVARCHAR(255) NULL;
    
    PRINT 'עמודת UserName נוספה בהצלחה לטבלת SerilogLogs';
END
ELSE
BEGIN
    PRINT 'עמודת UserName כבר קיימת בטבלת SerilogLogs';
END
GO

-- אופציונלי: יצירת אינדקס על העמודה החדשה לביצועים טובים יותר
IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'IX_SerilogLogs_UserName' 
    AND object_id = OBJECT_ID('dbo.SerilogLogs')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_SerilogLogs_UserName 
    ON dbo.SerilogLogs (UserName);
    
    PRINT 'אינדקס על עמודת UserName נוצר בהצלחה';
END
GO