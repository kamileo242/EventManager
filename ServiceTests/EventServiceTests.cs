using System.Linq.Expressions;
using EventManager.Datalayer;
using EventManager.Domain;
using EventManager.Domain.Services;
using EventManager.Models;
using EventManager.Models.Exceptions;
using FluentAssertions;
using Moq;

namespace ServiceTests
{
  [TestFixture]
  public class EventServiceTests
  {
    private Mock<IEventRepository> mockRepository;
    private Mock<IAddressRepository> addressRepository;
    private IEventService eventService;

    [SetUp]
    public void Setup()
    {
      mockRepository = new Mock<IEventRepository>();
      addressRepository = new Mock<IAddressRepository>();
      eventService = new EventService(mockRepository.Object, addressRepository.Object);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_event_when_valid_id()
    {
      var expected = new Event
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };

      mockRepository.Setup(s => s.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

      var result = await eventService.GetByIdAsync(expected.Id);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdAsync_Should_throw_exception_when_event_not_found()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((Event)null);

      var action = async () => await eventService.GetByIdAsync(userId);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono wydarzenia o id: {userId}.");
    }

    [Test]
    public async Task GetAllAsync_Should_return_events_with_pagination()
    {
      var expected = new List<Event>
      {
        new Event
        {
          Id = Guid.Parse("00000000000000000000000000000001"),
          Name = "Sylwester",
          Description = "Impreza życia !",
          StartDate = DateTime.Parse("2025-12-12 20:00:00"),
          EndDate = DateTime.Parse("2026-01-01 04:00:00"),
          AddressId = Guid.Parse("00000000000000000000000000000001"),
          MaxParticipants = 50,
          Cost = 200
        },
        new Event
        {
          Id = Guid.Parse("00000000000000000000000000000001"),
          Name = "Urodziny",
          Description = "Moje 25 urodziny.",
          StartDate = DateTime.Parse("2025-07-04 20:00:00"),
          AddressId = Guid.Parse("00000000000000000000000000000001"),
          MaxParticipants = 20,
        }
      };

      mockRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(expected);

      var result = await eventService.GetAllAsync();

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindAsync_Should_return_matching_events()
    {
      var events = new List<Event>
      {
        new Event
        {
          Id = Guid.Parse("00000000000000000000000000000001"),
          Name = "Sylwester",
          Description = "Impreza życia !",
          StartDate = DateTime.Parse("2025-12-12 20:00:00"),
          EndDate = DateTime.Parse("2026-01-01 04:00:00"),
          AddressId = Guid.Parse("00000000000000000000000000000001"),
          MaxParticipants = 50,
          Cost = 200
        },
        new Event
        {
          Id = Guid.Parse("00000000000000000000000000000001"),
          Name = "Urodziny",
          Description = "Moje 25 urodziny.",
          StartDate = DateTime.Parse("2025-07-04 20:00:00"),
          AddressId = Guid.Parse("00000000000000000000000000000001"),
          MaxParticipants = 20,
        }
      };
      var expected = new List<Event>
      {
        new Event
        {
          Id = Guid.Parse("00000000000000000000000000000001"),
          Name = "Sylwester",
          Description = "Impreza życia !",
          StartDate = DateTime.Parse("2025-12-12 20:00:00"),
          EndDate = DateTime.Parse("2026-01-01 04:00:00"),
          AddressId = Guid.Parse("00000000000000000000000000000001"),
          MaxParticipants = 50,
          Cost = 200
        },
      };

      mockRepository
        .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Event, bool>>>()))
        .ReturnsAsync((Expression<Func<Event, bool>> predicate) => events.AsQueryable().Where(predicate).ToList());

      var result = await eventService.FindAsync(u => u.Name == "Sylwester");

      result.Should().NotBeNull();
      result.Should().HaveCount(1);
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindAsync_Should_return_empty_list_when_no_events_match()
    {
      var events = new List<Event>
      {
        new Event
        {
          Id = Guid.Parse("00000000000000000000000000000001"),
          Name = "Sylwester",
          Description = "Impreza życia !",
          StartDate = DateTime.Parse("2025-12-12 20:00:00"),
          EndDate = DateTime.Parse("2026-01-01 04:00:00"),
          AddressId = Guid.Parse("00000000000000000000000000000001"),
          MaxParticipants = 50,
          Cost = 200
        },
        new Event
        {
          Id = Guid.Parse("00000000000000000000000000000001"),
          Name = "Urodziny",
          Description = "Moje 25 urodziny.",
          StartDate = DateTime.Parse("2025-07-04 20:00:00"),
          AddressId = Guid.Parse("00000000000000000000000000000001"),
          MaxParticipants = 20,
        }
      };
      mockRepository
        .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Event, bool>>>()))
        .ReturnsAsync((Expression<Func<Event, bool>> predicate) => events.AsQueryable().Where(predicate).ToList());

      var result = await eventService.FindAsync(u => u.Name == "Dyskoteka");

      result.Should().NotBeNull();
      result.Should().BeEmpty();
    }

    [Test]
    public async Task AddAsync_Should_add_event()
    {
      var eventToAdd = new Event
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        MaxParticipants = 50,
        Cost = 200
      };
      mockRepository.Setup(s => s.AddAsync(It.IsAny<Event>()));

      await eventService.AddAsync(eventToAdd);

      mockRepository.Verify(s => s.AddAsync(It.IsAny<Event>()), Times.Once);
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_event_data_has_not_name()
    {
      var eventToAdd = new Event
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };

      var action = async () => await eventService.AddAsync(eventToAdd);

      var exception = await action.Should().ThrowAsync<IncorrectDataException>();
      exception.WithMessage("Pozycja 'Nazwa' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_invalid_address()
    {
      var invalidAddressId = Guid.Parse("00000000000000000000000000000001");
      var eventToAdd = new Event
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = invalidAddressId,
        MaxParticipants = 50,
        Cost = 200
      };
      addressRepository.Setup(s => s.GetByIdAsync(invalidAddressId)).ReturnsAsync((Address)null);

      var action = async () => await eventService.AddAsync(eventToAdd);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono wydarzenia o id: {invalidAddressId}.");
    }

    [Test]
    public async Task UpdateAsync_Should_update_event()
    {
      var id = Guid.Parse("00000000000000000000000000000001");
      var existingEvent = new Event
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };
      var eventToUpdate = new Event
      {
        Cost = 300
      };
      mockRepository.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(existingEvent);
      mockRepository.Setup(s => s.UpdateAsync(It.IsAny<Event>()));

      await eventService.UpdateAsync(id, eventToUpdate);

      mockRepository.Verify(s => s.UpdateAsync(It.IsAny<Event>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_Should_throw_exception_when_event_is_not_exists()
    {
      var id = Guid.Parse("00000000000000000000000000000001");
      var userToUpdate = new Event
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };
      mockRepository.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Event)null);
      mockRepository.Setup(s => s.UpdateAsync(It.IsAny<Event>()));

      var action = async () => await eventService.UpdateAsync(id, userToUpdate);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono wydarzenia o id: {id}.");
    }

    [Test]
    public async Task UpdateAsync_Should_Throw_exception_when_invalid_address()
    {
      var invalidAddressId = Guid.Parse("00000000000000000000000000000001");
      var eventToUpdate = new Event
      {
        AddressId = Guid.Parse("00000000000000000000000000000001"),
      };
      var existingEvent = new Event
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        MaxParticipants = 50,
        Cost = 200,
        AddressId = Guid.Parse("00000000000000000000000000000001"),
      };
      mockRepository.Setup(s => s.GetByIdAsync(invalidAddressId)).ReturnsAsync(existingEvent);
      mockRepository.Setup(s => s.UpdateAsync(It.IsAny<Event>()));
      addressRepository.Setup(s => s.GetByIdAsync(invalidAddressId)).ReturnsAsync((Address)null);

      var action = async () => await eventService.UpdateAsync(invalidAddressId, eventToUpdate);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono wydarzenia o id: {invalidAddressId}.");
    }

    [Test]
    public async Task DeleteAsync_Should_remove_event()
    {
      var eventIdToDelete = Guid.Parse("00000000000000000000000000000001");

      await eventService.DeleteAsync(eventIdToDelete);

      mockRepository.Verify(s => s.DeleteAsync(eventIdToDelete), Times.Once);
    }
  }
}
