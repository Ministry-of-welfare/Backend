using System;
using System.Collections.Generic;


namespace Dal.Models
{
    public partial class AppImportControl
    {
        public int ImportControlId { get; set; }
        public int ImportDataSourceId { get; set; }
        public DateTime ImportStartDate { get; set; }
        public DateTime? ImportFinishDate { get; set; }
        public DateTime ImportFromDate { get; set; }
        public DateTime? ImportToDate { get; set; }
        public int? TotalRows { get; set; }
        public int? TotalRowsAffected { get; set; }
        public int? RowsInvalid { get; set; }
        public int ImportStatusId { get; set; }
        public string UrlFileAfterProcess { get; set; }
        public string FileName { get; set; }
        public string ErrorReportPath { get; set; }
        public string EmailSento { get; set; }

        // Navigation properties
        public virtual ICollection<AppImportProblem> AppImportProblems { get; set; } = new List<AppImportProblem>();

        public virtual TabImportDataSource ImportDataSource { get; set; }
        public virtual TImportStatus ImportStatus { get; set; }
    }
}