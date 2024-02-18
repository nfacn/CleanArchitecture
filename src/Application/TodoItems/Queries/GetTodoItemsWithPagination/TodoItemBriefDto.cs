using System.Diagnostics;
using CleanArchitecture.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace CleanArchitecture.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class TodoItemBriefDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

}

[Mapper]
public static partial class TodoItemBriefDtoMapper
{
    public static partial TodoItemBriefDto ToDto(this TodoItem todoItem);

    public static partial IQueryable<TodoItemBriefDto> ProjectToDto(this IQueryable<TodoItem> q);
}
