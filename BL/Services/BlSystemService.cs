using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlSystemService : IBlSystem
    {
        private readonly IDalSystem _dal;

        public BlSystemService(IDalSystem dal)
        {
            _dal = dal;
        }

        public async Task<List<BlTSystem>> GetAll()
        {
            var systems = await _dal.GetAll();
            return systems.Select(s => new BlTSystem
            {
                SystemId = s.SystemId,
                SystemCode = s.SystemCode,
                SystemName = s.SystemName
            }).ToList();
        }

        public async Task<BlTSystem> GetById(int id)
        {
            var system = await _dal.GetByIdAsync(id);
            if (system == null) return null;

            return new BlTSystem
            {
                SystemId = system.SystemId,
                SystemCode = system.SystemCode,
                SystemName = system.SystemName
            };
        }

        public async Task<BlTSystem> Create(BlTSystem item)
        {
            var system = new Dal.Models.System
            {
                SystemCode = item.SystemCode,
                SystemName = item.SystemName
            };

            await _dal.Create(system);

            item.SystemId = system.SystemId;
            return item;
        }

        public async Task<BlTSystem> Update(BlTSystem item)
        {
            var system = new Dal.Models.System
            {
                SystemId = item.SystemId,
                SystemCode = item.SystemCode,
                SystemName = item.SystemName
            };

            await _dal.Update(system);
            return item;
        }

        public async Task Delete(int id)
        {
            await _dal.Delete(id);
        }
    }
}
