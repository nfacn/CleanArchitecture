using System.Data.Common;

namespace CleanArchitecture.Application.IntegrationTests.Infrastructure.Databases;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetConnection();

    Task ResetAsync();

    Task DisposeAsync();
}
