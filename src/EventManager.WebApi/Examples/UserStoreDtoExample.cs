using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
  public class UserStoreDtoExample : IExamplesProvider<UserStoreDto>
  {
    public UserStoreDto GetExamples()
      => new()
      {
        Name = "Kamil",
        LastName = "Wiśniewski"
      };
  }
}
