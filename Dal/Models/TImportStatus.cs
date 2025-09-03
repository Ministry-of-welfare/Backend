using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

/// <summary>
/// טבלת סטטוסי קליטה
/// </summary>
[Table("T_ImportStatus")]
public partial class TImportStatus
{
    /// <summary>
    /// קוד סטטוס קליטה
    /// </summary>
    [Key]
    public int ImportStatusId { get; set; }

    /// <summary>
    /// תיאור סטטוס קליטה
    /// </summary>
    [StringLength(400)]
    public string ImportStatusDesc { get; set; }

    [InverseProperty("ImportStatus")]
    public virtual ICollection<AppImportControl> AppImportControls { get; set; } = new List<AppImportControl>();
}
