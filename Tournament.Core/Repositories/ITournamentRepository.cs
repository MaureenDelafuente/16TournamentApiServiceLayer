using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface ITournamentRepository
{
    Task<IEnumerable<TournamentDetails>> GetAllAsync(int pageSize, int page);
    Task<IEnumerable<TournamentDetails>> GetAllWithGamesAsync(int pageSize, int page);
    Task<TournamentDetails?> GetAsync(int id);
    Task<TournamentDetails?> GetAsync(string title);
    int GetGameCount(int tournamentId);
    Task<bool> AnyAsync(int id);
    Task Add(TournamentDetails tournament);
    void Update(TournamentDetails tournament);
    void Remove(TournamentDetails tournament);
    Task<int> Count();
}