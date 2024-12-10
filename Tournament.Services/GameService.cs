using AutoMapper;
using Service.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;


namespace Tournament.Services;

public class GameService : IGameService
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public GameService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        var games = await _unitOfWork.GameRepository.GetAllAsync();
        var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
        return gameDtos;
    }

    public async Task<GameDto> Get(int id)
    {
        var game = await _unitOfWork.GameRepository.GetAsync(id);
        var gameDto = _mapper.Map<GameDto>(game);
        return gameDto;
    }

    public async Task<GameDto> Get(string title)
    {
        var game = await _unitOfWork.GameRepository.GetAsync(title);
        var gameDto = _mapper.Map<GameDto>(game);
        return gameDto;
    }

    public async Task<bool> Exists(int id)
    {
        var exists = await _unitOfWork.GameRepository.AnyAsync(id);
        return exists;
    }

    public Game Add(GameDto gameDto)
    {
        var game = _mapper.Map<Game>(gameDto);
        _unitOfWork.GameRepository.Add(game);
        _unitOfWork.CompleteAsync();
        return game;
    }

    public void Update(Game gameDto)
    {
        var game = _mapper.Map<Game>(gameDto);
        _unitOfWork.GameRepository.Update(game);
        _unitOfWork.CompleteAsync();
    }

    public async Task Remove(int id)
    {
        var game = await _unitOfWork.GameRepository.GetAsync(id);
        _unitOfWork.GameRepository.Remove(game);
        _unitOfWork.CompleteAsync();
    }
}