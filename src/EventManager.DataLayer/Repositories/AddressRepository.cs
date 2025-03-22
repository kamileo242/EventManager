using EventManager.Datalayer.Dbos;
using EventManager.Models;

namespace EventManager.Datalayer.Repositories
{
  public class AddressRepository : Repository<Address, AddressDbo>, IAddressRepository
  {
    public AddressRepository(EventManagerDbContext context, IDboConverter dboConverter)
      : base(context, dboConverter)
    {
    }
  }
}
