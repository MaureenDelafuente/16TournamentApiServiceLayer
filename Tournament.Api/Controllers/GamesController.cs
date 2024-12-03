using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public GamesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<IEnumerable<Game>> GetGame()
        {
            return await _unitOfWork.GameRepository.GetAllAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            //var game = await _context.Game.FindAsync(id);
            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            //_context.Entry(game).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                _unitOfWork.GameRepository.Update(game);
                _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            //_context.Game.Add(game);
            //await _context.SaveChangesAsync();
            _unitOfWork.GameRepository.Add(game);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            //var game = await _context.Game.FindAsync(id);
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            //_context.Game.Remove(game);
            //await _context.SaveChangesAsync();
            _unitOfWork.GameRepository.Remove(game);
            _unitOfWork.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> GameExists(int id)
        {
            //return _context.Game.Any(e => e.Id == id);
            return await _unitOfWork.GameRepository.AnyAsync(id);
        }
    }
}
