using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Models;
using Dal.Models;

namespace BL.Api
{
    public interface IBlEnvironmentEntity
    {
        Task<List<BlEnvironmentEntity>> GetAll();
        Task<BlEnvironmentEntity> GetById(int id);
        Task<BlEnvironmentEntity> Create(BlEnvironmentEntity item);
        Task<BlEnvironmentEntity> Update(BlEnvironmentEntity item);
        Task Delete(int id);

        // פונקציות המרה בין שכבות
        BlEnvironmentEntity CastingEnvironmentEntityFromDalToBl(EnvironmentEntity e);
        EnvironmentEntity CastingEnvironmentEntityFromBlToDal(BlEnvironmentEntity? e);
    }
}
