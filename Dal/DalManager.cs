using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DalManager
    {
        public IdalEnvironment Environment { get; }

        private readonly AppDbContext _context;
        //public DalManager()
        //{




        //    ServiceCollection services = new ServiceCollection();

        //    services.AddSingleton<IDalCustomer, DalCustomerService>();
        //    services.AddSingleton<IdalInvestmentProvider, DalInvestmentProviderService>();
        //    services.AddSingleton<IDalInvestment, DalInvestmentService>();
        //    services.AddSingleton<IDalRequest, DalRequestService>();

        //    services.AddSingleton<dbcontext>();
        //    ServiceProvider servicesProvider = services.BuildServiceProvider();


        //    // Customer = new DalCustomerService(data);
        //    Customer = servicesProvider.GetRequiredService<IDalCustomer>();
        //    InvestmentProvider = servicesProvider.GetRequiredService<IdalInvestmentProvider>();
        //    Investment = servicesProvider.GetRequiredService<IDalInvestment>();

        //    RequestDetails = servicesProvider.GetRequiredService<IDalRequest>();


        //}




    }
}
