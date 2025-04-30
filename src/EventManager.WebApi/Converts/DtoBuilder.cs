using EventManager.Domain;
using EventManager.Models;
using EventManager.WebApi.Dtos;

namespace EventManager.WebApi.Converts
{
  public class DtoBuilder : IDtoBuilder
  {
    private readonly IAddressService addressService;
    private readonly IEventService eventService;
    private readonly IUserService userService;

    public DtoBuilder(IAddressService addressService, IEventService eventService, IUserService userService)
    {
      this.userService = userService;
      this.addressService = addressService;
      this.eventService = eventService;
    }
    public async Task<AddressDto> ConvertToAddressDto(Address address)
      => new AddressDto
      {
        Id = address.Id.GuidToText(),
        City = address.City,
        HouseNumber = address.HouseNumber,
        Street = address.Street,
        Events = (await Task.WhenAll(address.EventsIds.Select(id => ConvertIdToEvent(id)))).ToList()
      };

    public async Task<EventDto> ConvertToEventDto(Event @event)
      => new EventDto
      {
        Id = @event.Id.GuidToText(),
        Name = @event.Name,
        StartDate = @event.StartDate,
        EndDate = @event.EndDate,
        Address = await ConvertIdToAddress(@event.AddressId.GetValueOrDefault()),
        Description = @event.Description,
        Cost = @event.Cost,
        MaxParticipants = @event.MaxParticipants
      };

    public async Task<UserDto> ConvertToUserDto(User user)
      => new UserDto
      {
        Id = user.Id.GuidToText(),
        Name = user.Name,
        LastName = user.LastName,
      };

    public async Task<UserEventDto> ConvertToUserEventDto(UserEvent userEvent)
      => new UserEventDto
      {
        User = await ConvertIdToUser(userEvent.UserId),
        Event = await ConvertIdToEvent(userEvent.EventId),
        DepositPaid = userEvent.DepositPaid,
        JoinedAt = userEvent.JoinedAt,
      };

    private async Task<AddressDto> ConvertIdToAddress(Guid id)
    {
      var address = await addressService.GetByIdAsync(id);

      return await ConvertToAddressDto(address);
    }

    private async Task<UserDto> ConvertIdToUser(Guid id)
    {
      var user = await userService.GetByIdAsync(id);

      return await ConvertToUserDto(user);
    }

    private async Task<EventDto> ConvertIdToEvent(Guid id)
    {
      var @event = await eventService.GetByIdAsync(id);

      return await ConvertToEventDto(@event);
    }
  }
}
