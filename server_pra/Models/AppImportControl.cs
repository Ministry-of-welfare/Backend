using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace server_pra.Models;

/// <summary>
/// טבלת בקרה על תהליך הקליטה
/// </summary>
[Table("APP_ImportControl")]
public partial class AppImportControl
{
    /// <summary>
    /// מס’ רץ קליטה
    /// </summary>
    [Key]
    public int ImportControlId { get; set; }

    /// <summary>
    /// מס’ רץ סוג קליטה (TAB_ImportDataSource)
    /// </summary>
    public int ImportDataSourceId { get; set; }

    /// <summary>
    /// תאריך ושעה התחלת קליטה
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime ImportStartDate { get; set; }

    /// <summary>
    /// תאריך סיום קליטה
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? ImportFinishDate { get; set; }

    /// <summary>
    /// מתאריך — ממתי הקליטה בתוקף
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime ImportFromDate { get; set; }

    /// <summary>
    /// עד תאריך — עד מתי הקליטה בתוקף
    /// </summary>
    [Column(TypeName = "datetime")]
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
    [Required]
    public string UrlFileAfterProcess { get; set; }

    /// <summary>
    /// שם קובץ שנקלט בפועל
    /// </summary>
    [StringLength(260)]
    public string FileName { get; set; }

    /// <summary>
    /// נתיב קובץ האקסל של השגיאות שנשלח
    /// </summary>
    [StringLength(400)]
    public string ErrorReportPath { get; set; }

    /// <summary>
    /// נמעני מייל
    /// </summary>
    [StringLength(1000)]
    public string EmailSento { get; set; }

    [InverseProperty("ImportControl")]
    public virtual ICollection<AppImportProblem> AppImportProblems { get; set; } = new List<AppImportProblem>();

    [ForeignKey("ImportStatusId")]
    [InverseProperty("AppImportControls")]
    public virtual TImportStatus ImportStatus { get; set; }
}
