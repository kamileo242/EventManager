using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Models;

namespace EventManager.Domain
{
  /// <summary>
  /// Interfejs serwisu obsługującego operacje na serwisie łączącym wydarzenia z użytkownikami.
  /// </summary>
  public interface IUserEventService
  {

    /// <summary>
    /// Pobiera wszystkie połączenia użytkowników z wydarzeniami.
    /// </summary>
    /// <returns>Lista uczestnictwa wszystkich użytkowniików we wszystkich wydarzeniach</returns>
    Task<IEnumerable<UserEvent>> GetAllAsync();

    /// <summary>
    /// Pobiera informacje o uczestnictwie użytkownika w danym wydarzeniu.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="eventId">Identyfikator wydarzenia.</param>
    /// <returns>Informacja o uczestnictwie użytkownika w danym wydarzeniu</returns>
    Task<UserEvent> GetAsync(Guid userId, Guid eventId);

    /// <summary>
    /// Aktualizuje opłaconą kwotę użytkownika w danym wydarzeniu.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika</param>
    /// <param name="eventId">Identyfikator wydarzenia</param>
    /// <param name="depositPaid">Wpłacona ilość</param>
    Task UpdateDepositPaidAsync(Guid userId, Guid eventId, decimal depositPaid);

    /// <summary>
    /// Usuwa uczestnictwo użytkownika w danym wydarzeniu.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika..</param>
    /// <param name="eventId">Identyfikator wydarzenia</param>
    Task DeleteAsync(Guid userId, Guid eventId);

    /// <summary>
    /// Dodaje użytkownika do danego wydarzenia.
    /// </summary>
    /// <param name="userEvent">Danne uczestnictwa w wydarzeniu.</param>
    /// <returns></returns>
    Task AddAsync(UserEvent userEvent);

    /// <summary>
    /// Wyszukuje uczestnictwa użytkowników w wydarzeniach na podstawie wyszukiwanej frazy.
    /// </summary>
    /// <param name="predicate">Wyrażenie warunkowe do filtrowania uczestnictwa użytkowników w wydarzeniach.</param>
    /// <returns>Uczestnictwo użytkownika w wydarzeniu.</returns>
    Task<IEnumerable<UserEvent>> FindAsync(Expression<Func<UserEvent, bool>> predicate);
  }
}
