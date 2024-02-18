using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.IntegrationTests.Infrastructure.Fixtures;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.PurgeTodoLists;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.IntegrationTests.TodoLists.Commands;

[Collection(nameof(ClientCollectionFixture))]
public class PurgeTodoListsTests : IAsyncLifetime
{
    private readonly ClientFixture _clientFixture;

    public PurgeTodoListsTests(ClientFixture clientFixture)
    {
        _clientFixture = clientFixture;
    }
    [Fact]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new PurgeTodoListsCommand();

        command.GetType().Should().BeDecoratedWith<AuthorizeAttribute>();

        var action = () => _clientFixture.SendAsync(command);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task ShouldDenyNonAdministrator()
    {
        await _clientFixture.RunAsDefaultUserAsync();

        var command = new PurgeTodoListsCommand();

        var action = () => _clientFixture.SendAsync(command);

        await action.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task ShouldAllowAdministrator()
    {
        await _clientFixture.RunAsAdministratorAsync();

        var command = new PurgeTodoListsCommand();

        var action = () => _clientFixture.SendAsync(command);

        await action.Should().NotThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task ShouldDeleteAllLists()
    {
        await _clientFixture.RunAsAdministratorAsync();

        await _clientFixture.SendAsync(new CreateTodoListCommand
        {
            Title = "New List #1"
        });

        await _clientFixture.SendAsync(new CreateTodoListCommand
        {
            Title = "New List #2"
        });

        await _clientFixture.SendAsync(new CreateTodoListCommand
        {
            Title = "New List #3"
        });

        await _clientFixture.SendAsync(new PurgeTodoListsCommand());

        var count = await _clientFixture.CountAsync<TodoList>();

        count.Should().Be(0);
    }
    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await _clientFixture.ResetDatabaseAsync();
}
