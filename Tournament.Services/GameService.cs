﻿using AutoMapper;
using Service.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Exceptions;
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

    public async Task<IEnumerable<GameDto>> GetAllAsync(int pageSize, int page)
    {
        var games = await _unitOfWork.GameRepository.GetAllAsync(pageSize, page);
        var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
        return gameDtos;
    }

    public async Task<GameDto?> GetAsync(int id)
    {
        var game = await _unitOfWork.GameRepository.GetAsync(id);
        if (game is null) throw new GameNotFoundException(id);
        var gameDto = _mapper.Map<GameDto>(game);
        return gameDto;
    }

    public async Task<GameDto?> GetAsync(string title)
    {
        var game = await _unitOfWork.GameRepository.GetAsync(title);
        if (game is null) throw new GameNotFoundException(title);
        var gameDto = _mapper.Map<GameDto>(game);
        return gameDto;
    }
        
    public async Task<bool> ExistsAsync(int id)
    {
        var exists = await _unitOfWork.GameRepository.AnyAsync(id);
        return exists;
    }

    public async Task<Game> Add(GameDto gameDto)
    {
        //var tournament = await _unitOfWork.TournamentRepository.GetAsync(gameDto.TournamentId);
        //if (tournament.Games.Count >= 10) throw new TournamentHasTooManyGamesException(gameDto.TournamentId);
        var count = _unitOfWork.TournamentRepository.GetGameCount(gameDto.TournamentId);
        if (count >= 10) throw new TournamentHasTooManyGamesException(gameDto.TournamentId);
        var game = _mapper.Map<Game>(gameDto);
        _unitOfWork.GameRepository.Add(game);
        _unitOfWork.CompleteAsync();
        return game;
    }

    public async Task Update(Game gameDto)
    {//TODO: this is weird
        var game = _mapper.Map<Game>(gameDto);
        _unitOfWork.GameRepository.Update(game);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Remove(int id)
    {
        var game = await _unitOfWork.GameRepository.GetAsync(id);
        _unitOfWork.GameRepository.Remove(game);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<int> Count()
    {
        return await _unitOfWork.GameRepository.CountAsync();
    }
}