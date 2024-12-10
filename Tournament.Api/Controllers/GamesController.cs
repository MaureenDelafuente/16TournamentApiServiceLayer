using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Tournament.Core.Dto;
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
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;

        public GamesController(IUnitOfWork unitOfWork, IMapper mapper, IServiceManager serviceManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _serviceManager = serviceManager;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gameDtos); //when returning statuscode like Ok instead of just data,
                                 //the method return type has to be wrapped in ActionResult
        }

        // GET: api/Games/
        [HttpGet("{title}")]
        public async Task<ActionResult<GameDto>> GetGame(string title)
        {
            //var game = await _context.Game.FindAsync(id);
            var game = await _unitOfWork.GameRepository.GetAsync(title);

            if (game == null)
            {
                return NotFound();
            }

            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }
        //// GET: api/Games/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<GameDto>> GetGame(int id)
        //{
        //    //var game = await _context.Game.FindAsync(id);
        //    var game = await _unitOfWork.GameRepository.GetAsync(id);

        //    if (game == null)
        //    {
        //        return NotFound();
        //    }

        //    var gameDto = _mapper.Map<GameDto>(game);
        //    return Ok(gameDto);
        //}

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        // POST: api/Games

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

            //return NoContent();
            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        [HttpPatch("{gameId}")]
        public async Task<ActionResult<GameDto>> PatchGame(int gameId, JsonPatchDocument<GameDto> patchDocument)
        {
            if (patchDocument is null) return BadRequest("No patch document");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var gameToPatch = await _unitOfWork.GameRepository.GetAsync(gameId);
            if (gameToPatch is null) return NotFound("Game not found");

            var dto = _mapper.Map<GameDto>(gameToPatch);
            patchDocument.ApplyTo(dto, ModelState);
            TryValidateModel(dto);
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            _mapper.Map(dto, gameToPatch);
            await _unitOfWork.CompleteAsync();

            return Ok(dto);
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //_context.Game.Add(gameDto);
            //await _context.SaveChangesAsync();
            var game = _mapper.Map<Game>(gameDto);
            _unitOfWork.GameRepository.Add(game);
            await _unitOfWork.CompleteAsync();
            var createdGameDto = _mapper.Map<GameDto>(game);
            return CreatedAtAction("GetGame", new { id = game.Id }, createdGameDto);
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
