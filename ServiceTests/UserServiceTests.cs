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
  public class UserServiceTests
  {
    private Mock<IUserRepository> mockRepository;
    private IUserService userService;

    [SetUp]
    public void Setup()
    {
      mockRepository = new Mock<IUserRepository>();
      userService = new UserService(mockRepository.Object);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_user_when_valid_id()
    {
      var expected = new User
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "Johnny",
        LastName = "Deep",
      };
      mockRepository.Setup(s => s.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

      var result = await userService.GetByIdAsync(expected.Id);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdAsync_Should_throw_exception_when_user_not_found()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User)null);

      var action = async () => await userService.GetByIdAsync(userId);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono użytkownika o id: {userId}.");
    }

    [Test]
    public async Task GetAllAsync_Should_return_users_with_pagination()
    {
      var expected = new List<User>
      {
          new User
          {
            Id = Guid.Parse("00000000000000000000000000000001"),
            Name = "Johnny",
            LastName = "Deep",
          },
          new User
          {
            Id = Guid.Parse("00000000000000000000000000000002"),
            Name = "Tom",
            LastName = "Holland",
          }
      };
      mockRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(expected);

      var result = await userService.GetAllAsync();

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindAsync_Should_return_matching_users()
    {
      var users = new List<User>
      {
        new User { Id =Guid.Parse("00000000000000000000000000000001"), Name = "Adam", LastName = "Kowalski" },
        new User { Id = Guid.Parse("00000000000000000000000000000002"), Name = "Jan", LastName = "Kowalski" },
        new User { Id = Guid.Parse("00000000000000000000000000000003"), Name = "Ewa", LastName = "Nowak" }
      };
      var expected = new List<User>
      {
        new User { Id =Guid.Parse("00000000000000000000000000000001"), Name = "Adam", LastName = "Kowalski" },
        new User { Id = Guid.Parse("00000000000000000000000000000002"), Name = "Jan", LastName = "Kowalski" },
      };

      mockRepository
        .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
        .ReturnsAsync((Expression<Func<User, bool>> predicate) => users.AsQueryable().Where(predicate).ToList());

      var result = await userService.FindAsync(u => u.LastName == "Kowalski");

      result.Should().NotBeNull();
      result.Should().HaveCount(2);
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindAsync_Should_return_empty_list_when_no_users_match()
    {
      var users = new List<User>
      {
        new User { Id =Guid.Parse("00000000000000000000000000000001"), Name = "Adam", LastName = "Kowalski" },
        new User { Id = Guid.Parse("00000000000000000000000000000002"), Name = "Jan", LastName = "Kowalski" },
        new User { Id = Guid.Parse("00000000000000000000000000000003"), Name = "Ewa", LastName = "Nowak" }
      };
      mockRepository
        .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
        .ReturnsAsync((Expression<Func<User, bool>> predicate) => users.AsQueryable().Where(predicate).ToList());

      var result = await userService.FindAsync(u => u.LastName == "Solecki");

      result.Should().NotBeNull();
      result.Should().BeEmpty();
    }

    [Test]
    public async Task AddAsync_Should_add_user()
    {
      var userToAdd = new User
      {
        Name = "Johnny",
        LastName = "Deep",
      };
      mockRepository.Setup(s => s.AddAsync(It.IsAny<User>()));

      await userService.AddAsync(userToAdd);

      mockRepository.Verify(s => s.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_user_data_has_not_name()
    {
      var invalidUser = new User()
      {
        LastName = "Deep",
      };

      var action = async () => await userService.AddAsync(invalidUser);

      var exception = await action.Should().ThrowAsync<IncorrectDataException>();
      exception.WithMessage("Pozycja 'Imię' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_user_data_has_not_lastname()
    {
      var invalidUser = new User()
      {
        Name = "Johnny",
      };

      var action = async () => await userService.AddAsync(invalidUser);

      var exception = await action.Should().ThrowAsync<IncorrectDataException>();
      exception.WithMessage("Pozycja 'Nazwisko' jest wymagana.");
    }

    [Test]
    public async Task UpdateAsync_Should_update_user()
    {
      var id = Guid.Parse("00000000000000000000000000000001");
      var userToUpdate = new User
      {
        Name = "Kamil",
        LastName = "Deep",
      };
      var existingUser = new User
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "Kamil",
        LastName = "Deep",
      };
      mockRepository.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(existingUser);
      mockRepository.Setup(s => s.UpdateAsync(It.IsAny<User>()));

      await userService.UpdateAsync(id, userToUpdate);

      mockRepository.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_Should_throw_exception_when_user_is_not_exists()
    {
      var id = Guid.Parse("00000000000000000000000000000001");
      var userToUpdate = new User
      {
        Name = "Kamil",
        LastName = "Deep",
      };
      mockRepository.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((User)null);
      mockRepository.Setup(s => s.UpdateAsync(It.IsAny<User>()));

      var action = async () => await userService.UpdateAsync(id, userToUpdate);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono użytkownika o id: {id}.");
    }

    [Test]
    public async Task DeleteAsync_Should_remove_user()
    {
      var userIdToDelete = Guid.Parse("00000000000000000000000000000001");

      await userService.DeleteAsync(userIdToDelete);

      mockRepository.Verify(s => s.DeleteAsync(userIdToDelete), Times.Once);
    }
  }
}
