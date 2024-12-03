namespace Tournament.Core.Dto;

public class TournamentDto
{
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate => StartDate.AddMonths(3);
}