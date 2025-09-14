using System.Threading.Tasks;
using Dal.Models;

namespace Dal.Api
{
    public interface IDalImportDataSourceColumn : ICrud<TabImportDataSourceColumn>
    {
        Task<TabImportDataSourceColumn> GetById(int id);

    }
}
