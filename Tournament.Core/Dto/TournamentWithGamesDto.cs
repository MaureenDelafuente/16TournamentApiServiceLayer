using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.Dto;

public class TournamentWithGamesDto
{
    [Required]
    [StringLength(100, ErrorMessage = "The title must be less than 100 characters.")]
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate => StartDate.AddMonths(3);
    public IEnumerable<GameDto> Games { get; set; }
}