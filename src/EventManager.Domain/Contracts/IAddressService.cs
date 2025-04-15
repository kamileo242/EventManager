using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManager.Models;

namespace EventManager.Domain
{
  /// <summary>
  /// Zbiór operacji wykonywanych na serwisie adresu.
  /// </summary>
  public interface IAddressService
  {
    /// <summary>
    /// Pobierz adres po id.
    /// </summary>
    /// <param name="id">Identyfikator adresu</param>
    /// <returns>Adres</returns>
    Task<Address> GetByIdAsync(Guid id);

    /// <summary>
    /// Pobierz wszytskie adresy.
    /// </summary>
    /// <returns>Lista adresów</returns>
    Task<List<Address>> GetAllAsync();

    /// <summary>
    /// Dodaj nowy adres.
    /// </summary>
    /// <param name="address">Adres</param>
    Task AddAsync(Address address);

    /// <summary>
    /// Zmień wybrane właściwości adresu.
    /// </summary>
    /// <param name="address">Dane adresu do modyfikacji</param>
    Task UpdateAsync(Address address);

    /// <summary>
    /// Usuń adres.
    /// </summary>
    /// <param name="id">Identyfikator adresu</param>
    Task DeleteAsync(Guid id);
  }
}
