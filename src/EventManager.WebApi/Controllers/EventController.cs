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
    [Route("api/event")]
    [ApiController]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanego wydarzenia w bazie danych.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
    public class EventController : ControllerBase
    {
        public const string GetEventByIdOperationType = "Pobranie wydarzenia po identyfikatorze";
        public const string BrowseEventOperationType = "Pobranie wydarzenia po wyszukiwanej frazie";
        public const string GetAllEventsByOperationType = "Pobranie wszystkich wydarzeń";
        public const string PostEventOperationType = "Dodanie wydarzenia";
        public const string DeleteEventOperationType = "Usunięcie wydarzenia";
        public const string PutEventOperationType = "Zmodyfikowanie wydarzenia";
        public const string PutAddressOperationType = "Zmodyfikowanie adresu wydarzenia";

        private readonly IEventService eventService;
        private readonly IDtoBuilder dtoBuilder;

        public EventController(IEventService eventService, IDtoBuilder dtoBuilder)
        {
            this.eventService = eventService;
            this.dtoBuilder = dtoBuilder;
        }

        /// <summary>
        /// Pobiera wydarzenie po identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator wydarzenia</param>
        /// <returns>Wydarzenie o podanym identyfikatorze</returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono wydarzenia.")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventDtoExample))]
        [SwaggerOperation(GetEventByIdOperationType)]
        public async Task<IActionResult> GetById([Required] string id)
        {
            var result = await eventService.GetByIdAsync(id.TextToGuid());

            return result == null
              ? NotFound()
              : Ok(await dtoBuilder.ConvertToEventDto(result));
        }

        /// <summary>
        /// Pobiera listę wydarzeń na podstawie wyszukiwanej frazy.
        /// </summary>
        /// <param name="query">Parametry wyszukiwania</param>
        /// <returns>Lista wydarzeń.</returns>
        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<EventDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(BrowseEventOperationType)]
        public async Task<IActionResult> Search([FromQuery] EventQuery query)
        {
            var events = await eventService.FindAsync(query.ToExpression());

            var dtos = await Task.WhenAll(events.Select(u => dtoBuilder.ConvertToEventDto(u)));

            return Ok(dtos);
        }

        /// <summary>
        /// Pobiera listę wszystkich wydarzeń
        /// </summary>
        /// <returns>Lista wydarzeń</returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<EventDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ListEventDtoExample))]
        [SwaggerOperation(GetAllEventsByOperationType)]
        public async Task<IActionResult> GetAll()
        {
            var events = await eventService.GetAllAsync();

            var dtos = await Task.WhenAll(events.Select(u => dtoBuilder.ConvertToEventDto(u)));

            return Ok(dtos);
        }

        /// <summary>
        /// Dodaje wydarzenie
        /// </summary>
        /// <param name="userDto">Dane wydarzenia</param>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(EventStoreDto), typeof(EventStoreDtoExample))]
        [SwaggerOperation(PostEventOperationType)]
        public async Task<IActionResult> Post([FromBody] EventStoreDto userDto)
        {
            await eventService.AddAsync(userDto.ConvertToEvent());

            return NoContent();
        }

        /// <summary>
        /// Aktualizuje wydarzenie
        /// </summary>
        /// <param name="id">Identyfikator wydarzenia do zmiany</param>
        /// <param name="userDto">Dane wydarzenia</param>
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(EventStoreDto), typeof(EventStoreDtoExample))]
        [SwaggerOperation(PutEventOperationType)]
        public async Task<IActionResult> Put([Required] string id, [FromBody] EventStoreDto userDto)
        {
            await eventService.UpdateAsync(id.TextToGuid(), userDto.ConvertToEvent());

            return NoContent();
        }

        /// <summary>
        /// Aktualizuje adres wydarzenia
        /// </summary>
        /// <param name="eventId">Identyfikator wydarzenia do zmiany</param>
        /// <param name="addressId">Identyfikator wydarzenia</param>
        [HttpPut("{eventId}/address/{addressId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(PutAddressOperationType)]
        public async Task<IActionResult> Put([Required] string eventId, [Required] string addressId)
        {
            await eventService.AddAddress(eventId.TextToGuid(), addressId.TextToGuid());

            return NoContent();
        }

        /// <summary>
        /// Usuwa wydarzenie
        /// </summary>
        /// <param name="id">Identyfikator wydarzenia</param>
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerOperation(DeleteEventOperationType)]
        public async Task<IActionResult> Delete([Required] string id)
        {
            await eventService.DeleteAsync(id.TextToGuid());

            return NoContent();
        }
    }
}
