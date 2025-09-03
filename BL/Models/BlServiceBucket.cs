namespace BL.Models
{
    public class BlServiceBucket
    {
        public int ServiceId { get; set; }
        public string BucketName { get; set; } = string.Empty;
        public string? BucketRegion { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}
