using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Api
{


    public interface ICrud<T>
    {

        Task<List<T>> GetAll();
        Task create(T item);

        Task Delete(int item);

        Task Update(T item);

    }

}
