using System.Linq.Expressions;
using EventManager.Models;

namespace EventManager.WebApi.Query
{
  public class UserQuery
  {
    public string? Name { get; set; }
    public string? LastName { get; set; }

    public Expression<Func<User, bool>> ToExpression()
    {
      return user =>
          (string.IsNullOrEmpty(Name) || user.Name.Contains(Name)) &&
          (string.IsNullOrEmpty(LastName) || user.LastName.Contains(LastName));
    }
  }
}
