using AutoMapper;
using Service.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;


namespace Tournament.Services
{
    public class TournamentService : ITournamentService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public TournamentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TournamentDto>> GetAll()
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();
            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return tournamentDtos;
        }

        public async Task<TournamentDto> Get(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);
            var tournamentDto = _mapper.Map<TournamentDto>(tournament);
            return tournamentDto;
        }

        public async Task<TournamentDto> Get(string title)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(title);
            var tournamentDto = _mapper.Map<TournamentDto>(tournament);
            return tournamentDto;
        }

        public async Task<bool> Exists(int id)
        {
            var exists = await _unitOfWork.TournamentRepository.AnyAsync(id);
            return exists;
        }

        public void Add(TournamentDto tournamentDto)
        {
            var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
            _unitOfWork.TournamentRepository.Add(tournament);
            _unitOfWork.CompleteAsync();
        }

        public void Update(TournamentDto tournamentDto)
        {
            var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
            _unitOfWork.TournamentRepository.Update(tournament);
            _unitOfWork.CompleteAsync();
        }

        public void Remove(TournamentDto tournamentDto)
        {
            var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
            _unitOfWork.TournamentRepository.Remove(tournament);
            _unitOfWork.CompleteAsync();
        }
    }
}
