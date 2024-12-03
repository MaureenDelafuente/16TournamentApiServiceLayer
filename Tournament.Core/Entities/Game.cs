using System.ComponentModel.DataAnnotations.Schema;

namespace Tournament.Core.Entities;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; }

    public DateTime Time { get; set; }

    public int TournamentId { get; set; }

    [ForeignKey("TournamentId")] 
    public TournamentDetails? TournamentDetails { get; set; } // Navigation property
}