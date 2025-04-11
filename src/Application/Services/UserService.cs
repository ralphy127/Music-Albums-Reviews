namespace MAR.Application.Services;

using MAR.Domain.Events;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;

public class UserService : BaseService<User>, IUserService
{
    private readonly IDomainEventDispatcher _eventDispatcher;

    public UserService(IUserRepository repository, ILoggerAdapter<User> logger, IDomainEventDispatcher eventDispatcher) 
        : base(repository, logger)
    {
        _eventDispatcher = eventDispatcher;
    }

    public override async Task DeleteAsync(User user)
    {
        await _eventDispatcher.Dispatch(new UserDeletedEvent(user));
        await base.DeleteAsync(user);
    }
}