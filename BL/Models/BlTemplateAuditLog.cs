namespace BL.Models
{
    public class BlTemplateAuditLog
    {
        public int LogId { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime ActionAt { get; set; }
        public string? ActionBy { get; set; }
        public string? NewValue { get; set; }
        public string? OldValue { get; set; }
    }
}
