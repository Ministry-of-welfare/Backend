

using Dal.Api;
using Dal.Models;
using Dal.Services;

namespace Dal
{
    public class DalManager:IDal
    {
        private readonly AppDbContext _context;
        

        public DalManager (AppDbContext context)
        {
            _context = context;
            //_connectionString = connectionString;

            Environments = new DalEnvironmentService(_context);
            DataSourceType = new DalDataSourceTypeService(_context);
            System = new DalSystemService(_context);
            ImportStatus = new DalImportStatusService(_context);
            TabImportDataSource = new DalImportDataSourceService(_context);
        }

        public IDalEnvironment Environments { get; }
        public IDalDataSourceType DataSourceType { get; }
        public IDalSystem System { get; }
        public IDalImportStatus ImportStatus { get; }
        public IDalImportDataSource TabImportDataSource { get; }
        //public IDalImportDataSource TabImportDataSource => new DalImportDataSourceService(_context, _connectionString);
    }
}
