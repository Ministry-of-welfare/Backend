using System;

namespace BL.Models
{
    public class BlEnvironmentEntity
    {
        public int EnvironmentId { get; set; }
        public string EnvironmentCode { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = string.Empty;
        public string? Description { get; set; } = null;
    }
}
