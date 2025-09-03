using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace server_pra.Models;

/// <summary>
/// טבלת סוגי מקור הקלטה
/// </summary>
[Table("T_DataSourceType")]
public partial class TDataSourceType
{
    /// <summary>
    /// קוד מקור
    /// </summary>
    [Key]
    public int DataSourceTypeId { get; set; }

    /// <summary>
    /// תיאור מקור
    /// </summary>
    [StringLength(400)]
    public string DataSourceTypeDesc { get; set; }

    [InverseProperty("DataSourceType")]
    public virtual ICollection<TabImportDataSource> TabImportDataSources { get; set; } = new List<TabImportDataSource>();
}
