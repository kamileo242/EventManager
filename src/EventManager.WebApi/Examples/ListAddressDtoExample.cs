using EventManager.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Examples
{
  public class ListAddressDtoExample : IExamplesProvider<List<AddressDto>>
  {
    public List<AddressDto> GetExamples()
      => new List<AddressDto>()
      {
        new AddressDto
        {
          Id = "00000000000000000000000000000001",
          City = "Warszawa",
          Street = "Puławska",
          HouseNumber = "134"
        },
        new AddressDto
        {
          Id = "00000000000000000000000000000002",
          City = "Łódź",
          Street = "Fabryczna",
          HouseNumber = "1"
        }
      };
  }
}
