using BL.Api;
using BL.Services;
using Dal.Api;

namespace BL
{
    public class BlManager : IBl
    {
        public IBlEnvironmentEntity EnvironmentEntity { get; }
        public IBlDataSourceType DataSourceType { get; }
        public IBlSystem System { get; }

        public IBlImportStatus ImportStatus { get; }
        public BlTabImportDataSourceService TabImportDataSourceService { get; }

        public IBlTabImportDataSource TabImportDataSource { get; }

        public BlManager(IDal dal)
        {
            EnvironmentEntity = new BlEnvironmentEntityService(dal.Environments);
            DataSourceType = new BlDataSourceTypeService(dal.DataSourceType);
            System = new BlSystemService(dal.System);
            ImportStatus = new BlImportStatusService(dal.ImportStatus);
            TabImportDataSourceService = new BlTabImportDataSourceService(dal.TabImportDataSource);
        }
    }
}
