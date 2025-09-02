using BL.Api;

namespace BL
{
    public class EnvironmentEntity : IblEnvironmentEntity
    {
        public int EnvironmentId { get; set; }
        public string EnvironmentCode { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
