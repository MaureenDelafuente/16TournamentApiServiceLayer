using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Data.Repositories;

public class TournamentRepository: ITournamentRepository
{
    DbContext _context;

    public TournamentRepository()
    {
        _context = 
    }
    public Task<IEnumerable<TournamentDetails>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TournamentDetails> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(TournamentDetails tournament)
    {
        throw new NotImplementedException();
    }

    public void Update(TournamentDetails tournament)
    {
        throw new NotImplementedException();
    }

    public void Remove(TournamentDetails tournament)
    {
        throw new NotImplementedException();
    }
}