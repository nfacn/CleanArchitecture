using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.IntegrationTests.Infrastructure.Fixtures;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.IntegrationTests.TodoLists.Commands;

[Collection(nameof(ClientCollectionFixture))]
public class CreateTodoListTests : IAsyncLifetime
{
    private readonly ClientFixture _clientFixture;

    public CreateTodoListTests(ClientFixture clientFixture)
    {
        _clientFixture = clientFixture;
    }
    [Fact]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTodoListCommand();
        await FluentActions.Invoking(() => _clientFixture.SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShouldRequireUniqueTitle()
    {
        await _clientFixture.SendAsync(new CreateTodoListCommand
        {
            Title = "Shopping"
        });

        var command = new CreateTodoListCommand
        {
            Title = "Shopping"
        };

        await FluentActions.Invoking(() =>
            _clientFixture.SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShouldCreateTodoList()
    {
        var userId = await _clientFixture.RunAsDefaultUserAsync();

        var command = new CreateTodoListCommand
        {
            Title = "Tasks"
        };

        var id = await _clientFixture.SendAsync(command);

        var list = await _clientFixture.FindAsync<TodoList>(id);

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.CreatedBy.Should().Be(userId);
        list.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await _clientFixture.ResetDatabaseAsync();
}
