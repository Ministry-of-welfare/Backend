using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

/// <summary>
/// טבלת בעיות/שגיאות בקליטה
/// </summary>
[Table("APP_ImportProblem")]
public partial class AppImportProblem
{
    /// <summary>
    /// מס’ רץ טבלת שגיאות
    /// </summary>
    [Key]
    public int ImportProblemId { get; set; }

    /// <summary>
    /// מס’ רץ קליטה (FK → APP_ImportControl)
    /// </summary>
    public int? ImportControlId { get; set; }

    /// <summary>
    /// העמודה בה קיימת השגיאה
    /// </summary>
    public string ErrorColumn { get; set; }

    /// <summary>
    /// הערך עבורו התקבלה השגיאה
    /// </summary>
    public string ErrorValue { get; set; }

    /// <summary>
    /// מספר השורה עם השגיאה
    /// </summary>
    public int? ErrorRow { get; set; }

    /// <summary>
    /// ID רשומת היעד
    /// </summary>
    public int? ErrorTableId { get; set; }

    /// <summary>
    /// מס’ רץ שגיאה (FK → TAB_ImportError)
    /// </summary>
    public int? ImportErrorId { get; set; }

    /// <summary>
    /// מלל חופשי של השגיאה
    /// </summary>
    public string ErrorDetail { get; set; }

    [ForeignKey("ImportControlId")]
    [InverseProperty("AppImportProblems")]
    public virtual AppImportControl ImportControl { get; set; }
}
