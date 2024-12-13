using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GamesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame(
            [FromQuery] int pageSize = 20, 
            [FromQuery] int page = 1)
        {
            if (pageSize > 100) pageSize = 100;
            var gameDtos = await _serviceManager.GameService.GetAllAsync(pageSize, page);

            var totalItemsInDb = await _serviceManager.GameService.Count();
            var totalPages = Math.Ceiling((double)totalItemsInDb / pageSize);
            Response.Headers.Append("X-Total-Pages", totalPages.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());
            Response.Headers.Append("X-Current-Page", page.ToString());
            Response.Headers.Append("X-Total-Items", totalItemsInDb.ToString());
            return Ok(gameDtos);
        }

        // GET: api/Games/
        [HttpGet("{title}")]
        public async Task<ActionResult<GameDto>> GetGame(string title)
        {
            var gameDto = await _serviceManager.GameService.GetAsync(title);
            if (gameDto == null) return Problem(
                detail: $"Game with title {title} not found",
                statusCode: StatusCodes.Status404NotFound
            );
            return Ok(gameDto);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id) return Problem(
                detail: $"Game id {game.Id} doesn't match provided id {id}",
                statusCode: StatusCodes.Status400BadRequest
            );

            try
            {
                _serviceManager.GameService.Update(game);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _serviceManager.TournamentService.ExistsAsync(id))
                {
                    return Problem(
                        detail: $"Game with id {id} not found",
                        statusCode: StatusCodes.Status404NotFound
                    );

                }

                throw;
            }

            var gameDto = await _serviceManager.GameService.GetAsync(id);
            return Ok(gameDto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
        {
            if (!ModelState.IsValid) return Problem(
                detail: $"Game couldn't be posted, modelstate: {ModelState}",
                statusCode: StatusCodes.Status400BadRequest
            );
            //return BadRequest(ModelState);

            var g = await _serviceManager.GameService.Add(gameDto);
            return CreatedAtAction("GetGame", new {id = g.Id}, gameDto);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var exists = await _serviceManager.GameService.ExistsAsync(id);
            if (!exists) return Problem(
                detail: $"Game with id {id} not found",
                statusCode: StatusCodes.Status404NotFound
            );


            _serviceManager.GameService.Remove(id);

            return NoContent();
        }
    }
}