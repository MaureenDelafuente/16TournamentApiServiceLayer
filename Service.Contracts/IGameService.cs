using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Service.Contracts;

public interface IGameService
{
    public Task<IEnumerable<GameDto>> GetAll();
    public Task<GameDto> Get(int id);
    public Task<GameDto> Get(string title);
    public Task<bool> Exists(int id);
    public void Add(GameDto game);
    public void Update(GameDto game);
    public void Remove(GameDto game);
}