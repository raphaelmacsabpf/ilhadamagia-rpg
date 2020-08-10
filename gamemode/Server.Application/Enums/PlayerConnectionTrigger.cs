namespace Server.Application.Enums
{
    public enum PlayerConnectionTrigger
    {
        PLAYER_DROPPED,
        CLIENT_READY,
        GAMEMODE_LOAD,
        ACCOUNT_FOUND,
        ACCOUNT_NOT_FOUND,
        ACCOUNT_SELECTED,
        SELECTING_SPAWN_POSITION,
        SET_TO_SPAWN,
        PLAYER_DIED
    }
}