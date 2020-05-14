using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UWPLockStep.ApplicationLayer.FHIR.Queries;

using UWPLockStep.Persistance.Services;

namespace UWPLockStep.Persistance
{
    public static class DependencyInjection
    {
    //    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    //    {
            
    //        //services.AddHttpClient<GetRxCuiList.IHandler, GetRxCuiListHandler>(client => { client.BaseAddress = new Uri(); });
    //        services.AddDbContext<LockStepContextSql>(options =>
    //            options.UseSqlServer(configuration.GetConnectionString("MyAppConnectionString")));

    //        services.AddScoped<ILockStepContextSql>(provider => provider.GetService<LockStepContextSql>());

    //        return services;

    //    }
    }
}