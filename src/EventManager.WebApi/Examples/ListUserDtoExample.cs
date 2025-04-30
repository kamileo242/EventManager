using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
  public class ListUserDtoExample : IExamplesProvider<List<UserDto>>
  {
    public List<UserDto> GetExamples()
      => new List<UserDto>()
      {
        new UserDto
        {
          Id = "00000000000000000000000000000001",
          Name = "Kamil",
          LastName = "Wiśniewski"
        },
        new UserDto
        {
          Id = "00000000000000000000000000000002",
          Name = "Tomek",
          LastName = "Nowak"
        }
      };
  }
}
