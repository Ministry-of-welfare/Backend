using Dal.Api;
using Dal.Models;

namespace Dal
{
    public class DalManager : IDal
    {
        private readonly AppDbContext _context;
        public DalManager()
        {

         Environments=new Services.DalEnvironmentService(_context);

         }
        public IDalEnvironment Environments { get; }

       
    }
}
