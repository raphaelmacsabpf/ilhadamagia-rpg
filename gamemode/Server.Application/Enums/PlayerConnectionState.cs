namespace Server.Application.Enums
{
    public enum PlayerConnectionState
    {
        GAMEMODE_LOAD_DELAY,
        INITIAL,
        CONNECTED,
        NEW_ACCOUNT,
        LOADING_ACCOUNT,
        LOGGED,
        DROPPED,
        SELECT_SPAWN_POSITION,
        SPAWNING,
        SPAWNED,
        DIED
    }
}