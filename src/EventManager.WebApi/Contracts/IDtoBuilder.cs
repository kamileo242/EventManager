using EventManager.Models;
using EventManager.WebApi.Dtos;

namespace EventManager.WebApi
{
  /// <summary>
  /// Interfejs zawierający konwersje modeli na podstawowe obiekty Dto.
  /// </summary>
  public interface IDtoBuilder
  {
    /// <summary>
    /// Konwertuje model użytkownika na obiekt dto użytkownika.
    /// </summary>
    /// <param name="user">Model użytkownika</param>
    /// <returns>Obiekt dto użytkownika</returns>
    Task<UserDto> ConvertToUserDto(User user);

    /// <summary>
    /// Konwertuje model wydarzenia na obiekt dto wydarzenia.
    /// </summary>
    /// <param name="event">Model wydarzenia</param>
    /// <returns>Obiekt dto wydarzenia</returns>
    Task<EventDto> ConvertToEventDto(Event @event);

    /// <summary>
    /// Konwertuje model uczestnictwa użytkownika w wydarzeniu na obiekt dto uczestnictwa użytkownika w wydarzeniu.
    /// </summary>
    /// <param name="userEvent">Model uczestnictwa użytkownika w wydarzeniu</param>
    /// <returns>Obiekt dto uczestnictwa użytkownika w wydarzeniu</returns>
    Task<UserEventDto> ConvertToUserEventDto(UserEvent userEvent);

    /// <summary>
    /// Konwertuje model adresu na obiekt dto adresu.
    /// </summary>
    /// <param name="address">Model adresu</param>
    /// <returns>Obiekt dto adresu</returns>
    Task<AddressDto> ConvertToAddressDto(Address address);
  }
}
