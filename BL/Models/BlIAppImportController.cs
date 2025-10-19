using Dal.Models;

namespace BL.Models
{
    public class BlAppImportControl
    {
        public int ImportControlId { get; set; }
        public int ImportDataSourceId { get; set; }
        public DateTime ImportStartDate { get; set; }
        public DateTime? ImportFinishDate { get; set; }
        public int? TotalRows { get; set; }
        public int? TotalRowsAffected { get; set; }
        public int? RowsInvalid { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? ErrorReportPath { get; set; }



        public DateTime ImportFromDate { get; set; }

        /// <summary>
        /// עד תאריך — עד מתי הקליטה בתוקף
        /// </summary>
        public DateTime? ImportToDate { get; set; }

        /// <summary>
        /// סך רשומות שהגיעו בקובץ
        /// </summary>

        public int ImportStatusId { get; set; }

        /// <summary>
        /// כתובת (URL/נתיב) לקובץ לאחר עיבוד
        /// </summary>
        public string UrlFileAfterProcess { get; set; }

        public string EmailSento { get; set; }

        public virtual ICollection<AppImportProblem> AppImportProblems { get; set; } = new List<AppImportProblem>();

        public virtual TabImportDataSource ImportDataSource { get; set; }

        public virtual TImportStatus ImportStatus { get; set; }
    }
}

