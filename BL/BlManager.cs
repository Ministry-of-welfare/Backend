

using BL.Services;
using Dal;

namespace BL
{
    public class BLManager
    {
        public BLEnvironmentEntityService Environments { get; }

        public BLManager(IDal dal)
        {
            Environments = new BLEnvironmentEntityService(dal);
        }
    }
}
