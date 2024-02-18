using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.IntegrationTests.Infrastructure.Databases;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchitecture.Application.IntegrationTests.Infrastructure;

public class CustomWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public ITestDatabase Database = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<IUser>()
                .AddScoped<IUser, CurrentTestUser>();
            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    options.UseSqlServer(Database.GetConnection());
                });
        });
        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync()
    {
        Database = await TestDatabaseFactory.CreateAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await Database.DisposeAsync();
    }
}
