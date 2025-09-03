namespace BL.Models
{
    public class BlAppImportProblem
    {
        public int ImportProblemId { get; set; }
        public string ErrorColumn { get; set; } = string.Empty;
        public string? ErrorValue { get; set; }
        public int? ErrorRow { get; set; }
        public string? ErrorDetail { get; set; }
    }
}
