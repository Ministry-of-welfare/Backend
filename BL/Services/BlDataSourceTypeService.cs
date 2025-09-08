using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlDataSourceTypeService : IBlDataSourceType
    {
        private readonly IDalDataSourceType _dal;

        public BlDataSourceTypeService(IDalDataSourceType dal)
        {
            _dal = dal;
        }

        public async Task<List<BlTDataSourceType>> GetAll()
        {
            var dataSourceTypes = await _dal.GetAll();
            return dataSourceTypes.Select(d => new BlTDataSourceType
            {
                DataSourceTypeId = d.DataSourceTypeId,
                DataSourceTypeDesc = d.DataSourceTypeDesc
            }).ToList();
        }

        public async Task<BlTDataSourceType> GetById(int id)
        {
            var dataSourceType = await _dal.GetByIdAsync(id);
            if (dataSourceType == null) return null;

            return new BlTDataSourceType
            {
                DataSourceTypeId = dataSourceType.DataSourceTypeId,
                DataSourceTypeDesc = dataSourceType.DataSourceTypeDesc
            };
        }

        public async Task<BlTDataSourceType> Create(BlTDataSourceType item)
        {
            var dataSourceType = new Dal.Models.DataSourceType
            {
                DataSourceTypeDesc = item.DataSourceTypeDesc
            };

            await _dal.Create(dataSourceType);

            item.DataSourceTypeId = dataSourceType.DataSourceTypeId;
            return item;
        }

        public async Task<BlTDataSourceType> Update(BlTDataSourceType item)
        {
            var dataSourceType = new Dal.Models.DataSourceType
            {
                DataSourceTypeId = item.DataSourceTypeId,
                DataSourceTypeDesc = item.DataSourceTypeDesc
            };

            await _dal.Update(dataSourceType);
            return item;
        }

        public async Task Delete(int id)
        {
            await _dal.Delete(id);
        }
    }
}
