namespace Tournament.Core.Exceptions;

public class NotFoundException : Exception
{
    public string Title { get; set; }

    protected NotFoundException(string? message, string title = "Not Found") : base(message)
    {
        Title = title;
    }
}

public class TournamentNotFoundException : NotFoundException
{
    public TournamentNotFoundException(int id) : base($"A tournament with id {id} was not found")
    {
        
    }
    public TournamentNotFoundException(string tournamentTitle) : base($"A tournament with id {tournamentTitle} was not found")
    {
        
    }
}

public class GameNotFoundException : NotFoundException
{
    public GameNotFoundException(int id) : base($"A game with id {id} was not found")
    {
        
    }
    public GameNotFoundException(string gameTitle) : base($"A game with title '{gameTitle}' was not found")
    {
        
    }

}
