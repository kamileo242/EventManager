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
    [Route("api/user")]
    [ApiController]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanego użytkownika w bazie danych.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
    public class UserController : ControllerBase
    {
        public const string GetUserByIdOperationType = "Pobranie użytkownika po identyfikatorze";
        public const string BrowseUserOperationType = "Pobranie użytkownika po wyszukiwanej frazie";
        public const string GetAllUsersByOperationType = "Pobranie wszystkich użytkowników";
        public const string PostUserOperationType = "Dodanie użytkownika";
        public const string DeleteUserOperationType = "Usunięcie użytkownika";
        public const string PutUserOperationType = "Zmodyfikowanie użytkownika";

        private readonly IUserService userService;
        private readonly IDtoBuilder dtoBuilder;

        public UserController(IUserService userService, IDtoBuilder dtoBuilder)
        {
            this.userService = userService;
            this.dtoBuilder = dtoBuilder;
        }

        /// <summary>
        /// Pobiera użytkownika po identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator użytkownika</param>
        /// <returns>Użytkownik o podanym identyfikatorze</returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono użytkownika.")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserDtoExample))]
        [SwaggerOperation(GetUserByIdOperationType)]
        public async Task<IActionResult> GetById([Required] string id)
        {
            var result = await userService.GetByIdAsync(id.TextToGuid());

            return result == null
              ? NotFound()
              : Ok(await dtoBuilder.ConvertToUserDto(result));
        }

        /// <summary>
        /// Pobiera listę użytkowników na podstawie wyszukiwanej frazy.
        /// </summary>
        /// <param name="query">Parametry wyszukiwania</param>
        /// <returns>Lista użytkowników.</returns>
        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(BrowseUserOperationType)]
        public async Task<IActionResult> Search([FromQuery] UserQuery query)
        {
            var users = await userService.FindAsync(query.ToExpression());

            var dtos = await Task.WhenAll(users.Select(u => dtoBuilder.ConvertToUserDto(u)));

            return Ok(dtos);
        }

        /// <summary>
        /// Pobiera listę wszystkich użytkowników.
        /// </summary>
        /// <returns>Lista użytkowników</returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ListUserDtoExample))]
        [SwaggerOperation(GetAllUsersByOperationType)]
        public async Task<IActionResult> GetAll()
        {
            var users = await userService.GetAllAsync();

            var dtos = await Task.WhenAll(users.Select(u => dtoBuilder.ConvertToUserDto(u)));

            return Ok(dtos);
        }

        /// <summary>
        /// Dodaje użytkownika.
        /// </summary>
        /// <param name="userDto">Dane użytkownika</param>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(UserStoreDto), typeof(UserStoreDtoExample))]
        [SwaggerOperation(PostUserOperationType)]
        public async Task<IActionResult> Post([FromBody] UserStoreDto userDto)
        {
            await userService.AddAsync(userDto.ConvertToUser());

            return NoContent();
        }

        /// <summary>
        /// Aktualizuje użytkownika
        /// </summary>
        /// <param name="id">Identyfikator użytkownika do zmiany</param>
        /// <param name="userDto">Dane użytkownika</param>
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(UserStoreDto), typeof(UserStoreDtoExample))]
        [SwaggerOperation(PutUserOperationType)]
        public async Task<IActionResult> Put([Required] string id, [FromBody] UserStoreDto userDto)
        {
            await userService.UpdateAsync(id.TextToGuid(), userDto.ConvertToUser());

            return NoContent();
        }

        /// <summary>
        /// Usuwa użytkownika.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika</param>
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerOperation(DeleteUserOperationType)]
        public async Task<IActionResult> Delete([Required] string id)
        {
            await userService.DeleteAsync(id.TextToGuid());

            return NoContent();
        }
    }
}
