
using BL.Models;
using Dal.Api;   // בשביל ה-IDal
using Dal.Models;
using System.Collections.Generic;

namespace BL.Services
{
    public class BLEnvironmentEntityService
    {
        private readonly IDal _dal;

        public BLEnvironmentEntityService(IDal dal)
        {
            _dal = dal;
        }

        public IEnumerable<BLEnvironmentEntity> GetAll()
        {
            var dalEntities = _dal.Environments.GetAll();

            return dalEntities.Select(e => new BLEnvironmentEntity
            {
                EnvironmentId = e.EnvironmentId,
                EnvironmentCode = e.EnvironmentCode,
                EnvironmentName = e.EnvironmentName,
                Description = e.Description
            });
        }

        public BLEnvironmentEntity? GetById(int id)
        {
            var entity = _dal.Environments.GetById(id);

            if (entity == null) return null;

            return new BLEnvironmentEntity
            {
                EnvironmentId = entity.EnvironmentId,
                EnvironmentCode = entity.EnvironmentCode,
                EnvironmentName = entity.EnvironmentName,
                Description = entity.Description
            };
        }

        public void Add(BLEnvironmentEntity entity)
        {
            _dal.Environments.Add(new Dal.Models.EnvironmentEntity
            {
                EnvironmentId = entity.EnvironmentId,
                EnvironmentCode = entity.EnvironmentCode,
                EnvironmentName = entity.EnvironmentName,
                Description = entity.Description
            });
        }
    }
}
