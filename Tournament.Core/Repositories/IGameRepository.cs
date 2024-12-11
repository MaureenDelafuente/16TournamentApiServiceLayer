using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync(int pageSize);
    Task<Game?> GetAsync(int id, bool trackChanges = false);
    Task<Game?> GetAsync(string title);

    Task<bool> AnyAsync(int id);
    void Add(Game game);
    void Update(Game game);
    void Remove(Game game);
}