using Dal.Api;
using Dal.Models;

namespace Dal
{
    public class DalManager : IDal
    {
        private readonly AppDbContext _context;
        public DalManager()
        {

            Environments = new Services.DalEnvironmentService(_context);
            DataSourceType = new Services.DalDataSourceTypeService(_context);
            System = new Services.DalSystemService(_context);
            TabImportDataSource = new Services.DalTabImportDataSourceService(_context);


        }
        public IDalEnvironment Environments { get; }
        public IDalDataSourceType DataSourceType { get; }
        public IDalSystem System { get; }
        public IDalTabImportDataSource TabImportDataSource { get; }


    }
}
