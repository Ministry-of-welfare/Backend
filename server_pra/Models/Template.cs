using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace server_pra.Models;

/// <summary>
/// טבלת תבניות (Templates) הכוללת פרטי תבניות למיילים/קבצי PDF
/// </summary>
public partial class Template
{
    /// <summary>
    /// מזהה תבנית
    /// </summary>
    [Key]
    public int TemplateId { get; set; }

    /// <summary>
    /// שם תבנית
    /// </summary>
    [Required]
    [StringLength(255)]
    public string TemplateName { get; set; }

    /// <summary>
    /// מזהה סביבה (קישור ל- Environments)
    /// </summary>
    public int EnvironmentId { get; set; }

    /// <summary>
    /// מזהה שירות (PDF או MAIL, קישור ל- Services)
    /// </summary>
    public int ServiceId { get; set; }

    /// <summary>
    /// מזהה מערכת (קישור ל- Systems)
    /// </summary>
    public int SystemId { get; set; }

    /// <summary>
    /// מזהה סטטוס (DRAFT, ACTIVE, DEPRECATED, DELETED, קישור ל- TemplateStatuses)
    /// </summary>
    public int TemplateStatusId { get; set; }

    /// <summary>
    /// נתיב מלא בקובץ S3 (bucket-name/path/to/template.html)
    /// </summary>
    [Required]
    [Column("S3Key")]
    [StringLength(1024)]
    public string S3key { get; set; }

    /// <summary>
    /// תוקף התחלה
    /// </summary>
    public DateOnly? ValidFrom { get; set; }

    /// <summary>
    /// תוקף סיום
    /// </summary>
    public DateOnly? ValidTo { get; set; }

    /// <summary>
    /// תאריך יצירה (ברירת מחדל GETDATE())
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// שם משתמש יוצר
    /// </summary>
    [Required]
    [StringLength(100)]
    public string CreatedBy { get; set; }

    /// <summary>
    /// תאריך עדכון אחרון
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// משתמש שעידכן אחרון
    /// </summary>
    [StringLength(100)]
    public string UpdatedBy { get; set; }

    /// <summary>
    /// האם פעיל (1 = פעיל, 0 = לא פעיל)
    /// </summary>
    public bool IsActive { get; set; }

    [ForeignKey("EnvironmentId")]
    [InverseProperty("Templates")]
    public virtual Environment Environment { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("Templates")]
    public virtual Service Service { get; set; }

    [ForeignKey("SystemId")]
    [InverseProperty("Templates")]
    public virtual System System { get; set; }

    [InverseProperty("Template")]
    public virtual ICollection<TemplateAuditLog> TemplateAuditLogs { get; set; } = new List<TemplateAuditLog>();

    [InverseProperty("Template")]
    public virtual ICollection<TemplatePermission> TemplatePermissions { get; set; } = new List<TemplatePermission>();

    [ForeignKey("TemplateStatusId")]
    [InverseProperty("Templates")]
    public virtual TemplateStatus TemplateStatus { get; set; }
}
