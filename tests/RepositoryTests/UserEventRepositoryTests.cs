using System.Linq.Expressions;
using EventManager.Datalayer;
using EventManager.Datalayer.Dbos;
using EventManager.Datalayer.Repositories;
using EventManager.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace RepositoryTests
{
  [TestFixture]
  public class UserEventRepositoryTests
  {
    private Mock<IDboConverter> mockDboConverter;
    private UserEventRepository userEventRepository;
    private EventManagerDbContext dbContext;

    [SetUp]
    public void Setup()
    {
      var options = new DbContextOptionsBuilder<EventManagerDbContext>()
                      .UseInMemoryDatabase(databaseName: "EventManagerDbTest")
                      .Options;

      dbContext = new EventManagerDbContext(options);
      mockDboConverter = new Mock<IDboConverter>();
      userEventRepository = new UserEventRepository(dbContext, mockDboConverter.Object);
    }

    [TearDown]
    public void TearDown()
    {
      dbContext.Database.EnsureDeleted();
      dbContext.Dispose();
    }

    [Test]
    public async Task GetAllAsync_Should_return_all_user_events()
    {
      var userEventId1 = new Guid("00000000-0000-0000-0000-000000000001");
      var userEventId2 = new Guid("00000000-0000-0000-0000-000000000002");
      var userEventDboList = new List<UserEventDbo>
        {
            new UserEventDbo { UserId = userEventId1, EventId = userEventId2, DepositPaid = 100, JoinedAt = DateTime.Parse("2024-10-10") },
            new UserEventDbo { UserId = userEventId2, EventId = userEventId1, DepositPaid = 200, JoinedAt = DateTime.Parse("2024-10-10") }
        };
      dbContext.AddRange(userEventDboList);
      dbContext.SaveChanges();

      mockDboConverter.Setup(d => d.Convert<UserEvent>(It.IsAny<UserEventDbo>()))
          .Returns((UserEventDbo dbo) => new UserEvent { UserId = dbo.UserId, EventId = dbo.EventId, DepositPaid = dbo.DepositPaid, JoinedAt = DateTime.Parse("2024-10-10") });

      var result = await userEventRepository.GetAllAsync();

      result.Should().NotBeNull();
      result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetAllAsync_Should_return_empty_list()
    {
      var result = await userEventRepository.GetAllAsync();

      result.Should().NotBeNull();
      result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAsync_Should_return_correct_user_event()
    {
      var userId = new Guid("00000000-0000-0000-0000-000000000001");
      var eventId = new Guid("00000000-0000-0000-0000-000000000002");
      var userEventDbo = new UserEventDbo { UserId = userId, EventId = eventId, DepositPaid = 150, JoinedAt = DateTime.Parse("2024-10-10") };
      dbContext.Add(userEventDbo);
      dbContext.SaveChanges();
      var expectedUserEvent = new UserEvent { UserId = userId, EventId = eventId, DepositPaid = 150, JoinedAt = DateTime.Parse("2024-10-10") };
      mockDboConverter.Setup(d => d.Convert<UserEvent>(userEventDbo)).Returns(expectedUserEvent);

      var result = await userEventRepository.GetAsync(userId, eventId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedUserEvent);
    }

    [Test]
    public async Task GetAsync_Should_return_null_when_invalid_userId()
    {
      var userId = new Guid("00000000-0000-0000-0000-000000000001");
      var invalidUserId = new Guid("00000000-0000-0000-0000-000000000003");
      var eventId = new Guid("00000000-0000-0000-0000-000000000002");
      var userEventDbo = new UserEventDbo { UserId = userId, EventId = eventId, DepositPaid = 150, JoinedAt = DateTime.Parse("2024-10-10") };
      dbContext.Add(userEventDbo);
      dbContext.SaveChanges();

      var result = await userEventRepository.GetAsync(invalidUserId, eventId);

      result.Should().BeNull();
    }

    [Test]
    public async Task GetAsync_Should_return_null_when_invalid_eventId()
    {
      var userId = new Guid("00000000-0000-0000-0000-000000000001");
      var invalidEventId = new Guid("00000000-0000-0000-0000-000000000003");
      var eventId = new Guid("00000000-0000-0000-0000-000000000002");
      var userEventDbo = new UserEventDbo { UserId = userId, EventId = eventId, DepositPaid = 150, JoinedAt = DateTime.Parse("2024-10-10") };
      dbContext.Add(userEventDbo);
      dbContext.SaveChanges();

      var result = await userEventRepository.GetAsync(userId, invalidEventId);

      result.Should().BeNull();
    }

    [Test]
    public async Task AddAsync_Should_add_user_event()
    {
      var userId = new Guid("00000000-0000-0000-0000-000000000010");
      var eventId = new Guid("00000000-0000-0000-0000-000000000011");
      var userEvent = new UserEvent { UserId = userId, EventId = eventId, DepositPaid = 100, JoinedAt = DateTime.Parse("2024-10-10") };
      var userEventDbo = new UserEventDbo { UserId = userId, EventId = eventId, DepositPaid = userEvent.DepositPaid, JoinedAt = userEvent.JoinedAt };
      mockDboConverter.Setup(d => d.Convert<UserEventDbo>(userEvent)).Returns(userEventDbo);
      await userEventRepository.AddAsync(userEvent);

      var result = await dbContext.UserEvents.FindAsync(userId, eventId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(userEventDbo);
    }

    [Test]
    public async Task UpdateDepositPaidAsync_Should_update_deposit()
    {
      var userId = new Guid("00000000-0000-0000-0000-000000000012");
      var eventId = new Guid("00000000-0000-0000-0000-000000000013");
      var userEventDbo = new UserEventDbo { UserId = userId, EventId = eventId, DepositPaid = 100, JoinedAt = DateTime.Parse("2024-10-10") };
      dbContext.Add(userEventDbo);
      dbContext.SaveChanges();

      await userEventRepository.UpdateDepositPaidAsync(userId, eventId, 50);

      var updatedUserEvent = await dbContext.UserEvents.FindAsync(userId, eventId);
      updatedUserEvent.DepositPaid.Should().Be(150);
    }

    [Test]
    public async Task DeleteAsync_Should_remove_user_event()
    {
      var userId = new Guid("00000000-0000-0000-0000-000000000014");
      var eventId = new Guid("00000000-0000-0000-0000-000000000015");
      var userEventDbo = new UserEventDbo { UserId = userId, EventId = eventId, DepositPaid = 100, JoinedAt = DateTime.Parse("2024-10-10") };
      dbContext.Add(userEventDbo);
      dbContext.SaveChanges();

      await userEventRepository.DeleteAsync(userId, eventId);

      var result = await dbContext.UserEvents.FindAsync(userId, eventId);
      result.Should().BeNull();
    }

    [Test]
    public async Task FindAsync_Should_return_matching_user_events()
    {
      var userId1 = new Guid("00000000-0000-0000-0000-000000000016");
      var eventId1 = new Guid("00000000-0000-0000-0000-000000000017");
      var userId2 = new Guid("00000000-0000-0000-0000-000000000018");

      var userEventDbo1 = new UserEventDbo { UserId = userId1, EventId = eventId1, DepositPaid = 100, JoinedAt = DateTime.Parse("2024-10-10") };
      var userEventDbo2 = new UserEventDbo { UserId = userId2, EventId = eventId1, DepositPaid = 200, JoinedAt = DateTime.Parse("2024-10-10") };
      dbContext.AddRange(userEventDbo1, userEventDbo2);
      dbContext.SaveChanges();

      var expectedUserEvent1 = new UserEvent { UserId = userId1, EventId = eventId1, DepositPaid = 100, JoinedAt = DateTime.Parse("2024-10-10") };
      var expectedUserEvent2 = new UserEvent { UserId = userId2, EventId = eventId1, DepositPaid = 200, JoinedAt = DateTime.Parse("2024-10-10") };

      mockDboConverter.Setup(d => d.Convert<UserEvent>(userEventDbo1)).Returns(expectedUserEvent1);
      mockDboConverter.Setup(d => d.Convert<UserEvent>(userEventDbo2)).Returns(expectedUserEvent2);
      mockDboConverter
          .Setup(d => d.ConvertExpression<UserEvent, UserEventDbo>(It.IsAny<Expression<Func<UserEvent, bool>>>()))
          .Returns((Expression<Func<UserEvent, bool>> expr) => (UserEventDbo ue) => ue.EventId == eventId1);

      var result = await userEventRepository.FindAsync(ue => ue.EventId == eventId1);

      result.Should().NotBeNull();
      result.Should().HaveCount(2);
    }
  }
}
