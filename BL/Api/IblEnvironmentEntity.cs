
namespace BL.Api
{
    public interface IblEnvironmentEntity
    {
        int EnvironmentId { get; set; }
        string EnvironmentCode { get; set; }
        string EnvironmentName { get; set; }
        string? Description { get; set; }
    }
}
