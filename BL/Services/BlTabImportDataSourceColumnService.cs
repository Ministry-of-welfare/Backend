using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;

namespace BL.Services
{
    public class BlTabImportDataSourceColumnService : IblTabImportDataSourceColumn
    {
        private readonly IDalImportDataSourceColumn _dal;

        public BlTabImportDataSourceColumnService(IDalImportDataSourceColumn dal)
        {
            _dal = dal;
        }

        // ממיר DAL -> BL
        public static BlTabImportDataSourceColumn ToBl(TabImportDataSourceColumn dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new BlTabImportDataSourceColumn
            {
                ImportDataSourceColumnsId = dto.ImportDataSourceId,
                ImportDataSourceId = dto.ImportDataSourceId,
                OrderId = dto.OrderId,
                ColumnName = dto.ColumnName,
                FormatColumnId = dto.FormatColumnId
            };
        }

        // ממיר BL -> DAL
        public static TabImportDataSourceColumn ToDal(BlTabImportDataSourceColumn bl)
        {
            if (bl == null) throw new ArgumentNullException(nameof(bl));

            return new TabImportDataSourceColumn
            {
                ImportDataSourceColumnsId = bl.ImportDataSourceId,
                ImportDataSourceId = bl.ImportDataSourceId,
                OrderId = bl.OrderId,
                ColumnName = bl.ColumnName,
                FormatColumnId = bl.FormatColumnId
            };
        }

        public async Task Create(BlTabImportDataSourceColumn item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var dalEntity = ToDal(item); // Correctly converts from BL to DAL type
            await _dal.Create(dalEntity);
        }

        public async Task Delete(int id)
        {
            await _dal.Delete(id);
        }

        public async Task<List<BlTabImportDataSourceColumn>> GetAll()
        {
            var data = await _dal.GetAll();
            return data.Select(ToBl).ToList(); // Correctly converts DAL to BL type
        }

        public async Task<BlTabImportDataSourceColumn> GetById(int id)
        {
            var entity = await _dal.GetById(id);
            return entity == null ? null : ToBl(entity); // Fixes CS0029 and CS8603
        }

        public async Task<BlTabImportDataSourceColumn> Update(BlTabImportDataSourceColumn item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var dalEntity = ToDal(item);

            // Fix for CS0815: Explicitly call the Update method and return the updated entity
            await _dal.Update(dalEntity);

            // Return the updated BL entity
            return ToBl(dalEntity);
        }

    }

}
