using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManager.Datalayer
{
  /// <summary>
  /// Zbiór operacji wykonywanych na repozytorium obiektów.
  /// </summary>
  /// <typeparam name="T">Typ obiektów w repozytorium.</typeparam>
  public interface IRepository<TModel> where TModel : class
  {
    /// <summary>
    /// Pobiera obiekt na podstawie identyfikatora.
    /// </summary>
    /// <param name="id">Identyfikator obiektu.</param>
    /// <returns>Znaleziona encja</returns>
    Task<TModel> GetByIdAsync(Guid id);

    /// <summary>
    /// Zwraca kolekcję wszystkich obiektów.
    /// </summary>
    /// <returns>Kolekcja wszystkich obiektów.</returns>
    Task<IEnumerable<TModel>> GetAllAsync();

    /// <summary>
    /// Wyszukuje rekordy w bazie danych spełniające określony warunek.
    /// </summary>
    /// <param name="predicate">Wyrażenie warunkowe do filtrowania encji.</param>
    /// <returns>Lista encji spełniających warunek.</returns>
    Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate);

    /// <summary>
    /// Dodaje nowy rekord do bazy danych.
    /// </summary>
    /// <param name="entity">Encja do dodania.</param>
    Task AddAsync(TModel entity);

    /// <summary>
    /// Aktualizuje istniejący rekord w bazie danych.
    /// </summary>
    /// <param name="entity">Encja do zaktualizowania.</param>
    Task UpdateAsync(TModel entity);

    /// <summary>
    /// Usuwa rekord z bazy danych.
    /// </summary>
    /// <param name="id">Identyfikator encji do usunięcia.</param>
    Task DeleteAsync(Guid id);
  }
}
