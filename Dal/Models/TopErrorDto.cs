using System;

namespace Dal.Models
{
    public class TopErrorDto
    {
        public int ImportErrorId { get; set; }
        public string ErrorColumn { get; set; }
        public string ErrorValue { get; set; }
        public string ErrorDetail { get; set; }
        public int ImportControlId { get; set; }
        public int ErrorCount { get; set; }
    }
}