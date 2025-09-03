using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace server_pra.Models;

/// <summary>
/// טבלת ServiceBucket – מכילה את ה-Buckets ב-S3 המשויכים לשירותים
/// </summary>
[Table("ServiceBucket")]
public partial class ServiceBucket
{
    /// <summary>
    /// מזהה שירות (קישור לטבלת Services)
    /// </summary>
    [Key]
    public int ServiceId { get; set; }

    /// <summary>
    /// שם ה-S3 Bucket המשויך לשירות (לדוגמה: pdf-templates-bucket)
    /// </summary>
    [Required]
    [StringLength(255)]
    public string BucketName { get; set; }

    /// <summary>
    /// אזור (Region) של ה-Bucket (לדוגמה: us-east-1)
    /// </summary>
    [StringLength(50)]
    public string BucketRegion { get; set; }

    /// <summary>
    /// האם ה-Bucket פעיל (1 = מותר, 0 = אסור)
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// תיאור שימוש או הערות
    /// </summary>
    [StringLength(255)]
    public string Description { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("ServiceBucket")]
    public virtual Service Service { get; set; }
}
