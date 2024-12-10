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

    public async Task<IEnumerable<GameDto>> GetAll()
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

    public void Add(GameDto gameDto)
    {
        var game = _mapper.Map<Game>(gameDto);
        _unitOfWork.GameRepository.Add(game);
    }

    public void Update(GameDto gameDto)
    {
        var game = _mapper.Map<Game>(gameDto);
        _unitOfWork.GameRepository.Update(game);
    }

    public void Remove(GameDto gameDto)
    {
        var game = _mapper.Map<Game>(gameDto);
        _unitOfWork.GameRepository.Remove(game);
    }
}