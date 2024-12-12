using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Service.Contracts;

public interface ITournamentService
{
    public Task<IEnumerable<TournamentDto>> GetAllAsync(int pageSize);
    public Task<IEnumerable<TournamentDto>> GetAllWithGamesAsync(int pageSize);
    public Task<TournamentDto?> GetAsync(int id);
    public Task<TournamentDto?> GetAsync(string title);
    public Task<bool> ExistsAsync(int id);
    public TournamentDetails Add(TournamentDto tournament);
    public void Update(TournamentDetails tournament);
    public Task Remove(int id);
    public Task<int> Count();
}