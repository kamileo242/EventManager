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
  public class UserService : IUserService
  {
    private readonly IUserRepository repository;

    public UserService(IUserRepository repository)
    {
      this.repository = repository;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
      var user = await repository.GetByIdAsync(id);

      if (user == null)
      {
        throw new MissingDataException($"Nie znaleziono użytkownika o id: {id}.");
      }

      return user;
    }

    public async Task<List<User>> GetAllAsync()
    {
      var users = await repository.GetAllAsync();

      return users.ToList();
    }

    public async Task<List<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
      var users = await repository.FindAsync(predicate);
      return users.ToList();
    }

    public async Task AddAsync(User user)
    {
      await ValidateStoreUser(user);

      user.Id = GuidProvider.GenetareGuid();

      await repository.AddAsync(user);
    }

    public async Task UpdateAsync(Guid id, User user)
    {
      var existingUser = await repository.GetByIdAsync(id);
      if (existingUser == null)
      {
        throw new MissingDataException($"Nie znaleziono użytkownika o id: {id}.");
      }
      user.Id = id;

      await repository.UpdateAsync(user);
    }

    public async Task DeleteAsync(Guid id)
      => await repository.DeleteAsync(id);

    private async Task ValidateStoreUser(User user)
    {
      if (string.IsNullOrWhiteSpace(user?.Name))
      {
        throw new IncorrectDataException("Pozycja 'Imię' jest wymagana.");
      }

      if (string.IsNullOrWhiteSpace(user?.LastName))
      {
        throw new IncorrectDataException("Pozycja 'Nazwisko' jest wymagana.");
      }
    }
  }
}
