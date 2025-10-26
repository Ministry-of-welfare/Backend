
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
        public IBlTabImportDataSource TabImportDataSource { get; }
        public IblTabImportDataSourceColumn TabImportDataSourceColumn { get; }
        public IBlFileStatus FileStatus { get; }
        public IBlimportControl ImportControl { get; }


        // public IBlTabImportDataSource TabImportDataSourceService { get; }

        public BlManager(IDal dal)
        {
            EnvironmentEntity = new BlEnvironmentEntityService(dal.Environments);
            DataSourceType = new BlDataSourceTypeService(dal.DataSourceType);
            System = new BlSystemService(dal.System);
            ImportStatus = new BlImportStatusService(dal.ImportStatus);
            TabImportDataSource = new BlTabImportDataSourceService(dal.TabImportDataSource);
            TabImportDataSourceColumn = new BlTabImportDataSourceColumnService(dal.ImportDataSourceColumn);
            FileStatus = new BlFileStatusService(dal.Tfilestatus);
            ImportControl = new BlImportControlService(dal.ImportControl);
            // TabImportDataSourceService = TabImportDataSource; // �� ��� ����� ���� ����
        }
    }
}