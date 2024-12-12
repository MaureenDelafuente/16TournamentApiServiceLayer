using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class TournamentRepository : ITournamentRepository
{
    DbContext _context;

    public TournamentRepository(TournamentApiContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TournamentDetails>> GetAllAsync(int pageSize, int page)
    {
        var tournaments = await _context.Set<TournamentDetails>()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return tournaments;
    }

    public async Task<IEnumerable<TournamentDetails>> GetAllWithGamesAsync(int pageSize, int page)
    {
        var tournaments = await _context.Set<TournamentDetails>()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(t => t.Games)
            .ToListAsync();
        return tournaments;
    }

    public async Task<TournamentDetails?> GetAsync(int id)
    {
        var tournament = await _context.Set<TournamentDetails>()
            .Where(t => t.Id == id)
            //.Include(t => t.Games)
            .FirstOrDefaultAsync();
        return tournament;
    }

    public async Task<TournamentDetails?> GetAsync(string title)
    {
        var tournament = await _context.Set<TournamentDetails>()
            .Where(t => t.Title == title)
            .FirstOrDefaultAsync();
        return tournament;
        ;
    }

    public int GetGameCount(int tournamentId)
    {
        var count = _context.Set<TournamentDetails>()
            .Include(t => t.Games)
            .FirstOrDefaultAsync(t => t.Id == tournamentId)?
            .Result?.Games.Count
                    ?? 0;

        return count;
    }

    public async Task<bool> AnyAsync(int id)
    {
        var exists = await _context.Set<TournamentDetails>()
            .AnyAsync(t => t.Id == id);
        return exists;
    }

    public async Task Add(TournamentDetails tournament)
    {
        _context.Set<TournamentDetails>().Add(tournament);
        await _context.SaveChangesAsync();
    }

    public void Update(TournamentDetails tournament)
    {
        _context.Set<TournamentDetails>().Update(tournament);
    }

    public void Remove(TournamentDetails tournament)
    {
        _context.Set<TournamentDetails>().Remove(tournament);
    }

    public async Task<int> Count()
    {
        return _context.Set<TournamentDetails>().Count();
    }
}