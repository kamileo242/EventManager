using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Datalayer;
using EventManager.Models;
using EventManager.Models.Exceptions;

namespace EventManager.Domain.Services
{
  public class UserEventService : IUserEventService
  {
    private readonly IUserRepository userRepository;
    private readonly IEventRepository eventRepository;
    private readonly IUserEventRepository userEventRepository;

    public UserEventService(IUserRepository userRepository, IEventRepository eventRepository, IUserEventRepository userEventRepository)
    {
      this.userRepository = userRepository;
      this.eventRepository = eventRepository;
      this.userEventRepository = userEventRepository;
    }

    public async Task<UserEvent> GetAsync(Guid userId, Guid eventId)
    {
      var userEvent = await userEventRepository.GetAsync(userId, eventId);

      if (userEvent == null)
      {
        throw new MissingDataException($"Nie znaleziono uczestnictwa użytkownika o id: {userId} w wydarzeniu o id: {eventId}.");
      }

      return userEvent;
    }

    public async Task<IEnumerable<UserEvent>> GetAllAsync()
    {
      var userEvents = await userEventRepository.GetAllAsync();

      return userEvents.ToList();
    }

    public async Task<IEnumerable<UserEvent>> FindAsync(Expression<Func<UserEvent, bool>> predicate)
    {
      var userEvents = await userEventRepository.FindAsync(predicate);
      return userEvents.ToList();
    }

    public async Task AddAsync(UserEvent userEvent)
    {
      await ValidateUserEvent(userEvent);

      await userEventRepository.AddAsync(userEvent);
    }

    public async Task UpdateDepositPaidAsync(Guid userId, Guid eventId, decimal depositPaid)
    {
      var userEvent = await userEventRepository.GetAsync(userId, eventId);

      if (userEvent == null)
      {
        throw new MissingDataException($"Nie znaleziono uczestnictwa użytkownika o id: {userId} w wydarzeniu o id: {eventId}.");
      }

      await userEventRepository.UpdateDepositPaidAsync(userId, eventId, depositPaid);
    }

    public async Task DeleteAsync(Guid userId, Guid eventId)
      => await userEventRepository.DeleteAsync(userId, eventId);

    private async Task ValidateUserEvent(UserEvent userEvent)
    {
      if (userEvent == null)
      {
        throw new ArgumentNullException();
      }
      await ValidateUser(userEvent.UserId);
      await ValidateEvent(userEvent.EventId);
    }

    private async Task ValidateUser(Guid id)
    {
      if (id == Guid.Empty)
      {
        throw new IncorrectDataException("Nie podano identyfikatora użytkownika.");
      }

      var user = await userRepository.GetByIdAsync(id);
      if (user == null)
      {
        throw new MissingDataException($"Nie znaleziono użytkownika o id {id}.");
      }
    }

    private async Task ValidateEvent(Guid id)
    {
      if (id == Guid.Empty)
      {
        throw new IncorrectDataException("Nie podano identyfikatora wydarzenia.");
      }

      var @event = await eventRepository.GetByIdAsync(id);
      if (@event == null)
      {
        throw new MissingDataException($"Nie znaleziono wydarzenia o id {id}.");
      }
    }
  }
}
