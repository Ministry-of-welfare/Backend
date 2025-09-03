using BL.Api;
using BL.Services;
using Dal.Api;
using Dal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BL
{
    public class BlManager : IBl
    {
        public IBlEnvironmentEntity EnvironmentEntity { get; }

        public BlManager()
        {
            var services = new ServiceCollection();

            // DAL
            services.AddSingleton<IDalEnvironmentEntity, DalEnvironmentEntityService>();

            // BL
            services.AddSingleton<IBlEnvironmentEntity, BlEnvironmentEntityService>();

            var provider = services.BuildServiceProvider();

            EnvironmentEntity = provider.GetRequiredService<IBlEnvironmentEntity>();
        }
    }
}
