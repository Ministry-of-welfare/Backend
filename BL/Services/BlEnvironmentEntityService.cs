using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;

namespace BL.Services
{
    public class BlEnvironmentEntityService : IBlEnvironmentEntity
    {
        private readonly IdalEnvironment _dal;

        public BlEnvironmentEntityService(IdalEnvironment dal)
        {
            _dal = dal;
        }

        #region GetAll
        //public async Task<List<BlEnvironmentEntity>> GetAll()
        //{
        //    var data = await _dal.GetAll();
        //    return data.Select(CastingEnvironmentEntityFromDalToBl).ToList();
        //}
        #endregion

        #region GetById
        //public async Task<BlEnvironmentEntity> GetById(int id)
        //{
        //    var entity = await _dal.GetById(id);
        //    return CastingEnvironmentEntityFromDalToBl(entity);
        //}
        #endregion

        #region Create
        //public async Task<BlEnvironmentEntity> Create(BlEnvironmentEntity item)
        //{
        //    var dalEntity = CastingEnvironmentEntityFromBlToDal(item);
        //    var created = await _dal.Create(dalEntity);
        //    return CastingEnvironmentEntityFromDalToBl(created);
        //}
        #endregion

        #region Update
        //public async Task<BlEnvironmentEntity> Update(BlEnvironmentEntity item)
        //{
        //    var dalEntity = CastingEnvironmentEntityFromBlToDal(item);
        //    var updated = await _dal.Update(dalEntity);
        //    return CastingEnvironmentEntityFromDalToBl(updated);
        //}
        #endregion

        #region Delete
        public async Task Delete(int id)
        {
            await _dal.Delete(id);
        }
        #endregion


        // פונקציות המרה

        #region  CastingEnvironmentEntityFromDalToBl

        public BlEnvironmentEntity CastingEnvironmentEntityFromDalToBl(DalEnvironment e) =>
            new BlEnvironmentEntity
            {
                EnvironmentId = e.EnvironmentId,
                EnvironmentCode = e.EnvironmentCode,
                EnvironmentName = e.EnvironmentName,
                Description = e.Description
            };
        #endregion

        #region  CastingEnvironmentEntityFromBlToDal

        public DalEnvironment CastingEnvironmentEntityFromBlToDal(BlEnvironmentEntity? e) =>
            new DalEnvironment
            {
                EnvironmentId = e?.EnvironmentId ?? 0,
                EnvironmentCode = e?.EnvironmentCode ?? string.Empty,
                EnvironmentName = e?.EnvironmentName ?? string.Empty,
                Description = e?.Description
            };

        BlEnvironmentEntity IBlEnvironmentEntity.CastingEnvironmentEntityFromBlToDal(BlEnvironmentEntity? e)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlEnvironmentEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlEnvironmentEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlEnvironmentEntity> Create(BlEnvironmentEntity item)
        {
            throw new NotImplementedException();
        }

        public Task<BlEnvironmentEntity> Update(BlEnvironmentEntity item)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
