using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using Dal.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services
{
    public class BlFileStatusService : IBlFileStatus
    {

        private readonly IDalFileStatus _dal;

        public BlFileStatusService(IDalFileStatus dal)
        {
            _dal = dal;
        }
       
       
        public async Task<List<BlTFileStatus>> GetAll()
        {
            var fieStatus = await _dal.GetAll();
            return fieStatus.Select(s => new BlTFileStatus
            {
                FileStatusId = s.FileStatusId,
                FileStatusDesc = s.FileStatusDesc
            }).ToList();
        }
        public async Task<BlTFileStatus> GetById(int id)
        {
            var fileStatus = await _dal.GetByIdAsync(id);
            if (fileStatus == null) return null;
            else
                return new BlTFileStatus
                {
                    FileStatusId = fileStatus.FileStatusId,
                    FileStatusDesc = fileStatus.FileStatusDesc
                };
        }
       

      
        public async Task<BlTFileStatus> Create(BlTFileStatus item)
        {
            var fileStatus = new Dal.Models.TFileStatus
            {
                FileStatusId = item.FileStatusId,
                FileStatusDesc = item.FileStatusDesc
            };

            await _dal.Create(fileStatus);

            item.FileStatusId = fileStatus.FileStatusId;
            return item;
        }
        
        public async Task<BlTFileStatus> Update(BlTFileStatus item)
        {
            var fileStatus = new Dal.Models.TFileStatus
            {
                FileStatusId = item.FileStatusId,
                FileStatusDesc = item.FileStatusDesc
            };

            await _dal.Update(fileStatus);
            return item;
        }

        public async Task Delete(int id)
        {
            await _dal.Delete(id);
        }
        

        




        

        

      
    }
}