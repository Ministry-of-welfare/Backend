using BL.Api;
using BL.Services;
using Dal.Api;

namespace BL
{
    public class BlManager : IBl
    {
        public IBlEnvironmentEntity EnvironmentEntity { get; }

        public BlManager(IDal dal)
        {
            EnvironmentEntity = new BlEnvironmentEntityService(dal.Environments);

        }
    }
}
