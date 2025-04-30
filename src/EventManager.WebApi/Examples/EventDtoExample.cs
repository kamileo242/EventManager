using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
  public class EventDtoExample : IExamplesProvider<EventDto>
  {
    public EventDto GetExamples()
      => new()
      {
        Id = "00000000000000000000000000000001",
        Name = "Sylwester",
        Description = "Impreza życia",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        MaxParticipants = 50,
        Cost = 200
      };
  }
}
