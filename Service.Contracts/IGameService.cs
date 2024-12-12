using Microsoft.AspNetCore.JsonPatch;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Service.Contracts;

public interface IGameService
{
    public Task<IEnumerable<GameDto>> GetAllAsync(int pageSize);
    public Task<GameDto?> GetAsync(int id);
    public Task<GameDto?> GetAsync(string title);
    public Task<bool> ExistsAsync(int id);
    public Game Add(GameDto game);
    public Task Update(Game game);
    public Task Remove(int id);
    public Task<int> Count();
}