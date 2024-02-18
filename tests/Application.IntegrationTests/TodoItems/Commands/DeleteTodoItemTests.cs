using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.IntegrationTests.Infrastructure.Fixtures;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.DeleteTodoItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.IntegrationTests.TodoItems.Commands;

[Collection(nameof(ClientCollectionFixture))]
public class DeleteTodoItemTests : IAsyncLifetime
{
    private readonly ClientFixture _clientFixture;

    public DeleteTodoItemTests(ClientFixture clientFixture)
    {
        _clientFixture = clientFixture;
    }

    [Fact]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemCommand(99);

        await FluentActions.Invoking(() =>
            _clientFixture.SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldDeleteTodoItem()
    {
        var listId = await _clientFixture.SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var itemId = await _clientFixture.SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New Item"
        });

        await _clientFixture.SendAsync(new DeleteTodoItemCommand(itemId));

        var item = await _clientFixture.FindAsync<TodoItem>(itemId);

        item.Should().BeNull();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await _clientFixture.ResetDatabaseAsync();
}
