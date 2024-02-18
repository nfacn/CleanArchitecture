using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.IntegrationTests.Infrastructure.Fixtures;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.IntegrationTests.TodoItems.Commands;

[Collection(nameof(ClientCollectionFixture))]
public class UpdateTodoItemTests : IAsyncLifetime
{
    private readonly ClientFixture _clientFixture;

    public UpdateTodoItemTests(ClientFixture clientFixture)
    {
        _clientFixture = clientFixture;
    }

    [Fact]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new UpdateTodoItemCommand { Id = 99, Title = "New Title" };
        await FluentActions.Invoking(() => _clientFixture.SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldUpdateTodoItem()
    {
        var userId = await _clientFixture.RunAsDefaultUserAsync();

        var listId = await _clientFixture.SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var itemId = await _clientFixture.SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New Item"
        });

        var command = new UpdateTodoItemCommand
        {
            Id = itemId,
            Title = "Updated Item Title"
        };

        await _clientFixture.SendAsync(command);

        var item = await _clientFixture.FindAsync<TodoItem>(itemId);

        item.Should().NotBeNull();
        item!.Title.Should().Be(command.Title);
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await _clientFixture.ResetDatabaseAsync();
}
