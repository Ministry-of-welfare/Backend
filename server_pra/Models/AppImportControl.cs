using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת בקרה על תהליך הקליטה
/// </summary>
public partial class AppImportControl
{
    /// <summary>
    /// מס’ רץ קליטה
    /// </summary>
    public int ImportControlId { get; set; }

    /// <summary>
    /// מס’ רץ סוג קליטה (TAB_ImportDataSource)
    /// </summary>
    public int ImportDataSourceId { get; set; }

    /// <summary>
    /// תאריך ושעה התחלת קליטה
    /// </summary>
    public DateTime ImportStartDate { get; set; }

    /// <summary>
    /// תאריך סיום קליטה
    /// </summary>
    public DateTime? ImportFinishDate { get; set; }

    /// <summary>
    /// מתאריך — ממתי הקליטה בתוקף
    /// </summary>
    public DateTime ImportFromDate { get; set; }

    /// <summary>
    /// עד תאריך — עד מתי הקליטה בתוקף
    /// </summary>
    public DateTime? ImportToDate { get; set; }

    /// <summary>
    /// סך רשומות שהגיעו בקובץ
    /// </summary>
    public int? TotalRows { get; set; }

    /// <summary>
    /// סך רשומות שנקלטו/עודכנו
    /// </summary>
    public int? TotalRowsAffected { get; set; }

    /// <summary>
    /// מספר שורות לא תקינות
    /// </summary>
    public int? RowsInvalid { get; set; }

    /// <summary>
    /// סטטוס קליטה (FK → T_ImportStatus)
    /// </summary>
    public int ImportStatusId { get; set; }

    /// <summary>
    /// כתובת (URL/נתיב) לקובץ לאחר עיבוד
    /// </summary>
    public string UrlFileAfterProcess { get; set; }

    /// <summary>
    /// שם קובץ שנקלט בפועל
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// נתיב קובץ האקסל של השגיאות שנשלח
    /// </summary>
    public string ErrorReportPath { get; set; }

    /// <summary>
    /// נמעני מייל
    /// </summary>
    public string EmailSento { get; set; }

    public virtual ICollection<AppImportProblem> AppImportProblems { get; set; } = new List<AppImportProblem>();

    public virtual TabImportDataSource ImportDataSource { get; set; }

    public virtual TImportStatus ImportStatus { get; set; }
}
