using System.ComponentModel.DataAnnotations;
using Tournament.Core.Entities;

namespace Tournament.Core.Dto;

public class GameDto
{
    [Required]
    [StringLength(100, ErrorMessage = "The title must be less than 100 characters.")]
    public string Title { get; set; }
    public DateTime Time { get; set; }
    public int TournamentId { get; set; }
}