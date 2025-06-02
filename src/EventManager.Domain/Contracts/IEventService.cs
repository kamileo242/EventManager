using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Models;

namespace EventManager.Domain
{
    /// <summary>
    /// Zbiór operacji wykonywanych na serwisie wydarzenia.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Pobierz wydarzenie po id.
        /// </summary>
        /// <param name="id">Identyfikator wydarzenia</param>
        /// <returns>Wydarzenie</returns>
        Task<Event> GetByIdAsync(Guid id);

        /// <summary>
        /// Pobierz wszytskie wydarzenia.
        /// </summary>
        /// <returns>Lista wydarzeń</returns>
        Task<List<Event>> GetAllAsync();

        /// <summary>
        /// Pobierz listę wydarzeń na podstawie wskazanej właściwości.
        /// </summary>
        /// <param name="predicate">Wyrażenie warunkowe do filtrowania wydarzeń.</param>
        /// <returns>Lista wydarzeń</returns>
        Task<List<Event>> FindAsync(Expression<Func<Event, bool>> predicate);

        /// <summary>
        /// Dodaj nowe wydarzenie.
        /// </summary>
        /// <param name="event">Wydarzenie</param>
        Task AddAsync(Event @event);

        /// <summary>
        /// Zmień wybrane wydarzenia adresu.
        /// </summary>
        /// <param name="id">identyfikator wydarzenia do modyfikacji</param>
        /// <param name="event">Dane wydarzenia do modyfikacji</param>
        Task UpdateAsync(Guid id, Event @event);

        /// <summary>
        /// Aktualizuje adres na wydarzeniu.
        /// </summary>
        /// <param name="eventId">Identyfikator wydarzenia</param>
        /// <param name="addressId"></param>
        Task AddAddress(Guid eventId, Guid addressId);

        /// <summary>
        /// Usuń wydarzenie.
        /// </summary>
        /// <param name="id">Identyfikator wydarzenia</param>
        Task DeleteAsync(Guid id);
    }
}
