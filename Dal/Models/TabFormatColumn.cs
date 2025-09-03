using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

/// <summary>
/// טבלת פורמטי עמודות
/// </summary>
[Table("TAB_FormatColumn")]
public partial class TabFormatColumn
{
    /// <summary>
    /// קוד פורמט
    /// </summary>
    [Key]
    public int FormatColumnId { get; set; }

    /// <summary>
    /// תיאור פורמט
    /// </summary>
    [StringLength(20)]
    public string FormatColumnDesc { get; set; }

    [InverseProperty("FormatColumn")]
    public virtual ICollection<TabImportDataSourceColumn> TabImportDataSourceColumns { get; set; } = new List<TabImportDataSourceColumn>();
}
