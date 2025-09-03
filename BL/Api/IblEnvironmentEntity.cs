
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Api
{
    public interface IblEnvironmentEntity
    {
        Task<List<Bl�> GetAll();
        Task create(BlCustomer customer);
        Task DeleteById(String id);
        Task<BlCustomer> getCustomerById(String id);

        Task update(BlCustomer customer);
    }
}
