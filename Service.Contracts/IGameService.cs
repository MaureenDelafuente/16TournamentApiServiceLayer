using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Service.Contracts;

public interface IGameService
{
    public Task<IEnumerable<GameDto>> GetAllAsync();
    public Task<GameDto> Get(int id);
    public Task<GameDto> Get(string title);
    public Task<bool> Exists(int id);
    public Game Add(GameDto game);
    public void Update(Game game);
    public Task Remove(int id);
}