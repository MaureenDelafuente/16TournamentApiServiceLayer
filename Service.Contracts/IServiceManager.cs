namespace Service.Contracts;

public interface IServiceManager
{
    public ITournamentService TournamentService { get; set; }
    public IGameService GameService { get; set; }

} 