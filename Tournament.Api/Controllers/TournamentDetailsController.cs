using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Repositories;
using Bogus.DataSets;
using Microsoft.AspNetCore.JsonPatch;
using Tournament.Core.Dto;
using Service.Contracts;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;

        public TournamentDetailsController(IUnitOfWork unitOfWork, IMapper mapper, IServiceManager serviceManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _serviceManager = serviceManager;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails([FromQuery] bool includeGames = false)
        {
            //return await _unitOfWork.TournamentRepository.GetAllAsync();
            var tournaments
                = includeGames
                    ? await _unitOfWork.TournamentRepository.GetAllWithGamesAsync()
                    : await _unitOfWork.TournamentRepository.GetAllAsync();
            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(tournamentDtos);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);

            if (tournamentDetails == null)
            {
                return NotFound();
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournamentDetails);
            return Ok(tournamentDto);
        }

        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDetails tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest();
            }

            //_context.Entry(tournament).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                _unitOfWork.TournamentRepository.Update(tournament);
                _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentDetailsExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournament);
            return Ok(tournamentDto);
        }

        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int tournamentId,
            JsonPatchDocument<TournamentDto> patchDocument)
        {
            if (patchDocument is null) return BadRequest("No patch document");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tournamentToPatch = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);
            if (tournamentToPatch is null) return NotFound("Tournament not found");

            var dto = _mapper.Map<TournamentDto>(tournamentToPatch);
            patchDocument.ApplyTo(dto, ModelState);
            TryValidateModel(dto);
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            _mapper.Map(dto, tournamentToPatch);
            await _unitOfWork.CompleteAsync();

            return Ok(dto);
        }

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDetails>> PostTournamentDetails(TournamentDetails tournament)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //_context.TournamentDetails.Add(tournament);
            //await _context.SaveChangesAsync();
            _unitOfWork.TournamentRepository.Add(tournament);

            return CreatedAtAction("GetTournamentDetails", new { id = tournament.Id }, tournament);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            //var tournamentDetails = await _context.TournamentDetails.FindAsync(id);
            var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound();
            }

            //_context.TournamentDetails.Remove(tournamentDetails);
            //await _context.SaveChangesAsync();
            _unitOfWork.TournamentRepository.Remove(tournamentDetails);
            _unitOfWork.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> TournamentDetailsExistsAsync(int id)
        {
            //return _context.TournamentDetails.Any(e => e.Id == id);
            return await _unitOfWork.TournamentRepository.AnyAsync(id);
        }
    }
}
