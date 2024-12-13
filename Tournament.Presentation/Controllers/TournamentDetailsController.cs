using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TournamentDetailsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails(
            [FromQuery] int pageSize = 20,
            [FromQuery] int page = 1,
            [FromQuery] bool includeGames = false)
        {
            if (pageSize > 100) pageSize = 100;
            var tournamentDtos
                = includeGames
                    ? await _serviceManager.TournamentService.GetAllWithGamesAsync(pageSize, page)
                    : await _serviceManager.TournamentService.GetAllAsync(pageSize, page);

            var totalItemsInDb = await _serviceManager.TournamentService.Count();
            var totalPages = Math.Ceiling((double) totalItemsInDb / pageSize);
            Response.Headers.Append("X-Total-Pages", totalPages.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());
            Response.Headers.Append("X-Current-Page", page.ToString());
            Response.Headers.Append("X-Total-Items", totalItemsInDb.ToString());

            return Ok(tournamentDtos);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournamentDto = await _serviceManager.TournamentService.GetAsync(id);
            if (tournamentDto == null)
            {
                HttpContext.Response.ContentType = "Application/Json";
                return Problem(
                    detail: $"Tournament with id {id} not found",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            //var tournamentDto = _mapper.Map<TournamentDto>(tournamentDto);
            return Ok(tournamentDto);
        }

        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDetails tournament)
        {
            if (id != tournament.Id) return BadRequest();

            try
            {
                _serviceManager.TournamentService.Update(tournament);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _serviceManager.TournamentService.ExistsAsync(id))
                {
                    HttpContext.Response.ContentType = "Application/Json";
                    return Problem(
                        detail: $"Tournament with id {id} not found",
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                throw;
            }

            var tournamentDto = await _serviceManager.TournamentService.GetAsync(id);
            return Ok(tournamentDto);
        }

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDetails>> PostTournamentDetails(TournamentDto tournament)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            //_context.TournamentDetails.Add(tournament);
            //await _context.SaveChangesAsync();
            //_unitOfWork.TournamentRepository.Add(tournament);
            var t = _serviceManager.TournamentService.Add(tournament);
            return CreatedAtAction("GetTournamentDetails", new {id = t.Id}, tournament);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            //var tournamentDetails = await _context.TournamentDetails.FindAsync(id);
            //var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);
            //if (tournamentDetails == null)
            //{
            //    return NotFound();
            //}


            ////_context.TournamentDetails.Remove(tournamentDetails);
            ////await _context.SaveChangesAsync();
            //_unitOfWork.TournamentRepository.Remove(tournamentDetails);
            //_unitOfWork.CompleteAsync();

            var exists = await _serviceManager.TournamentService.ExistsAsync(id);
            if (!exists)
            {
                HttpContext.Response.ContentType = "Application/Json";
                return Problem(
                    detail: $"Tournament with id {id} not found",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _serviceManager.TournamentService.Remove(id);

            return NoContent();
        }
    }
}