using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace server_pra.Models;

/// <summary>
/// טבלת שגיאות קליטה
/// </summary>
[Table("TAB_ImportError")]
public partial class TabImportError
{
    /// <summary>
    /// מס’ רץ שגיאה
    /// </summary>
    [Key]
    public int ImportErrorId { get; set; }

    /// <summary>
    /// תיאור שגיאה
    /// </summary>
    [StringLength(400)]
    public string ImportErrorDesc { get; set; }

    /// <summary>
    /// תיאור שגיאה באנגלית
    /// </summary>
    public string ImportErrorDescEng { get; set; }

    /// <summary>
    /// קוד קליטה מקושר (FK → TAB_ImportDataSource)
    /// </summary>
    public int? ImportDataSourceId { get; set; }

    [ForeignKey("ImportDataSourceId")]
    [InverseProperty("TabImportErrors")]
    public virtual TabImportDataSource ImportDataSource { get; set; }
}
