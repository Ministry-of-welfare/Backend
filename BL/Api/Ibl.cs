namespace BL.Api
{
    public interface IBl
    {
        IBlEnvironmentEntity EnvironmentEntity { get; }
        IBlDataSourceType DataSourceType { get; }
        IBlSystem System { get; }

        IBlImportStatus ImportStatus { get; }
        IBlTabImportDataSource TabImportDataSource { get; } 


    }
}
