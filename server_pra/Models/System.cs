using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace server_pra.Models;

/// <summary>
/// טבלת המערכות
/// </summary>
public partial class System
{
    /// <summary>
    /// מזהה מערכת
    /// </summary>
    [Key]
    public int SystemId { get; set; }

    /// <summary>
    /// קוד מערכת (HRM, WELFARE, PAYMENTS)
    /// </summary>
    [Required]
    [StringLength(50)]
    public string SystemCode { get; set; }

    /// <summary>
    /// שם תצוגה של המערכת
    /// </summary>
    [Required]
    [StringLength(255)]
    public string SystemName { get; set; }

    /// <summary>
    /// אימייל של בעל המערכת
    /// </summary>
    [StringLength(100)]
    public string OwnerEmail { get; set; }

    [InverseProperty("System")]
    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
