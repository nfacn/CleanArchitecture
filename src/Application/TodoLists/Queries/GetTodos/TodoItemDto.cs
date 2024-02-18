using CleanArchitecture.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using CleanArchitecture.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos;

public class TodoItemDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    public int Priority { get; init; }

    public string? Note { get; init; }

}

[Mapper]
public static partial class TodoItemDtoMapper
{
    public static partial TodoItemDto ToDto(this TodoItem todoItem);

    public static partial IQueryable<TodoItemDto> ProjectToDto(this IQueryable<TodoItem> q);
}
