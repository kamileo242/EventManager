using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Datalayer.Dbos;
using EventManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Datalayer.Repositories
{
  public class UserEventRepository : IUserEventRepository
  {
    private readonly EventManagerDbContext context;
    private readonly DbSet<UserEventDbo> dbSet;
    private readonly IDboConverter dboConverter;

    public UserEventRepository(EventManagerDbContext context, IDboConverter dboConverter)
    {
      this.context = context ?? throw new ArgumentNullException(nameof(context));
      this.dboConverter = dboConverter ?? throw new ArgumentNullException(nameof(dboConverter));
      this.dbSet = context.Set<UserEventDbo>();
    }

    public async Task<IEnumerable<UserEvent>> GetAllAsync()
    {
      var result = await dbSet.ToListAsync();

      return result.Select(s => dboConverter.Convert<UserEvent>(s)).ToList();
    }

    public async Task<UserEvent> GetAsync(Guid userId, Guid eventId)
    {
      var userEvent = await dbSet
          .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId);

      return dboConverter.Convert<UserEvent>(userEvent);
    }

    public async Task UpdateDepositPaidAsync(Guid userId, Guid eventId, decimal depositPaid)
    {
      var userEvent = await dbSet
          .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId);

      if (userEvent != null)
      {
        userEvent.DepositPaid += depositPaid;
        await context.SaveChangesAsync();
      }
    }

    public async Task DeleteAsync(Guid userId, Guid eventId)
    {
      var userEvent = await dbSet
          .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId);

      if (userEvent != null)
      {
        dbSet.Remove(userEvent);
        await context.SaveChangesAsync();
      }
    }

    public async Task AddAsync(UserEvent userEvent)
    {
      if (userEvent == null)
      {
        throw new ArgumentNullException(nameof(userEvent));
      }

      var dboEntity = dboConverter.Convert<UserEventDbo>(userEvent);
      await dbSet.AddAsync(dboEntity);
      await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserEvent>> FindAsync(Expression<Func<UserEvent, bool>> predicate)
    {
      var convertedPredicate = dboConverter.ConvertExpression<UserEvent, UserEventDbo>(predicate);
      var result = await dbSet.Where(convertedPredicate).ToListAsync();
      return result.Select(s => dboConverter.Convert<UserEvent>(s)).ToList();
    }
  }
}
