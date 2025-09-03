using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

/// <summary>
/// תיאור עמודות בעברית עבור קבצים
/// </summary>
[Table("TAB_ColumnHebDescription")]
public partial class TabColumnHebDescription
{
    /// <summary>
    /// מזהה שדה
    /// </summary>
    [Key]
    public int ColumnHebDescriptionId { get; set; }

    /// <summary>
    /// מזהה קובץ (FK → TAB_ImportDataSource)
    /// </summary>
    public int ImportDataSourceId { get; set; }

    /// <summary>
    /// שם שדה
    /// </summary>
    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string ColumnName { get; set; }

    /// <summary>
    /// תאור שדה
    /// </summary>
    [StringLength(100)]
    [Unicode(false)]
    public string ColumnDescription { get; set; }
}
