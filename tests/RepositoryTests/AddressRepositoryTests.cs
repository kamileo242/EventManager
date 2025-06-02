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
    public class AddressRepositoryTests
    {
        private Mock<IDboConverter> mockDboConverter;
        private AddressRepository addressRepository;
        private EventManagerDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EventManagerDbContext>()
                            .UseInMemoryDatabase(databaseName: "EventManagerDbTest")
                            .Options;

            dbContext = new EventManagerDbContext(options);

            mockDboConverter = new Mock<IDboConverter>();
            addressRepository = new AddressRepository(dbContext, mockDboConverter.Object);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllAsync_Should_returns_addresses()
        {
            var addressId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var addressId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");

            var addressDboList = new List<AddressDbo>
      {
        new AddressDbo
        {
          Id = addressId1,
          City = "Warszawa",
          Street = "Czerniakowska",
          HouseNumber = "4",
        },
        new AddressDbo
        {
          Id = addressId2,
          City = "Łódź",
          Street = "Fabryczna",
          HouseNumber = "22/7"
        }
      };
            dbContext.AddRange(addressDboList);
            dbContext.SaveChanges();

            var expected = new List<Address>
      {
        new Address
        {
            Id = addressId1,
            Street = "Czerniakowska",
            City = "Warszawa",
            HouseNumber = "4",
        },
        new Address
        {
            Id = addressId2,
            Street = "Fabryczna",
            City = "Łódź",
            HouseNumber = "22/7",
        }
      };
            mockDboConverter
                .Setup(d => d.Convert<Address>(It.IsAny<AddressDbo>()))
                .Returns((AddressDbo dbo) =>
                {
                    return new Address
                    {
                        Id = dbo.Id,
                        Street = dbo.Street,
                        City = dbo.City,
                        HouseNumber = dbo.HouseNumber,
                    };
                });

            var result = await addressRepository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllAsync_Should_returns_empty_list()
        {
            var result = await addressRepository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetByIdAsync_Should_return_address()
        {
            var addressId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var addressDbo = new AddressDbo
            {
                Id = addressId,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };
            var expected = new Address
            {
                Id = addressId,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };
            dbContext.Add(addressDbo);
            dbContext.SaveChanges();

            mockDboConverter.Setup(d => d.Convert<Address>(addressDbo)).Returns(expected);

            var result = await addressRepository.GetByIdAsync(addressId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_Should_return_null_when_address_is_not_exist()
        {
            var addressId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var result = await addressRepository.GetByIdAsync(addressId);

            result.Should().BeNull();
        }

        [Test]
        public async Task AddAsync_Should_add_address()
        {
            var addressId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var addressDbo = new AddressDbo
            {
                Id = addressId,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };
            var address = new Address
            {
                Id = addressId,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };

            mockDboConverter.Setup(d => d.Convert<AddressDbo>(address)).Returns(addressDbo);

            await addressRepository.AddAsync(address);

            var result = await dbContext.Addresses.FindAsync(address.Id);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(result);
        }

        [Test]
        public async Task AddAsync_Should_throw_exception_when_address_is_null()
        {
            Func<Task> act = async () => await addressRepository.AddAsync(null);

            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'entity')");
        }

        [Test]
        public async Task UpdateAsync_Should_update_address()
        {
            var addressId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var addressDbo = new AddressDbo
            {
                Id = addressId,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };
            dbContext.Add(addressDbo);
            dbContext.SaveChanges();
            var updateAddress = new Address
            {
                Id = addressId,
                Street = "Taśmowa",
                HouseNumber = "8",
            };
            var updateAddressDbo = new AddressDbo
            {
                Id = addressId,
                Street = "Taśmowa",
                HouseNumber = "8",
            };
            var expected = new AddressDbo
            {
                Id = addressId,
                City = "Warszawa",
                Street = "Taśmowa",
                HouseNumber = "8",
            };

            mockDboConverter.Setup(d => d.Convert<AddressDbo>(updateAddress)).Returns(updateAddressDbo);

            await addressRepository.UpdateAsync(updateAddress);
            var result = await dbContext.Addresses.FindAsync(addressId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(result);
        }

        [Test]
        public async Task UpdateAsync_Should_throw_exception_when_address_is_not_exists()
        {
            var addressId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var updateAddress = new Address
            {
                Id = addressId,
                Street = "Taśmowa",
                HouseNumber = "8",
            };

            await addressRepository.UpdateAsync(updateAddress);

            dbContext.ChangeTracker.Entries()
                     .All(e => e.State == EntityState.Unchanged)
                     .Should().BeTrue();
        }

        [Test]
        public async Task DeleteAsync_Should_remove_address()
        {
            var addressId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var addressDbo = new AddressDbo
            {
                Id = addressId,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };
            dbContext.Add(addressDbo);
            dbContext.SaveChanges();

            await addressRepository.DeleteAsync(addressId);
            var result = await dbContext.Addresses.FindAsync(addressId);

            result.Should().BeNull();
            dbContext.ChangeTracker.Entries()
               .All(e => e.State == EntityState.Deleted)
               .Should().BeTrue();
        }

        [Test]
        public async Task DeleteAsync_Should_do_nothing_when_address_is_not_exists()
        {
            var addressId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            await addressRepository.DeleteAsync(addressId);

            dbContext.ChangeTracker.Entries()
                     .All(e => e.State == EntityState.Unchanged)
                     .Should().BeTrue();
        }

        [Test]
        public async Task FindAsync_Should_returns_all_addresses_with_the_same_city()
        {
            var addressId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var addressId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var addressDbo1 = new AddressDbo
            {
                Id = addressId1,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "4",
            };
            var expectedAddress1 = new Address
            {
                Id = addressId1,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "4",
            };
            var addressDbo2 = new AddressDbo
            {
                Id = addressId2,
                City = "Warszawa",
                Street = "Fabryczna",
                HouseNumber = "22/7",
            };
            var expectedAddress2 = new Address
            {
                Id = addressId2,
                City = "Warszawa",
                Street = "Fabryczna",
                HouseNumber = "22/7",
            };
            dbContext.AddRange(addressDbo1, addressDbo2);
            dbContext.SaveChanges();
            mockDboConverter.Setup(d => d.Convert<Address>(addressDbo1)).Returns(expectedAddress1);
            mockDboConverter.Setup(d => d.Convert<Address>(addressDbo2)).Returns(expectedAddress2);
            mockDboConverter
                .Setup(d => d.ConvertExpression<Address, AddressDbo>(It.IsAny<Expression<Func<Address, bool>>>()))
                .Returns((Expression<Func<Address, bool>> expr) =>
                {
                    Expression<Func<AddressDbo, bool>> convertedExpression = dbo => dbo.City == "Warszawa";
                    return convertedExpression;
                });
            var expectedAddresses = new List<Address> { expectedAddress1, expectedAddress2 };

            var result = await addressRepository.FindAsync(a => a.City == "Warszawa");

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedAddresses);
        }

        [Test]
        public async Task FindAsync_Should_returns_empty_list_when_adress_is_not_match()
        {
            var addressId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var addressId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");

            var addressDbo1 = new AddressDbo
            {
                Id = addressId1,
                City = "Łódź",
                Street = "Czerniakowska",
                HouseNumber = "4",
            };
            var addressDbo2 = new AddressDbo
            {
                Id = addressId2,
                City = "Kraków",
                Street = "Fabryczna",
                HouseNumber = "22/7",
            };
            dbContext.AddRange(addressDbo1, addressDbo2);
            dbContext.SaveChanges();
            mockDboConverter
                .Setup(d => d.ConvertExpression<Address, AddressDbo>(It.IsAny<Expression<Func<Address, bool>>>()))
                .Returns((Expression<Func<Address, bool>> expr) =>
                {
                    Expression<Func<AddressDbo, bool>> convertedExpression = dbo => dbo.City == "Warszawa";
                    return convertedExpression;
                });

            var result = await addressRepository.FindAsync(a => a.City == "Warszawa");

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
