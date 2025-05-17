using ECom.Core.Interfaces;
using ECom.Infrastructure.Data;
using ECom.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure
{
    public static class InfrastructureRegestration
    {
        public static IServiceCollection InfrastructureConfiguration (this IServiceCollection Services , IConfiguration configuration)
        {
            Services.AddScoped(typeof(IGenericRepository<>) , typeof(GenericRepository<>) );
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ECom"));
            });

            return Services;
        }
    }
}
