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
    public class BlImportControlService:IBlimportControl
    {

        private readonly IDalImportControl _dal;

        public BlImportControlService(IDalImportControl dal)
        {
            _dal = dal;
        }

        public async Task<List<BlAppImportControl>> GetAll()
        {
            var status = await _dal.GetAll();
            return status.Select(s => new BlAppImportControl 
            {
                ImportControlId = s.ImportControlId,
                ImportDataSourceId = s.ImportDataSourceId,
                ImportStartDate = s.ImportStartDate,
                ImportFinishDate = s.ImportFinishDate,
                TotalRows = s.TotalRows,
                TotalRowsAffected = s.TotalRowsAffected,
                RowsInvalid = s.RowsInvalid,
                FileName = s.FileName,
                ErrorReportPath = s.ErrorReportPath
            }).ToList();
        }

        public async Task<BlAppImportControl> GetById(int id)
        {
            var s= await _dal.GetByIdAsync(id);
            if (s== null) return null;
            else
                return new BlAppImportControl
                {
                    ImportControlId = s.ImportControlId,
                    ImportDataSourceId = s.ImportDataSourceId,
                    ImportStartDate = s.ImportStartDate,
                    ImportFinishDate = s.ImportFinishDate,
                    TotalRows = s.TotalRows,
                    TotalRowsAffected = s.TotalRowsAffected,
                    RowsInvalid = s.RowsInvalid,
                    FileName = s.FileName,
                    ErrorReportPath = s.ErrorReportPath
                };
        }

        public async Task<BlAppImportControl> Create(BlAppImportControl item)
        {
            var system = new Dal.Models.AppImportControl
            {
                ImportControlId = item.ImportControlId,
                ImportDataSourceId = item.ImportDataSourceId,
                ImportStartDate = item.ImportStartDate,
                ImportFinishDate = item.ImportFinishDate,
                TotalRows = item.TotalRows,
                TotalRowsAffected = item.TotalRowsAffected,
                RowsInvalid = item.RowsInvalid,
                FileName = item.FileName,
                ErrorReportPath = item.ErrorReportPath,





            };

            await _dal.Create(system);

            item.ImportControlId = system.ImportStatusId;
            return item;
        }

        public async Task<BlAppImportControl> Update(BlAppImportControl item)
        {
            var system = new Dal.Models.AppImportControl
            {
                ImportControlId = item.ImportControlId,
                ImportDataSourceId = item.ImportDataSourceId,
                ImportStartDate = item.ImportStartDate,
                ImportFinishDate = item.ImportFinishDate,
                TotalRows = item.TotalRows,
                TotalRowsAffected = item.TotalRowsAffected,
                RowsInvalid = item.RowsInvalid,
                FileName = item.FileName,
                ErrorReportPath = item.ErrorReportPath
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
