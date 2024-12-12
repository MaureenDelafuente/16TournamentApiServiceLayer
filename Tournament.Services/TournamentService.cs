using AutoMapper;
using Service.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;


namespace Tournament.Services;

public class TournamentService : ITournamentService
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public TournamentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TournamentDto>> GetAllAsync(int pageSize, int page)
    {
        var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync(pageSize, page);
        var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        return tournamentDtos;
    }

    public async Task<IEnumerable<TournamentDto>> GetAllWithGamesAsync(int pageSize, int page)
    {
        var tournaments = await _unitOfWork.TournamentRepository.GetAllWithGamesAsync(pageSize, page);
        var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        return tournamentDtos;
    }

    public async Task<TournamentDto?> GetAsync(int id)
    {
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament is null) return null;
        var tournamentDto = _mapper.Map<TournamentDto>(tournament);
        return tournamentDto;
    }

    public async Task<TournamentDto?> GetAsync(string title)
    {
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(title);
        if (tournament is null) return null;
        var tournamentDto = _mapper.Map<TournamentDto>(tournament);
        return tournamentDto;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var exists = await _unitOfWork.TournamentRepository.AnyAsync(id);
        return exists;
    }

    public TournamentDetails Add(TournamentDto tournamentDto)
    {
        var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
        _unitOfWork.TournamentRepository.Add(tournament);
        _unitOfWork.CompleteAsync();
        return tournament;
    }

    public async Task Update(TournamentDto tournamentDto)
    {
        var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
        _unitOfWork.TournamentRepository.Update(tournament);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Remove(TournamentDto tournamentDto)
    {
        var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
        _unitOfWork.TournamentRepository.Remove(tournament);
         await _unitOfWork.CompleteAsync();
    }

    public void Update(TournamentDetails tournament)
    {
        throw new NotImplementedException();
    }

    public Task Remove(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> Count()
    {
        return _unitOfWork.TournamentRepository.Count();
    }
}