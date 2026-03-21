public static class TeamSelectionState
{
    public enum Team
    {
        Red,
        Blue
    }

    public static Team Player1Team { get; private set; } = Team.Red;

    public static Team Player2Team { get; private set; } = Team.Blue;

    public static void AssignTeams(Team player1Team)
    {
        Player1Team = player1Team;
        Player2Team = player1Team == Team.Red ? Team.Blue : Team.Red;
    }
}
