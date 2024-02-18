#if (UseApiOnly)
using CleanArchitecture.Infrastructure.Identity;

namespace CleanArchitecture.API.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapIdentityApi<ApplicationUser>();
    }
}
#endif
