using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
  public class UserDtoExample : IExamplesProvider<UserDto>
  {
    public UserDto GetExamples()
      => new()
      {
        Id = "00000000000000000000000000000001",
        Name = "Kamil",
        LastName = "Wiśniewski"
      };
  }
}
