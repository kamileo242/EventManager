using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
    public class UserEventStoreDtoExample : IExamplesProvider<UserEventStoreDto>
    {
        public UserEventStoreDto GetExamples()
          => new()
          {
              UserId = "00000000000000000000000000000001",
              EventId = "00000000000000000000000000000001"
          };
    }
}
