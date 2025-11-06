using System;
using System.Collections.Generic;

namespace Dal.Models
{
    public partial class TabImportDataSource
    {
        public int ImportDataSourceId { get; set; }
        public string ImportDataSourceDesc { get; set; }
        public int? FileStatusId { get; set; }
        public int DataSourceTypeId { get; set; }
        public int? SystemId { get; set; }
        public string JobName { get; set; }
        public string TableName { get; set; }
        public string UrlFile { get; set; }
        public string UrlFileAfterProcess { get; set; }
        public DateTime? EndDate { get; set; }
        public string ErrorRecipients { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? StartDate { get; set; }

        // Navigation properties (collections initialized)
        public virtual System System { get; set; }
        public virtual TDataSourceType DataSourceType { get; set; }
        public virtual TFileStatus FileStatus { get; set; }
        public virtual ICollection<TabValidationRule> TabValidationRules { get; set; } = new HashSet<TabValidationRule>();
        public virtual ICollection<AppImportControl> AppImportControls { get; set; } = new List<AppImportControl>();
        public virtual ICollection<TabColumnHebDescription> TabColumnHebDescriptions { get; set; } = new List<TabColumnHebDescription>();
        public virtual ICollection<TabImportDataSourceColumn> TabImportDataSourceColumns { get; set; } = new List<TabImportDataSourceColumn>();
        public virtual ICollection<TabImportError> TabImportErrors { get; set; } = new List<TabImportError>();
    }
}