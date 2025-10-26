namespace BL.Models
{
    /// <summary>
    /// מחלקה המייצגת את מקור הקליטה.
    /// </summary>
    public class BlTabImportDataSource
    {
        public int ImportDataSourceId { get; set; } // מזהה מקור קליטה
        public string ImportDataSourceDesc { get; set; } = string.Empty; // תיאור מקור קליטה
        public int DataSourceTypeId { get; set; } // סוג מקור קליטה
        public int? FileStatusId { get; set; }
        public int? SystemId { get; set; } // מזהה מערכת
        public string? JobName { get; set; } // שם משימה
        public string? TableName { get; set; } // שם טבלה
        public string UrlFile { get; set; } = string.Empty; // נתיב קובץ
        public string UrlFileAfterProcess { get; set; } = string.Empty; // נתיב קובץ לאחר עיבוד
        public DateTime? EndDate { get; set; } // תאריך סיום
        public string? ErrorRecipients { get; set; } // נמענים לשגיאות
        public DateTime InsertDate { get; set; } // תאריך יצירה
        public DateTime? StartDate { get; set; } // תאריך התחלה

    }

    /// <summary>
    /// מחלקה המייצגת את תוצאות החיפוש עבור מקור הקליטה.
    /// </summary>
    public class BlTabImportDataSourceForQuery
    {
        public int ImportControlId { get; set; } // מזהה קליטה
        public string ImportDataSourceDesc { get; set; } = string.Empty; // תיאור מקור קליטה
        public string SystemName { get; set; } = string.Empty; // שם מערכת
        public string FileName { get; set; } = string.Empty; // שם קובץ
        public DateTime ImportStartDate { get; set; } // תאריך התחלת קליטה
        public DateTime ImportFinishDate { get; set; } // תאריך סיום קליטה
        public int TotalRows { get; set; } // סך כל השורות בקובץ
        public int TotalRowsAffected { get; set; } // סך השורות שנקלטו
        public int RowsInvalid { get; set; } // סך השורות הפגומות
        public string ImportStatusDesc { get; set; } = string.Empty;// מזהה סטטוס קליטה
        public string UrlFileAfterProcess { get; set; } = string.Empty; // נתיב קובץ לאחר עיבוד
        public string ErrorReportPath { get; set; } = string.Empty; // נתיב לדוח שגיאות
    }

}
