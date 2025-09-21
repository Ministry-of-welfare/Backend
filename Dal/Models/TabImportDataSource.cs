using System;
using System.Collections.Generic;

namespace Dal.Models;

/// <summary>
/// טבלת סוגי קליטה
/// </summary>
public partial class TabImportDataSource
{
    /// <summary>
    /// מס’ רץ סוג קליטה
    /// </summary>
    public int ImportDataSourceId { get; set; }

    /// <summary>
    /// תיאור סוג קליטה
    /// </summary>
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
    public string JobName { get; set; }

    /// <summary>
    /// שם טבלה שמקושרת לתהליך
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// נתיב בו קיימים הקבצים
    /// </summary>
    public string UrlFile { get; set; }

    /// <summary>
    /// נתיב להעברת הקבצים בעת קליטה (מתועד ב־ImportControl)
    /// </summary>
    public string UrlFileAfterProcess { get; set; }

    /// <summary>
    /// תאריך סיום של תהליך שכבר לא פעיל
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// מייל לשגיאות, אפשרי מספר נמענים
    /// </summary>
    public string ErrorRecipients { get; set; }

    /// <summary>
    /// תאריך הכנסת השורה
    /// </summary>
    public DateTime InsertDate { get; set; }

    /// <summary>
    /// תאריך תחילת ההרצה
    /// </summary>
    public DateTime? StartDate { get; set; }

    public virtual ICollection<AppImportControl> AppImportControls { get; set; } = new List<AppImportControl>();

    public virtual DataSourceType DataSourceType { get; set; }

    public virtual ICollection<TabColumnHebDescription> TabColumnHebDescriptions { get; set; } = new List<TabColumnHebDescription>();

    public virtual ICollection<TabImportDataSourceColumn> TabImportDataSourceColumns { get; set; } = new List<TabImportDataSourceColumn>();

    public virtual ICollection<TabImportError> TabImportErrors { get; set; } = new List<TabImportError>();
}
