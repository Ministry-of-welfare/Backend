using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

/// <summary>
/// טבלת סוגי קליטה
/// </summary>
[Table("TAB_ImportDataSource")]
public partial class TabImportDataSource
{
    /// <summary>
    /// מס’ רץ סוג קליטה
    /// </summary>
    [Key]
    public int ImportDataSourceId { get; set; }

    /// <summary>
    /// תיאור סוג קליטה
    /// </summary>
    [StringLength(200)]
    public string ImportDataSourceDesc { get; set; }

    /// <summary>
    /// סוג קליטה (מקושר ל-T_DataSourceType)
    /// </summary>
    public int DataSourceTypeId { get; set; }

    /// <summary>
    /// קוד מערכת (קשור ל־T_SystemType)
    /// </summary>
    public int? SystemId { get; set; }

    /// <summary>
    /// שם הג&apos;וב שמפעיל את טבלת ImportControl
    /// </summary>
    [StringLength(400)]
    public string JobName { get; set; }

    /// <summary>
    /// שם טבלה שמקושרת לתהליך
    /// </summary>
    [StringLength(100)]
    public string TableName { get; set; }

    /// <summary>
    /// נתיב בו קיימים הקבצים
    /// </summary>
    [Required]
    public string UrlFile { get; set; }

    /// <summary>
    /// נתיב להעברת הקבצים בעת קליטה (מתועד ב־ImportControl)
    /// </summary>
    [Required]
    public string UrlFileAfterProcess { get; set; }

    /// <summary>
    /// תאריך סיום של תהליך שכבר לא פעיל
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// מייל לשגיאות, אפשרי מספר נמענים
    /// </summary>
    public string ErrorRecipients { get; set; }

    /// <summary>
    /// תאריך הכנסת השורה
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime InsertDate { get; set; }

    /// <summary>
    /// תאריך תחילת ההרצה
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [ForeignKey("DataSourceTypeId")]
    [InverseProperty("TabImportDataSources")]
    public virtual TDataSourceType DataSourceType { get; set; }

    [InverseProperty("ImportDataSource")]
    public virtual ICollection<TabImportError> TabImportErrors { get; set; } = new List<TabImportError>();
}
