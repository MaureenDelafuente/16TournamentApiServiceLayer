using Microsoft.EntityFrameworkCore;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private TournamentApiContext _context;
    public ITournamentRepository TournamentRepository { get; }
    public IGameRepository GameRepository { get; }

    public UnitOfWork(TournamentApiContext context, ITournamentRepository tournamentRepository, IGameRepository gameRepository)
    {
        _context = context;
        TournamentRepository = tournamentRepository;
        GameRepository = gameRepository;
    }

    // "Klassen kommer även behöva få dbcontext injicerat i sin konstruktor så den kan skicka det vidare
    // till instanserna av TournamentRepository och GameRepository"
    //TODO: do i need to manually pass the context to the repository instances here somehow?
    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}