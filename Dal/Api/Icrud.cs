using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
