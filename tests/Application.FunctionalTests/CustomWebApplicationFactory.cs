using System.Data.Common;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Application.FunctionalTests;

using static Testing;

public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection;

    public CustomWebApplicationFactory(DbConnection connection)
    {
        _connection = connection;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<IUser>()
                .AddTransient(provider => Mock.Of<IUser>(s => s.Id == GetUserId()));

            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
#if (UseSQLite)
                    options.UseSqlite(_connection);
#else
                    options.UseSqlServer(_connection);
#endif
                });
        });
    }
}
