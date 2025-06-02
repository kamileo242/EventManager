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
    [Route("api/userEvent")]
    [ApiController]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanego udziału w wydarzeniu w bazie danych.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
    public class UserEventController : ControllerBase
    {
        public const string GetUserEventByIdOperationType = "Pobranie udziału w wydarzeniu po identyfikatorze";
        public const string BrowseUserEventOperationType = "Pobranie udziału w wydarzeniu po wyszukiwanej frazie";
        public const string GetAllUserEventsByOperationType = "Pobranie wszystkich uczestnictwo w wydarzeniuów";
        public const string PostUserEventOperationType = "Dodanie udziału w wydarzeniu";
        public const string DeleteUserEventOperationType = "Usunięcie udziału w wydarzeniu";
        public const string PutUserEventOperationType = "Zmodyfikowanie udziału w wydarzeniu";

        private readonly IUserEventService userEventService;
        private readonly IDtoBuilder dtoBuilder;

        public UserEventController(IUserEventService userEventService, IDtoBuilder dtoBuilder)
        {
            this.userEventService = userEventService;
            this.dtoBuilder = dtoBuilder;
        }

        /// <summary>
        /// Pobiera uczestnictwo w wydarzeniu po identyfikatorze
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <param name="eventId">Identyfikator wydarzenia</param>
        /// <returns>Uczestnictwo w wydarzeniu o podanym identyfikatorze</returns>
        [HttpGet("user/{userId}/event{eventId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserEventDto), StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono udziału w wydarzeniu.")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserEventDtoExample))]
        [SwaggerOperation(GetUserEventByIdOperationType)]
        public async Task<IActionResult> GetById([Required] string userId, [Required] string eventId)
        {
            var result = await userEventService.GetAsync(userId.TextToGuid(), eventId.TextToGuid());

            return result == null
              ? NotFound()
              : Ok(await dtoBuilder.ConvertToUserEventDto(result));
        }

        /// <summary>
        /// Pobiera listę uczestnictw w wydarzeniu na podstawie wyszukiwanej frazy.
        /// </summary>
        /// <param name="query">Parametry wyszukiwania</param>
        /// <returns>Lista uczestnictw w wydarzeniu</returns>
        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<UserEventDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(BrowseUserEventOperationType)]
        public async Task<IActionResult> Search([FromQuery] UserEventQuery query)
        {
            var results = await userEventService.FindAsync(query.ToExpression());

            var dtos = await Task.WhenAll(results.Select(u => dtoBuilder.ConvertToUserEventDto(u)));

            return Ok(dtos);
        }

        /// <summary>
        /// Pobiera listę wszystkich uczestnictw w wydarzeniach
        /// </summary>
        /// <returns>Lista uczestnictw w wydarzeniach </returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<UserEventDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ListUserEventDtoExample))]
        [SwaggerOperation(GetAllUserEventsByOperationType)]
        public async Task<IActionResult> GetAll()
        {
            var results = await userEventService.GetAllAsync();

            var dtos = await Task.WhenAll(results.Select(u => dtoBuilder.ConvertToUserEventDto(u)));

            return Ok(dtos);
        }

        /// <summary>
        /// Dodaje uczestnictwo w wydarzeniu.
        /// </summary>
        /// <param name="userEventDto">Dane udziału w wydarzeniu</param>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserEventDto), StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(UserEventStoreDto), typeof(UserEventStoreDtoExample))]
        [SwaggerOperation(PostUserEventOperationType)]
        public async Task<IActionResult> Post([FromBody] UserEventStoreDto userEventDto)
        {
            await userEventService.AddAsync(userEventDto.ConvertToUserEvent());

            return NoContent();
        }

        /// <summary>
        /// Aktualizuje udział w wydarzeniu
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <param name="eventId">Identyfikator wydarzenia</param>
        /// <param name="depositPaid">Zapłacona cena za wydarzenie</param>
        [HttpPut("user/{userId}/event{eventId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserEventDto), StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(PutUserEventOperationType)]
        public async Task<IActionResult> Put([Required] string userId, [Required] string eventId, [Required] int depositPaid)
        {
            await userEventService.UpdateDepositPaidAsync(userId.TextToGuid(), eventId.TextToGuid(), depositPaid);

            return NoContent();
        }

        /// <summary>
        /// Usuwa udział w wydarzeniu.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <param name="eventId">Identyfikator wydarzenia.</param>
        [HttpDelete("user/{userId}/event{eventId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerOperation(DeleteUserEventOperationType)]
        public async Task<IActionResult> Delete([Required] string userId, [Required] string eventId)
        {
            await userEventService.DeleteAsync(userId.TextToGuid(), eventId.TextToGuid());

            return NoContent();
        }
    }
}
