using BL.Api;
using BL.Models;
using Dal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class BlImportStatusService:IBlImportStatus
    {

        private readonly IDalImportStatus _dal;

        public BlImportStatusService(IDalImportStatus dal )
        {   
            _dal = dal;
        }

        public async Task<List<BlTImportStatus>> GetAll()
        {
            var status = await _dal.GetAll();
            return status.Select(s => new BlTImportStatus // Changed from IBlImportStatus to BlImportStatus
            {
                ImportStatusId = s.ImportStatusId, // Fixed: Correctly reference the property from 's'
                ImportStatusDesc = s.ImportStatusDesc // Fixed: Correctly reference the property from 's'
            }).ToList();
        }

        public async Task<BlTImportStatus> GetById(int id)
        {
            var system = await _dal.GetByIdAsync(id);
            if (system == null) return null;
            else
                return new BlTImportStatus
                {
                    ImportStatusId = system.ImportStatusId,
                    ImportStatusDesc = system.ImportStatusDesc
                };
        }

        public async Task<BlTImportStatus> Create(BlTImportStatus item)
        {
            var system = new Dal.Models.TImportStatus
            {
                ImportStatusDesc = item.ImportStatusDesc
            };

            await _dal.Create(system);

            item.ImportStatusId = system.ImportStatusId;
            return item;
        }

        public async Task<BlTImportStatus> Update(BlTImportStatus item)
        {
            var system = new Dal.Models.TImportStatus
            {
                ImportStatusId = item.ImportStatusId,
                ImportStatusDesc = item.ImportStatusDesc
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
