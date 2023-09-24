using Book.API.Extensions;
using Book.Application.Interfaces;

namespace Book.API.Identity;

public class LoggedUser : ILoggedUser
{
    private readonly IHttpContextAccessor _accessor;

    public LoggedUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public Guid GetUserId()
    {
        if (_accessor.HttpContext is null)
        {
            throw new InvalidOperationException("HttpContext could not be found");
        }

        return Guid.Parse(_accessor.HttpContext.User.GetUserId());
    }
}
