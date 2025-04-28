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
  public class AddressServiceTests
  {
    private Mock<IAddressRepository> mockRepository;
    private IAddressService addressService;

    [SetUp]
    public void Setup()
    {
      mockRepository = new Mock<IAddressRepository>();
      addressService = new AddressService(mockRepository.Object);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_address_when_valid_id()
    {
      var expected = new Address
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        City = "Warszawa",
        Street = "Puławska",
        HouseNumber = "134"
      };
      mockRepository.Setup(s => s.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

      var result = await addressService.GetByIdAsync(expected.Id);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdAsync_Should_throw_exception_when_address_not_found()
    {
      var addressId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(addressId)).ReturnsAsync((Address)null);

      var action = async () => await addressService.GetByIdAsync(addressId);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono adresu o id: {addressId}.");
    }

    [Test]
    public async Task GetAllAsync_Should_return_addresses()
    {
      var expected = new List<Address>
      {
          new Address
          {
            Id = Guid.Parse("00000000000000000000000000000001"),
            City = "Łódź",
            Street = "Fabrycznaa",
            HouseNumber = "15"
          },
          new Address
          {
            Id = Guid.Parse("00000000000000000000000000000002"),
            City = "Warszawa",
            Street = "Puławska",
            HouseNumber = "134"
          }
      };
      mockRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(expected);

      var result = await addressService.GetAllAsync();

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindAsync_Should_return_matching_addresses()
    {
      var addresses = new List<Address>
      {
        new Address { Id =Guid.Parse("00000000000000000000000000000001"), City = "Warszawa", Street = "Puławksa", HouseNumber = "15" },
        new Address { Id = Guid.Parse("00000000000000000000000000000002"), City = "Warszawa", Street = "Świętokrzyska", HouseNumber = "1" },
        new Address { Id = Guid.Parse("00000000000000000000000000000003"), City = "Łódź", Street = "Fabryczna", HouseNumber = "5"  }
      };
      var expected = new List<Address>
      {
        new Address { Id =Guid.Parse("00000000000000000000000000000001"), City = "Warszawa", Street = "Puławksa", HouseNumber = "15" },
        new Address { Id = Guid.Parse("00000000000000000000000000000002"), City = "Warszawa", Street = "Świętokrzyska", HouseNumber = "1" },
      };

      mockRepository
        .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Address, bool>>>()))
        .ReturnsAsync((Expression<Func<Address, bool>> predicate) => addresses.AsQueryable().Where(predicate).ToList());

      var result = await addressService.FindAsync(u => u.City == "Warszawa");

      result.Should().NotBeNull();
      result.Should().HaveCount(2);
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindAsync_Should_return_empty_list_when_no_addresses_match()
    {
      var addresses = new List<Address>
      {
        new Address { Id =Guid.Parse("00000000000000000000000000000001"), City = "Warszawa", Street = "Puławksa", HouseNumber = "15" },
        new Address { Id = Guid.Parse("00000000000000000000000000000002"), City = "Warszawa", Street = "Świętokrzyska", HouseNumber = "1" },
        new Address { Id = Guid.Parse("00000000000000000000000000000003"), City = "Łódź", Street = "Fabryczna", HouseNumber = "5"  }
      };
      mockRepository
        .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Address, bool>>>()))
        .ReturnsAsync((Expression<Func<Address, bool>> predicate) => addresses.AsQueryable().Where(predicate).ToList());

      var result = await addressService.FindAsync(u => u.Street == "Kraków");

      result.Should().NotBeNull();
      result.Should().BeEmpty();
    }

    [Test]
    public async Task AddAsync_Should_add_address()
    {
      var addressToAdd = new Address
      {
        City = "Łódź",
        Street = "Fabrycznaa",
        HouseNumber = "15"
      };
      mockRepository.Setup(s => s.AddAsync(It.IsAny<Address>()));

      await addressService.AddAsync(addressToAdd);

      mockRepository.Verify(s => s.AddAsync(It.IsAny<Address>()), Times.Once);
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_address_data_has_not_city()
    {
      var invalidAddress = new Address()
      {
        Street = "Fabrycznaa",
        HouseNumber = "15"
      };

      var action = async () => await addressService.AddAsync(invalidAddress);

      var exception = await action.Should().ThrowAsync<IncorrectDataException>();
      exception.WithMessage("Pozycja 'Miasto' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_address_data_has_not_street()
    {
      var invalidAddress = new Address()
      {
        City = "Łódź",
        HouseNumber = "15"
      };

      var action = async () => await addressService.AddAsync(invalidAddress);

      var exception = await action.Should().ThrowAsync<IncorrectDataException>();
      exception.WithMessage("Pozycja 'Ulica' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_address_data_has_not_houseNumber()
    {
      var invalidAddress = new Address()
      {
        City = "Łódź",
        Street = "Fabrycznaa",
      };

      var action = async () => await addressService.AddAsync(invalidAddress);

      var exception = await action.Should().ThrowAsync<IncorrectDataException>();
      exception.WithMessage("Pozycja 'Numer domu' jest wymagana.");
    }

    [Test]
    public async Task UpdateAsync_Should_update_address()
    {
      var id = Guid.Parse("00000000000000000000000000000001");
      var addressToUpdate = new Address
      {
        HouseNumber = "15"
      };
      var existingAddress = new Address
      {
        City = "Łódź",
        Street = "Fabrycznaa",
        HouseNumber = "20"
      };
      mockRepository.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(existingAddress);
      mockRepository.Setup(s => s.UpdateAsync(It.IsAny<Address>()));

      await addressService.UpdateAsync(id, addressToUpdate);

      mockRepository.Verify(s => s.UpdateAsync(It.IsAny<Address>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_Should_throw_exception_when_address_is_not_exists()
    {
      var id = Guid.Parse("00000000000000000000000000000001");
      var addressToUpdate = new Address
      {
        City = "Łódź",
        Street = "Fabrycznaa",
        HouseNumber = "15"
      };
      mockRepository.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Address)null);
      mockRepository.Setup(s => s.UpdateAsync(It.IsAny<Address>()));

      var action = async () => await addressService.UpdateAsync(id, addressToUpdate);

      var exception = await action.Should().ThrowAsync<MissingDataException>();
      exception.WithMessage($"Nie znaleziono adresu o id: {id}.");
    }

    [Test]
    public async Task DeleteAsync_Should_remove_address()
    {
      var addressIdToDelete = Guid.Parse("00000000000000000000000000000001");

      await addressService.DeleteAsync(addressIdToDelete);

      mockRepository.Verify(s => s.DeleteAsync(addressIdToDelete), Times.Once);
    }
  }
}
