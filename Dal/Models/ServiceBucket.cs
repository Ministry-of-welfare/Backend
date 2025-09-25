using System;
using System.Collections.Generic;

namespace Dal.Models;

/// <summary>
/// טבלת ServiceBucket – מכילה את ה-Buckets ב-S3 המשויכים לשירותים
/// </summary>
public partial class ServiceBucket
{
    /// <summary>
    /// מזהה שירות (קישור לטבלת Services)
    /// </summary>
    public int ServiceId { get; set; }

    /// <summary>
    /// שם ה-S3 Bucket המשויך לשירות (לדוגמה: pdf-templates-bucket)
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// אזור (Region) של ה-Bucket (לדוגמה: us-east-1)
    /// </summary>
    public string BucketRegion { get; set; }

    /// <summary>
    /// האם ה-Bucket פעיל (1 = מותר, 0 = אסור)
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// תיאור שימוש או הערות
    /// </summary>
    public string Description { get; set; }

    public virtual Service Service { get; set; }
}
