namespace Dal.Api
{
    public interface IDal
    {
       public IDalEnvironment Environments { get; }
        public IDalDataSourceType DataSourceType { get; }
        public IDalSystem System { get; }
    }
}
