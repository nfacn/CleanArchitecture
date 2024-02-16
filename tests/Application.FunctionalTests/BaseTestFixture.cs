namespace CleanArchitecture.Application.FunctionalTests;

using static Testing;

public abstract class BaseTestFixture : IDisposable
{
    protected BaseTestFixture()
    {
        ResetState().Wait();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
