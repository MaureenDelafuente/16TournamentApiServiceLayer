using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync();
    Task<Game> GetAsync(int id);
    Task<Game> GetAsync(string title);

    Task<bool> AnyAsync(int id);
    void Add(Game tournament);
    void Update(Game tournament);
    void Remove(Game tournament);

}