using EventManager.Datalayer.Dbos;
using EventManager.Models;

namespace EventManager.Datalayer.Repositories
{
  public class UserRepository : Repository<User, UserDbo>, IUserRepository
  {
    public UserRepository(EventManagerDbContext context, IDboConverter dboConverter)
      : base(context, dboConverter)
    {
    }
  }
}
