namespace Dal.Models
{
	public class EnvironmentEntity
	{
		public int EnvironmentId { get; set; }   // מפתח ראשי
		public string EnvironmentCode { get; set; }
		public string EnvironmentName { get; set; }
		public string Description { get; set; }
	}
}
