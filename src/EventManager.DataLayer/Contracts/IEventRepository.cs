using EventManager.Models;

namespace EventManager.Datalayer
{
  /// <summary>
  /// Zbiór operacji wykonywanych na repozytorium wydarzenia.
  /// </summary>
  public interface IEventRepository : IRepository<Event>
  {
  }
}
