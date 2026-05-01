/// <summary>
/// Shared enumerations for the Cluedo game.
/// Keep in a single file so every script references the same types.
/// </summary>
public static class CluedoEnums
{
    public enum Suspect
    {
        MissScarlett,
        ColMustard,
        MrsWhite,
        RevGreen,
        MrsPeacock,
        ProfPlum
    }

    public enum Weapon
    {
        Candlestick,
        Knife,
        LeadPipe,
        Revolver,
        Rope,
        Wrench
    }

    public enum Room
    {
        Kitchen,
        Ballroom,
        Conservatory,
        BilliardRoom,
        Library,
        Study,
        Hall,
        Lounge,
        DiningRoom,
        Corridor   
    }

    public enum GamePhase
    {
        Setup,
        Rolling,
        Moving,
        MakingSuggestion,
        ResolvingSuggestion,
        MakingAccusation,
        GameOver
    }
}