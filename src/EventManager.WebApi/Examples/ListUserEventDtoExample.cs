using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
  public class ListUserEventDtoExample : IExamplesProvider<List<UserEventDto>>
  {
    public List<UserEventDto> GetExamples()
      => new List<UserEventDto>()
      {
        new UserEventDto
        {
          User = new UserDto
          {
            Id = "00000000000000000000000000000001",
            Name = "Kamil",
            LastName = "Wiśniewski"
          },
          Event = new EventDto
          {
            Id = "00000000000000000000000000000001",
            Name = "Sylwester",
            Description = "Impreza życia",
            StartDate = DateTime.Parse("2025-12-12 20:00:00"),
            EndDate = DateTime.Parse("2026-01-01 04:00:00"),
            MaxParticipants = 50,
            Cost = 200
          },
          JoinedAt = DateTime.Parse("2025-05-12 20:34:07"),
          DepositPaid = 50
        },
        new UserEventDto
        {
          User = new UserDto
          {
            Id = "00000000000000000000000000000001",
            Name = "Kamil",
            LastName = "Wiśniewski"
          },
          Event = new EventDto
          {
            Id = "00000000000000000000000000000002",
            Name = "Urodzinki moje",
            Description = "25 urodziny",
            StartDate = DateTime.Parse("2025-07-04 20:00:00"),
            MaxParticipants = 20
          },
          JoinedAt = DateTime.Parse("2025-05-12 20:34:07"),
        }
      };
  }
}
