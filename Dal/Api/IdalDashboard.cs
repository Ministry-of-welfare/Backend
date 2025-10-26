using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Api
{
    public interface IdalDashboard
    {
        // Get top errors with filters: status, data source, system, start date, end date
        Task<List<TopErrorDto>> GetTopErrors(int? statusId = null, int? importDataSourceId = null, 
            int? systemId = null, DateTime? startDate = null, DateTime? endDate = null);
    }
}
