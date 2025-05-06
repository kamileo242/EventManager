using System.Linq.Expressions;
using EventManager.Models;

namespace EventManager.WebApi.Query
{
  /// <summary>
  /// Model zawierający właściwości do wyszukania adresu.
  /// </summary>
  public class AddressQuery
  {
    /// <summary>
    /// Miasto.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Ulica. 
    /// </summary>
    public string? Street { get; set; }

    /// <summary>
    /// Numer domu.
    /// </summary>
    public string? HouseNumber { get; set; }

    public Expression<Func<Address, bool>> ToExpression()
    {
      return address =>
          (string.IsNullOrEmpty(City) || address.City.Contains(City)) &&
          (string.IsNullOrEmpty(Street) || address.Street.Contains(Street)) &&
          (string.IsNullOrEmpty(HouseNumber) || address.HouseNumber.Contains(HouseNumber));
    }
  }
}
