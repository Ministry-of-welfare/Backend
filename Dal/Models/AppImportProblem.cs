using System;

namespace Dal.Models
{
    public partial class AppImportProblem
    {
        public int ImportProblemId { get; set; }
        public int? ImportControlId { get; set; }
        public string ErrorColumn { get; set; }
        public string ErrorValue { get; set; }
        public int? ErrorRow { get; set; }
        public int? ErrorTableId { get; set; }
        public int? ImportErrorId { get; set; }
        public string ErrorDetail { get; set; }

        // Navigation properties
        public virtual AppImportControl ImportControl { get; set; }
        public virtual TabImportError ImportError { get; set; }
    }
}