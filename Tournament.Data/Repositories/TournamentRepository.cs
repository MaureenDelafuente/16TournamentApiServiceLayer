using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class TournamentRepository: ITournamentRepository
{
    DbContext _context;

    public TournamentRepository(TournamentApiContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TournamentDetails>> GetAllAsync()
    {
        var tournaments = await _context.Set<TournamentDetails>()
            .ToListAsync();
        return tournaments;
    }

    public async Task<IEnumerable<TournamentDetails>> GetAllWithGamesAsync()
    {
        var tournaments = await _context.Set<TournamentDetails>()
            .Include(t => t.Games)
            .ToListAsync();
        return tournaments;
    }

    public async Task<TournamentDetails> GetAsync(int id)
    {
        var tournament = await _context.Set<TournamentDetails>()
            .Where(t => t.Id == id)
            //.Include(t => t.Games)
            .FirstOrDefaultAsync();
        return tournament;
    }

    public async Task<bool> AnyAsync(int id)
    {
        var exists = await _context.Set<TournamentDetails>()
            .AnyAsync(t => t.Id == id);
        return exists;
    }

    public void Add(TournamentDetails tournament)
    {
        _context.Set<TournamentDetails>().Add(tournament);
    }

    public void Update(TournamentDetails tournament)
    {
        _context.Set<TournamentDetails>().Update(tournament);
    }

    public void Remove(TournamentDetails tournament)
    {
        _context.Set<TournamentDetails>().Remove(tournament);
    }
}