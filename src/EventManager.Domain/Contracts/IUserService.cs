using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Models;

namespace EventManager.Domain
{
  /// <summary>
  /// Zbiór operacji wykonywanych na serwisie użytkownika
  /// </summary>
  public interface IUserService
  {
    /// <summary>
    /// Pobierz użytkownika po id.
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    /// <returns>Użytkownik</returns>
    Task<User> GetByIdAsync(Guid id);

    /// <summary>
    /// Pobierz wszytskich użytkowników.
    /// </summary>
    /// <returns>Lista użytkowników</returns>
    Task<List<User>> GetAllAsync();

    /// <summary>
    /// Pobierz listę użytkowników na podstawie wskazanej właściwości.
    /// </summary>
    /// <param name="predicate">Wyrażenie warunkowe do filtrowania użytkowników.</param>
    /// <returns>Lista użytkowników</returns>
    Task<List<User>> FindAsync(Expression<Func<User, bool>> predicate);

    /// <summary>
    /// Dodaj nowego użytkownika
    /// </summary>
    /// <param name="user">Użytkownik</param>
    Task AddAsync(User user);

    /// <summary>
    /// Zmień wybrane właściwości użytkownika
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    /// <param name="user">Dane użytkownika do modyfikacji</param>
    Task UpdateAsync(Guid id, User user);

    /// <summary>
    /// Usuń użytkownika
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    Task DeleteAsync(Guid id);
  }
}
