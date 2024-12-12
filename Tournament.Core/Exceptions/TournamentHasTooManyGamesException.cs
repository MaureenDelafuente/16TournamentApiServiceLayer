namespace Tournament.Core.Exceptions
{
    [Serializable]
    public class TournamentHasTooManyGamesException : Exception
    {
        public TournamentHasTooManyGamesException(int tournamentId)
            : base($"Tournament {tournamentId} has too many games already, cannot add more")
        {
        }
    }
}