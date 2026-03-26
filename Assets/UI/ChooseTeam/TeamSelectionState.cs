public static class TeamSelectionState
{
    public enum Team
    {
        Red,
        Blue
    }

    public static Team Device0Team { get; private set; } = Team.Red;

    public static Team Device1Team { get; private set; } = Team.Blue;

    public static int RedDeviceIndex => Device0Team == Team.Red ? 0 : 1;

    public static int BlueDeviceIndex => Device0Team == Team.Blue ? 0 : 1;

    public static void AssignDevice0Team(Team device0Team)
    {
        Device0Team = device0Team;
        Device1Team = device0Team == Team.Red ? Team.Blue : Team.Red;
    }
}
