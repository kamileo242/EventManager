using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
  public class AddressDtoExample : IExamplesProvider<AddressDto>
  {
    public AddressDto GetExamples()
      => new()
      {
        Id = "00000000000000000000000000000001",
        City = "Warszawa",
        Street = "Puławska",
        HouseNumber = "134"
      };
  }
}
