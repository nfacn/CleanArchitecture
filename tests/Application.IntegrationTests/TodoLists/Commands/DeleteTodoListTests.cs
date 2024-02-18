using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.IntegrationTests.Infrastructure.Fixtures;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.DeleteTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.IntegrationTests.TodoLists.Commands;

[Collection(nameof(ClientCollectionFixture))]
public class DeleteTodoListTests : IAsyncLifetime
{
    private readonly ClientFixture _clientFixture;

    public DeleteTodoListTests(ClientFixture clientFixture)
    {
        _clientFixture = clientFixture;
    }
    [Fact]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new DeleteTodoListCommand(99);
        await FluentActions.Invoking(() => _clientFixture.SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldDeleteTodoList()
    {
        var listId = await _clientFixture.SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        await _clientFixture.SendAsync(new DeleteTodoListCommand(listId));

        var list = await _clientFixture.FindAsync<TodoList>(listId);

        list.Should().BeNull();
    }
    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await _clientFixture.ResetDatabaseAsync();
}
