using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Api
{
    // DTO: מזהה סטטוס, תיאור סטטוס, וכמות רשומות (count)
    public record StatusCountDto(int ImportStatusId, string ImportStatusDesc, int Count);

    public interface IdalDashboard
    {
        Task<List<StatusCountDto>> GetStatusCountsAsync();
    }
}