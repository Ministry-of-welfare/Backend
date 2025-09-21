using System;
using System.Collections.Generic;

namespace Dal.Models;

/// <summary>
/// טבלת שגיאות קליטה
/// </summary>
public partial class TabImportError
{
    /// <summary>
    /// מס’ רץ שגיאה
    /// </summary>
    public int ImportErrorId { get; set; }

    /// <summary>
    /// תיאור שגיאה
    /// </summary>
    public string ImportErrorDesc { get; set; }

    /// <summary>
    /// תיאור שגיאה באנגלית
    /// </summary>
    public string ImportErrorDescEng { get; set; }

    /// <summary>
    /// קוד קליטה מקושר (FK → TAB_ImportDataSource)
    /// </summary>
    public int? ImportDataSourceId { get; set; }

    public virtual ICollection<AppImportProblem> AppImportProblems { get; set; } = new List<AppImportProblem>();

    public virtual TabImportDataSource ImportDataSource { get; set; }
}
