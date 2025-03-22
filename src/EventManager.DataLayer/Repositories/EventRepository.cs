using EventManager.Datalayer.Dbos;
using EventManager.Models;

namespace EventManager.Datalayer.Repositories
{
  public class EventRepository : Repository<Event, EventDbo>, IEventRepository
  {
    public EventRepository(EventManagerDbContext context, IDboConverter dboConverter)
      : base(context, dboConverter)
    {
    }
  }
}
