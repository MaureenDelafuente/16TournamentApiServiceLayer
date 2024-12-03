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

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        var games = await _context.Set<Game>()
            .Include(g => g.TournamentDetails) // TODO:include or not?
            .ToListAsync();
        return games;
    }

    public async Task<Game> GetAsync(int id)
    {
        var game = await _context.Set<Game>()
            .Where(g => g.Id == id)
            .Include(g => g.TournamentDetails) // TODO:include or not?
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
        _context.Set<Game>().Update(game);
    }

    public void Remove(Game game)
    {
        _context.Set<Game>().Remove(game);
    }
}