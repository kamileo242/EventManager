using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using EventManager.Domain;
using EventManager.WebApi.Converts;
using EventManager.WebApi.Dtos;
using EventManager.WebApi.Examples;
using EventManager.WebApi.Query;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi.Controllers
{
    [Route("api/address")]
    [ApiController]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanego adresu w bazie danych.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
    public class AddressController : ControllerBase
    {
        public const string GetAddressByIdOperationType = "Pobranie adresu po identyfikatorze";
        public const string BrowseAddressOperationType = "Pobranie adresu po wyszukiwanej frazie";
        public const string GetAllAddresssByOperationType = "Pobranie wszystkich adresów";
        public const string PostAddressOperationType = "Dodanie adresu";
        public const string DeleteAddressOperationType = "Usunięcie adresu";
        public const string PutAddressOperationType = "Zmodyfikowanie adresu";

        private readonly IAddressService addressService;
        private readonly IDtoBuilder dtoBuilder;

        public AddressController(IAddressService addressService, IDtoBuilder dtoBuilder)
        {
            this.addressService = addressService;
            this.dtoBuilder = dtoBuilder;
        }

        /// <summary>
        /// Pobiera adres po identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator adresu</param>
        /// <returns>Adres o podanym identyfikatorze</returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono adresu.")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddressDtoExample))]
        [SwaggerOperation(GetAddressByIdOperationType)]
        public async Task<IActionResult> GetById([Required] string id)
        {
            var result = await addressService.GetByIdAsync(id.TextToGuid());

            return result == null
              ? NotFound()
              : Ok(await dtoBuilder.ConvertToAddressDto(result));
        }

        /// <summary>
        /// Pobiera listę adresów na podstawie wyszukwanej frazy.
        /// </summary>
        /// <param name="query">Parametry wyszukiwania.</param>
        /// <returns>Lista wydarzeń</returns>
        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<AddressDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(BrowseAddressOperationType)]
        public async Task<IActionResult> Search([FromQuery] AddressQuery query)
        {
            var results = await addressService.FindAsync(query.ToExpression());

            var dtos = await Task.WhenAll(results.Select(u => dtoBuilder.ConvertToAddressDto(u)));

            return Ok(dtos);
        }

        /// <summary>
        /// Pobiera listę wszystkich adresów
        /// </summary>
        /// <returns>Lista adresów</returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<AddressDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ListAddressDtoExample))]
        [SwaggerOperation(GetAllAddresssByOperationType)]
        public async Task<IActionResult> GetAll()
        {
            var results = await addressService.GetAllAsync();

            var dtos = await Task.WhenAll(results.Select(u => dtoBuilder.ConvertToAddressDto(u)));

            return Ok(dtos);
        }

        /// <summary>
        /// Dodaje adres.
        /// </summary>
        /// <param name="addressDto">Dane adresu</param>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(AddressStoreDto), typeof(AddressStoreDtoExample))]
        [SwaggerOperation(PostAddressOperationType)]
        public async Task<IActionResult> Post([FromBody] AddressStoreDto addressDto)
        {
            await addressService.AddAsync(addressDto.ConvertToAddress());

            return NoContent();
        }

        /// <summary>
        /// Aktualizuje adres
        /// </summary>
        /// <param name="id">Identyfikator adresu do zmiany</param>
        /// <param name="addressDto">Dane adres</param>
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(AddressStoreDto), typeof(AddressStoreDtoExample))]
        [SwaggerOperation(PutAddressOperationType)]
        public async Task<IActionResult> Put([Required] string id, [FromBody] AddressStoreDto addressDto)
        {
            await addressService.UpdateAsync(id.TextToGuid(), addressDto.ConvertToAddress());

            return NoContent();
        }

        /// <summary>
        /// Usuwa adresu
        /// </summary>
        /// <param name="id">Identyfikator adresu</param>
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerOperation(DeleteAddressOperationType)]
        public async Task<IActionResult> Delete([Required] string id)
        {
            await addressService.DeleteAsync(id.TextToGuid());

            return NoContent();
        }
    }
}
