using System.Linq.Expressions;
using EventManager.Datalayer;
using EventManager.Domain.Services;
using EventManager.Models;
using EventManager.Models.Exceptions;
using FluentAssertions;
using Moq;

namespace ServiceTests
{
  [TestFixture]
  public class UserEventServiceTests
  {
    private Mock<IUserRepository> userRepositoryMock;
    private Mock<IEventRepository> eventRepositoryMock;
    private Mock<IUserEventRepository> userEventRepositoryMock;

    private UserEventService service;

    [SetUp]
    public async Task SetUp()
    {
      userRepositoryMock = new Mock<IUserRepository>();
      eventRepositoryMock = new Mock<IEventRepository>();
      userEventRepositoryMock = new Mock<IUserEventRepository>();

      service = new UserEventService(
          userRepositoryMock.Object,
          eventRepositoryMock.Object,
          userEventRepositoryMock.Object
      );
    }

    [Test]
    public async Task GetAsync_Should_return_userEvent()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var expected = new UserEvent { UserId = userId, EventId = eventId, DepositPaid = 50, JoinedAt = DateTime.Parse("2025-04-01") };
      userEventRepositoryMock.Setup(r => r.GetAsync(userId, eventId)).ReturnsAsync(expected);

      var result = await service.GetAsync(userId, eventId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetAsync_Should_throw_exception_when_userEvent_do_not_exists()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      userEventRepositoryMock.Setup(r => r.GetAsync(userId, eventId)).ReturnsAsync((UserEvent)null);

      var action = async () => await service.GetAsync(userId, eventId);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono uczestnictwa użytkownika o id: {userId} w wydarzeniu o id: {eventId}.");
    }

    [Test]
    public async Task GetAllAsync_Should_return_all_existing_userEvents()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var userId1 = Guid.Parse("00000000000000000000000000000002");
      var eventId1 = Guid.Parse("00000000000000000000000000000002");
      var expected = new List<UserEvent>
      {
        new UserEvent { UserId = userId, EventId = eventId, DepositPaid = 50, JoinedAt = DateTime.Parse("2025-04-01") },
        new UserEvent { UserId = userId1, EventId = eventId1, DepositPaid = 100, JoinedAt = DateTime.Parse("2025-03-15") },
      };
      userEventRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

      var result = await service.GetAllAsync();

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindAsync_Should_return_matching_userEvents()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var userId1 = Guid.Parse("00000000000000000000000000000002");
      var eventId1 = Guid.Parse("00000000000000000000000000000002");
      var userEvents = new List<UserEvent>
      {
        new UserEvent { UserId = userId, EventId = eventId, DepositPaid = 50, JoinedAt = DateTime.Parse("2025-04-01") },
        new UserEvent { UserId = userId1, EventId = eventId1, DepositPaid = 100, JoinedAt = DateTime.Parse("2025-03-15") },
      };
      var expected = new List<UserEvent>
      {
        new UserEvent { UserId = userId, EventId = eventId, DepositPaid = 50, JoinedAt = DateTime.Parse("2025-04-01") },
      };
      userEventRepositoryMock
      .Setup(r => r.FindAsync(It.IsAny<Expression<Func<UserEvent, bool>>>()))
      .ReturnsAsync((Expression<Func<UserEvent, bool>> predicate) => userEvents.AsQueryable().Where(predicate).ToList());

      var result = await service.FindAsync(u => u.DepositPaid == 50);

      result.Should().NotBeNull();
      result.Should().HaveCount(1);
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindAsync_Should_return_nothing_when_any_userEvents_do_not_match()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var userId1 = Guid.Parse("00000000000000000000000000000002");
      var eventId1 = Guid.Parse("00000000000000000000000000000002");
      var userEvents = new List<UserEvent>
      {
        new UserEvent { UserId = userId, EventId = eventId, DepositPaid = 50, JoinedAt = DateTime.Parse("2025-04-01") },
        new UserEvent { UserId = userId1, EventId = eventId1, DepositPaid = 100, JoinedAt = DateTime.Parse("2025-03-15") },
      };
      userEventRepositoryMock
      .Setup(r => r.FindAsync(It.IsAny<Expression<Func<UserEvent, bool>>>()))
      .ReturnsAsync((Expression<Func<UserEvent, bool>> predicate) => userEvents.AsQueryable().Where(predicate).ToList());

      var result = await service.FindAsync(u => u.DepositPaid == 150);

      result.Should().NotBeNull();
      result.Should().BeEmpty();
    }

    [Test]
    public async Task AddAsync_Should_add_userEvent_when_valid_data()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var @event = new Event
      {
        Id = eventId,
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };
      var user = new User
      {
        Id = userId,
        Name = "Johnny",
        LastName = "Deep",
      };
      var userEvent = new UserEvent
      {
        UserId = userId,
        EventId = eventId,
        DepositPaid = 50,
        JoinedAt = DateTime.Parse("2025-04-01")
      };
      userRepositoryMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      eventRepositoryMock.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync(@event);
      userEventRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserEvent>()));

      await service.AddAsync(userEvent);

      userEventRepositoryMock.Verify(s => s.AddAsync(It.IsAny<UserEvent>()), Times.Once);
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_userEvent_is_null()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var @event = new Event
      {
        Id = eventId,
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };
      var user = new User
      {
        Id = userId,
        Name = "Johnny",
        LastName = "Deep",
      };
      var userEvent = new UserEvent
      {
        UserId = userId,
        EventId = eventId,
        DepositPaid = 50,
        JoinedAt = DateTime.Parse("2025-04-01")
      };
      userRepositoryMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      eventRepositoryMock.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync(@event);
      userEventRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserEvent>()));

      var action = async () => await service.AddAsync(null);

      var exception = await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_userId_is_null()
    {
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var @event = new Event
      {
        Id = eventId,
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };
      var userEvent = new UserEvent
      {
        EventId = eventId,
        DepositPaid = 50,
        JoinedAt = DateTime.Parse("2025-04-01")
      };
      eventRepositoryMock.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync(@event);
      userEventRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserEvent>()));

      var action = async () => await service.AddAsync(userEvent);

      var exception = await action.Should().ThrowAsync<IncorrectDataException>();
      exception.WithMessage("Nie podano identyfikatora użytkownika.");
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_eventId_is_null()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var user = new User
      {
        Id = userId,
        Name = "Johnny",
        LastName = "Deep",
      };
      var userEvent = new UserEvent
      {
        UserId = userId,
        DepositPaid = 50,
        JoinedAt = DateTime.Parse("2025-04-01")
      };
      userRepositoryMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      userEventRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserEvent>()));

      var action = async () => await service.AddAsync(userEvent);

      var exception = await action.Should().ThrowAsync<IncorrectDataException>();
      exception.WithMessage("Nie podano identyfikatora wydarzenia.");
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_user_is_not_exists()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var @event = new Event
      {
        Id = eventId,
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };
      var user = new User
      {
        Id = userId,
        Name = "Johnny",
        LastName = "Deep",
      };
      var userEvent = new UserEvent
      {
        UserId = userId,
        EventId = eventId,
        DepositPaid = 50,
        JoinedAt = DateTime.Parse("2025-04-01")
      };
      userRepositoryMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User)null);
      eventRepositoryMock.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync(@event);
      userEventRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserEvent>()));

      var action = async () => await service.AddAsync(userEvent);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono użytkownika o id {userId}.");
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_event_is_not_exists()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var @event = new Event
      {
        Id = eventId,
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };
      var user = new User
      {
        Id = userId,
        Name = "Johnny",
        LastName = "Deep",
      };
      var userEvent = new UserEvent
      {
        UserId = userId,
        EventId = eventId,
        DepositPaid = 50,
        JoinedAt = DateTime.Parse("2025-04-01")
      };
      userRepositoryMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      eventRepositoryMock.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync((Event)null);
      userEventRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserEvent>()));

      var action = async () => await service.AddAsync(userEvent);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono wydarzenia o id {userId}.");
    }

    [Test]
    public async Task UpdateDepositPaidAsync_Should_update_userEvent_when_valid_data()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var @event = new Event
      {
        Id = eventId,
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };
      var user = new User
      {
        Id = userId,
        Name = "Johnny",
        LastName = "Deep",
      };
      var userEvent = new UserEvent
      {
        UserId = userId,
        EventId = eventId,
        DepositPaid = 50,
        JoinedAt = DateTime.Parse("2025-04-01")
      };
      userRepositoryMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      eventRepositoryMock.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync(@event);
      userEventRepositoryMock.Setup(r => r.GetAsync(userId, eventId)).ReturnsAsync(userEvent);
      userEventRepositoryMock.Setup(r => r.UpdateDepositPaidAsync(userId, eventId, 50));

      await service.UpdateDepositPaidAsync(userId, eventId, 50);

      userEventRepositoryMock.Verify(s => s.UpdateDepositPaidAsync(userId, eventId, 50), Times.Once);
    }

    [Test]
    public async Task UpdateDepositPaidAsync_Should_throw_exception_when_userEvent_is_notExists()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");
      var @event = new Event
      {
        Id = eventId,
        Name = "Sylwester",
        Description = "Impreza życia !",
        StartDate = DateTime.Parse("2025-12-12 20:00:00"),
        EndDate = DateTime.Parse("2026-01-01 04:00:00"),
        AddressId = Guid.Parse("00000000000000000000000000000001"),
        MaxParticipants = 50,
        Cost = 200
      };
      var user = new User
      {
        Id = userId,
        Name = "Johnny",
        LastName = "Deep",
      };
      var userEvent = new UserEvent
      {
        UserId = userId,
        EventId = eventId,
        DepositPaid = 50,
        JoinedAt = DateTime.Parse("2025-04-01")
      };
      userRepositoryMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      eventRepositoryMock.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync(@event);
      userEventRepositoryMock.Setup(r => r.GetAsync(userId, eventId)).ReturnsAsync((UserEvent)null);
      userEventRepositoryMock.Setup(r => r.UpdateDepositPaidAsync(userId, eventId, 50));

      var action = async () => await service.UpdateDepositPaidAsync(userId, eventId, 50);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono uczestnictwa użytkownika o id: {userId} w wydarzeniu o id: {eventId}.");
    }

    [Test]
    public async Task DeleteAsync_Should_remove_userEvent()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var eventId = Guid.Parse("00000000000000000000000000000001");

      await service.DeleteAsync(userId, eventId);

      userEventRepositoryMock.Verify(s => s.DeleteAsync(userId, eventId), Times.Once);
    }
  }
}
