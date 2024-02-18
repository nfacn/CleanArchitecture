using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .FindAsync([request.Id], cancellationToken);

        // TODO: Handle for not found/null case
        if (entity == null)
            return;
        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);

    }
}
