using EventManager.Models;
using EventManager.WebApi.Dtos;

namespace EventManager.WebApi.Converts
{
    /// <summary>
    /// Zbiór konwersji obiektów dto na modele.
    /// </summary>
    public static class ModelBuilder
    {
        /// <summary>
        /// Konwersja dodawanego lub aktualizowanego użytkownika na model.
        /// </summary>
        /// <param name="dto">Dane użytkownika</param>
        /// <returns>Model użytkownika</returns>
        public static User ConvertToUser(this UserStoreDto dto)
          => new User
          {
              Name = dto.Name,
              LastName = dto.LastName,
          };

        /// <summary>
        /// Konwersja dodawanego lub aktualizowanego wydarzenia na model.
        /// </summary>
        /// <param name="dto">Dane wydarzenia</param>
        /// <returns>Model wydarzenia</returns>
        public static Event ConvertToEvent(this EventStoreDto dto)
          => new Event
          {
              Name = dto.Name,
              Description = dto.Description,
              StartDate = dto.StartDate,
              EndDate = dto.EndDate,
              Cost = dto.Cost,
              MaxParticipants = dto.MaxParticipants,
          };

        /// <summary>
        /// Konwersja dodawanego lub aktualizowanego adresu na model.
        /// </summary>
        /// <param name="dto">Dane adresu</param>
        /// <returns>Model adresu</returns>
        public static Address ConvertToAddress(this AddressStoreDto dto)
          => new Address
          {
              City = dto.City,
              Street = dto.Street,
              HouseNumber = dto.HouseNumber,
          };

        /// <summary>
        /// Konwersja wzięcia udziału w wydarzeniu na model.
        /// </summary>
        /// <param name="dto">Dane uczestnictwa udziału w wydarzeniu.</param>
        /// <returns>Model udziału w wydarzeniu.</returns>
        public static UserEvent ConvertToUserEvent(this UserEventStoreDto dto)
          => new UserEvent
          {
              UserId = dto.UserId.TextToGuid(),
              EventId = dto.EventId.TextToGuid(),
          };
    }
}
