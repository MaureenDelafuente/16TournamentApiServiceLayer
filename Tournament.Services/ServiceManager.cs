using Service.Contracts;
using Tournament.Core.Repositories;

namespace Tournament.Services;

public class ServiceManager : Service.Contracts.IServiceManager
{
    private IUnitOfWork _unitOfWork;

    public ITournamentService TournamentService { get; set; }
    public IGameService GameService { get; set; }

    public ServiceManager(IUnitOfWork unitOfWork, ITournamentService tournamentService, IGameService gameService)
    {
        _unitOfWork = unitOfWork;
        TournamentService = tournamentService;
        GameService = gameService;
    }
}