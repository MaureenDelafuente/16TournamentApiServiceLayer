using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Service.Contracts;

public interface ITournamentService
{
    public Task<IEnumerable<TournamentDto>> GetAll();
    public Task<TournamentDto> Get(int id);
    public Task<TournamentDto> Get(string title);
    public Task<bool> Exists(int id);
    public void Add(TournamentDto tournament);
    public void Update(TournamentDto tournament);
    public void Remove(TournamentDto tournament);
}