using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Datalayer;
using EventManager.Domain.Providers;
using EventManager.Models;
using EventManager.Models.Exceptions;

namespace EventManager.Domain.Services
{
  public class AddressService : IAddressService
  {
    private readonly IAddressRepository repository;

    public AddressService(IAddressRepository repository)
    {
      this.repository = repository;
    }

    public async Task<Address> GetByIdAsync(Guid id)
    {
      var address = await repository.GetByIdAsync(id);

      if (address == null)
      {
        throw new MissingDataException($"Nie znaleziono adresu o id: {id}.");
      }

      return address;
    }

    public async Task<List<Address>> GetAllAsync()
    {
      var addresses = await repository.GetAllAsync();

      return addresses.ToList();
    }

    public async Task<List<Address>> FindAsync(Expression<Func<Address, bool>> predicate)
    {
      var addresses = await repository.FindAsync(predicate);
      return addresses.ToList();
    }

    public async Task AddAsync(Address address)
    {
      await ValidateStoreAddress(address);

      address.Id = GuidProvider.GenetareGuid();

      await repository.AddAsync(address);
    }

    public async Task UpdateAsync(Guid id, Address address)
    {
      var existingAddress = await repository.GetByIdAsync(id);
      if (existingAddress == null)
      {
        throw new MissingDataException($"Nie znaleziono adresu o id: {id}.");
      }
      address.Id = id;

      await repository.UpdateAsync(address);
    }

    public async Task DeleteAsync(Guid id)
      => await repository.DeleteAsync(id);

    private async Task ValidateStoreAddress(Address address)
    {
      if (string.IsNullOrWhiteSpace(address?.City))
      {
        throw new IncorrectDataException("Pozycja 'Miasto' jest wymagana.");
      }

      if (string.IsNullOrWhiteSpace(address?.Street))
      {
        throw new IncorrectDataException("Pozycja 'Ulica' jest wymagana.");
      }

      if (string.IsNullOrWhiteSpace(address?.HouseNumber))
      {
        throw new IncorrectDataException("Pozycja 'Numer domu' jest wymagana.");
      }
    }
  }
}
