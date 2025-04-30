using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
  public class EventStoreDtoExample : IExamplesProvider<EventStoreDto>
  {
    public EventStoreDto GetExamples()
      => new()
      {
        Name = "Sylwester",
        Description = "Impreza życia",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        MaxParticipants = 50,
        Cost = 200
      };
  }
}
