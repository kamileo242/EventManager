using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Models;

namespace EventManager.Datalayer
{
  /// <summary>
  /// Zbiór operacji wykonywanych na repozytorium łączącym wydarzenia z użytkownikami.
  /// </summary>
  public interface IUserEventRepository
  {
    /// <summary>
    /// Pobiera wszystkie encje UserEvent.
    /// </summary>
    /// <returns>Lista encji UserEvent.</returns>
    Task<IEnumerable<UserEvent>> GetAllAsync();

    /// <summary>
    /// Pobiera encję UserEvent na podstawie złożonego klucza (UserId i EventId).
    /// </summary>
    /// <param name="userId">Id użytkownika.</param>
    /// <param name="eventId">Id wydarzenia.</param>
    /// <returns>Obiekt UserEvent.</returns>
    Task<UserEvent> GetAsync(Guid userId, Guid eventId);

    /// <summary>
    /// Aktualizuje wartość DepositPaid dla UserEvent na podstawie złożonego klucza.
    /// </summary>
    /// <param name="userId">Id użytkownika.</param>
    /// <param name="eventId">Id wydarzenia.</param>
    /// <param name="depositPaid">Kwota zaliczki, która została zapłacona.</param>
    Task UpdateDepositPaidAsync(Guid userId, Guid eventId, decimal depositPaid);

    /// <summary>
    /// Usuwa encję UserEvent na podstawie złożonego klucza (UserId i EventId).
    /// </summary>
    /// <param name="userId">Id użytkownika.</param>
    /// <param name="eventId">Id wydarzenia.</param>
    Task DeleteAsync(Guid userId, Guid eventId);

    /// <summary>
    /// Dodaje nową encję UserEvent.
    /// </summary>
    /// <param name="userEvent">Obiekt encji UserEvent do dodania.</param>
    Task AddAsync(UserEvent userEvent);

    /// <summary>
    /// Wyszukuje encje UserEvent na podstawie predykatu (warunków).
    /// </summary>
    /// <param name="predicate">Predykat, który reprezentuje warunki filtrowania.</param>
    /// <returns>Lista encji UserEvent, które spełniają warunki predykatu.</returns>
    Task<IEnumerable<UserEvent>> FindAsync(Expression<Func<UserEvent, bool>> predicate);
  }
}
