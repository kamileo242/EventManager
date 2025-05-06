using System.Linq.Expressions;
using EventManager.Models;

namespace EventManager.WebApi.Query
{
  /// <summary>
  /// Model zawierający właściwości do wyszukania użytkownika.
  /// </summary>
  public class UserQuery
  {
    /// <summary>
    /// Imię.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Nazwisko.
    /// </summary>
    public string? LastName { get; set; }

    public Expression<Func<User, bool>> ToExpression()
    {
      return user =>
          (string.IsNullOrEmpty(Name) || user.Name.Contains(Name)) &&
          (string.IsNullOrEmpty(LastName) || user.LastName.Contains(LastName));
    }
  }
}
