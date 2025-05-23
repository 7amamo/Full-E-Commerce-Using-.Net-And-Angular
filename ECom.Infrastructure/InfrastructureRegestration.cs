using ECom.Core.Interfaces;
using ECom.Core.Services;
using ECom.Infrastructure.Data;
using ECom.Infrastructure.Repositories;
using ECom.Infrastructure.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
            Services.AddSingleton<IImageManagementService, ImageManagementService>();
            Services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot")));
            return Services;

        }
    }
}
