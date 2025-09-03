using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

/// <summary>
/// טבלת השירותים (כגון PDF, MAIL)
/// </summary>
public partial class Service
{
    /// <summary>
    /// מזהה שירות (1, 2)
    /// </summary>
    [Key]
    public int ServiceId { get; set; }

    /// <summary>
    /// שם השירות (PDF, MAIL)
    /// </summary>
    [Required]
    [StringLength(50)]
    public string ServiceName { get; set; }

    /// <summary>
    /// תיאור השירות
    /// </summary>
    [StringLength(255)]
    public string Description { get; set; }

    [InverseProperty("Service")]
    public virtual ServiceBucket ServiceBucket { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
