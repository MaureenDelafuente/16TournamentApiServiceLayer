using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface ITournamentRepository
{
    Task<IEnumerable<TournamentDetails>> GetAllAsync(int pageSize);
    Task<IEnumerable<TournamentDetails>> GetAllWithGamesAsync(int pageSize);
    Task<TournamentDetails?> GetAsync(int id);
    Task<TournamentDetails?> GetAsync(string title);
    Task<bool> AnyAsync(int id);
    Task Add(TournamentDetails tournament);
    void Update(TournamentDetails tournament);
    void Remove(TournamentDetails tournament);
    Task<int> Count();
}