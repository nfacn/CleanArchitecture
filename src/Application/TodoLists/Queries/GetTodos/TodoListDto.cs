using CleanArchitecture.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos;

public class TodoListDto
{
    public TodoListDto()
    {
        Items = [];
    }

    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Colour { get; init; }

    public IReadOnlyCollection<TodoItemDto> Items { get; init; }

}

[Mapper]
public static partial class TodoListDtoMapper
{
    public static partial TodoItemDto ToDto(this TodoList todoItem);

    public static partial IQueryable<TodoListDto> ProjectToDto(this IQueryable<TodoList> q);
}
