using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface ITournamentRepository
{
    Task<IEnumerable<TournamentDetails>> GetAllAsync();
    Task<IEnumerable<TournamentDetails>> GetAllWithGamesAsync();
    Task<TournamentDetails> GetAsync(int id);
    Task<TournamentDetails> GetAsync(string title);
    Task<bool> AnyAsync(int id);
    void Add(TournamentDetails tournament);
    void Update(TournamentDetails tournament);
    void Remove(TournamentDetails tournament);
}