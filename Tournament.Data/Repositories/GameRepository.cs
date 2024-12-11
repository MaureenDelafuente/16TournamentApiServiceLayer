using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class GameRepository : IGameRepository
{
    private DbContext _context;

    public GameRepository(TournamentApiContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Game>> GetAllAsync(int pageSize)
    {
        var games = await _context.Set<Game>()
            .Take(pageSize)
            .ToListAsync();
        return games;
    }

    public async Task<Game?> GetAsync(int id, bool trackChanges = false)
    {
        if (trackChanges)
        {
            return await _context.Set<Game>()
                .Where(g => g.Id == id)
                .AsTracking()
                .FirstOrDefaultAsync();
        }
        else
        {
            return await _context.Set<Game>()
                .Where(g => g.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }

    public async Task<Game?> GetAsync(string title)
    {
        var game = await _context.Set<Game>()
            .Where(g => g.Title == title)
            .FirstOrDefaultAsync();
        return game;
    }

    public async Task<bool> AnyAsync(int id)
    {
        var exists = await _context.Set<Game>()
            .AnyAsync(t => t.Id == id);
        return exists;
    }

    public void Add(Game game)
    {
        _context.Set<Game>().Add(game);
    }

    public void Update(Game game)
    {
        //if (_context.Entry(game).State == EntityState.Detached)
        //{
        //    _context.Attach(game);
        //}
        _context.Set<Game>().Update(game);
    }

    public void Remove(Game game)
    {
        _context.Set<Game>().Remove(game);
    }
}