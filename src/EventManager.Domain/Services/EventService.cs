using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Datalayer;
using EventManager.Domain.Providers;
using EventManager.Models;
using EventManager.Models.Exceptions;

namespace EventManager.Domain.Services
{
  public class EventService : IEventService
  {
    private readonly IEventRepository repository;
    private readonly IAddressRepository addressRepository;

    public EventService(IEventRepository repository, IAddressRepository addressRepository)
    {
      this.repository = repository;
      this.addressRepository = addressRepository;
    }

    public async Task<Event> GetByIdAsync(Guid id)
    {
      var @event = await repository.GetByIdAsync(id);

      if (@event == null)
      {
        throw new MissingDataException($"Nie znaleziono wydarzenia o id: {id}.");
      }

      return @event;
    }

    public async Task<List<Event>> GetAllAsync()
    {
      var events = await repository.GetAllAsync();

      return events.ToList();
    }

    public async Task<List<Event>> FindAsync(Expression<Func<Event, bool>> predicate)
    {
      var events = await repository.FindAsync(predicate);
      return events.ToList();
    }

    public async Task AddAsync(Event @event)
    {
      await ValidateStoreEvent(@event);
      await ValidateAddress(@event.AddressId.GetValueOrDefault());

      @event.Id = GuidProvider.GenetareGuid();

      await repository.AddAsync(@event);
    }

    public async Task UpdateAsync(Guid id, Event @event)
    {
      var existingEvent = await repository.GetByIdAsync(id);
      if (existingEvent == null)
      {
        throw new MissingDataException($"Nie znaleziono wydarzenia o id: {id}.");
      }
      @event.Id = id;

      await ValidateAddress(@event.AddressId.GetValueOrDefault());

      await repository.UpdateAsync(@event);
    }

    public async Task DeleteAsync(Guid id)
      => await repository.DeleteAsync(id);

    private async Task ValidateStoreEvent(Event @event)
    {
      if (string.IsNullOrWhiteSpace(@event?.Name))
      {
        throw new IncorrectDataException("Pozycja 'Nazwa' jest wymagana.");
      }
    }

    private async Task ValidateAddress(Guid id)
    {
      if (id != Guid.Empty)
      {
        var address = await addressRepository.GetByIdAsync(id);

        if (address == null)
        {
          throw new MissingDataException($"Nie znaleziono wydarzenia o id: {id}.");
        }
      }
    }
  }
}
