using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
  public class AddressStoreDtoExample : IExamplesProvider<AddressStoreDto>
  {
    public AddressStoreDto GetExamples()
      => new()
      {
        City = "Warszawa",
        Street = "Puławska",
        HouseNumber = "134"
      };
  }
}
