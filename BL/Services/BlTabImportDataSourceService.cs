using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public class BlTabImportDataSourceService : IBlTabImportDataSource
    {
        private readonly IDalImportDataSource _dal;

        public BlTabImportDataSourceService(IDalImportDataSource dal)
        {
            _dal = dal;
        }


        public static BlTabImportDataSource ToBl(TabImportDataSource dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new BlTabImportDataSource
            {
                ImportDataSourceId = dto.ImportDataSourceId,
                ImportDataSourceDesc = dto.ImportDataSourceDesc ?? string.Empty,
                DataSourceTypeId = dto.DataSourceTypeId,
                SystemId = dto.SystemId,
                JobName = dto.JobName,
                TableName = dto.TableName,
                UrlFile = dto.UrlFile ?? string.Empty,
                UrlFileAfterProcess = dto.UrlFileAfterProcess ?? string.Empty,
                EndDate = dto.EndDate,
                ErrorRecipients = dto.ErrorRecipients,
                InsertDate = dto.InsertDate,
                StartDate = dto.StartDate
            };
        }

        public static TabImportDataSource ToDal(BlTabImportDataSource bl)
        {
            if (bl == null) throw new ArgumentNullException(nameof(bl));

            return new TabImportDataSource
            {
                ImportDataSourceId = bl.ImportDataSourceId,
                ImportDataSourceDesc = bl.ImportDataSourceDesc ?? string.Empty,
                DataSourceTypeId = bl.DataSourceTypeId,
                SystemId = bl.SystemId,
                JobName = bl.JobName,
                TableName = bl.TableName,
                UrlFile = bl.UrlFile ?? string.Empty,
                UrlFileAfterProcess = bl.UrlFileAfterProcess ?? string.Empty,
                EndDate = bl.EndDate,
                ErrorRecipients = bl.ErrorRecipients,
                InsertDate = bl.InsertDate,
                StartDate = bl.StartDate
            };
        }
        public async Task<List<BlTabImportDataSource>> GetAll()
        {
            var data = await _dal.GetAll(); 
            return data.Select(s => ToBl(s)).ToList();
        }

        public async Task Create(BlTabImportDataSource item)
        {
            var dalEntity = ToDal(item);
            await _dal.Create(dalEntity);
            
        }

        public async Task<TabImportDataSource> Update(TabImportDataSource item)
        {
            _dal.Update(item);
            return await Task.FromResult(item);
        }

        public Task Delete(int id)
        {
            _dal.Delete(id);
            return Task.CompletedTask;
        }

        

        Task<BlTabImportDataSource> IBlTabImportDataSource.GetById(int id)
        {
            throw new NotImplementedException();
        }

        //public async Task<TabImportDataSource> GetById(int id)
        //{
        //    return await Task.FromResult(_dal.GetById(id));
        //}
        

        public Task<BlTabImportDataSource> Update(BlTabImportDataSource item)
        {
            throw new NotImplementedException();
        }
    }
}
