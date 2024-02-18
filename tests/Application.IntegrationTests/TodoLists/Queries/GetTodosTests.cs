using CleanArchitecture.Application.IntegrationTests.Infrastructure.Fixtures;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Application.IntegrationTests.TodoLists.Queries;

[Collection(nameof(ClientCollectionFixture))]
public class GetTodosTests : IAsyncLifetime
{
    private readonly ClientFixture _clientFixture;

    public GetTodosTests(ClientFixture clientFixture)
    {
        _clientFixture = clientFixture;
    }
    [Fact]
    public async Task ShouldReturnPriorityLevels()
    {
        await _clientFixture.RunAsDefaultUserAsync();

        var query = new GetTodosQuery();

        var result = await _clientFixture.SendAsync(query);

        result.PriorityLevels.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ShouldReturnAllListsAndItems()
    {
        await _clientFixture.RunAsDefaultUserAsync();

        await _clientFixture.AddAsync(new TodoList
        {
            Title = "Shopping",
            Colour = Colour.Blue,
            Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" }
                    }
        });

        var query = new GetTodosQuery();

        var result = await _clientFixture.SendAsync(query);

        result.Lists.Should().HaveCount(1);
        result.Lists.First().Items.Should().HaveCount(7);
    }

    [Fact]
    public async Task ShouldDenyAnonymousUser()
    {
        var query = new GetTodosQuery();

        var action = () => _clientFixture.SendAsync(query);
        
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await _clientFixture.ResetDatabaseAsync();
}
