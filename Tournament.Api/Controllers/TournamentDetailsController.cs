using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Repositories;
using Bogus.DataSets;
using Tournament.Core.Dto;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentDetailsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails()
        {
            //return await _unitOfWork.TournamentRepository.GetAllAsync();
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();
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

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDetails>> PostTournamentDetails(TournamentDetails tournament)
        {
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
