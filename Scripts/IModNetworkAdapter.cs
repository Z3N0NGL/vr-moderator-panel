using System.Collections.Generic;

// Implement this once for whatever networking solution the game uses
// (Mirror, Fusion, Netcode for GameObjects, custom PlayFab server, etc).
// Every other script only talks to this interface, so swapping backends
// later does not require touching the panel code at all.

public interface IModNetworkAdapter
{
    // Returns every player currently in the lobby/room.
    List<ModPlayerInfo> GetPlayers();

    // Force-closes the target player's game/client.
    void KickPlayer(string playFabId);

    // Teleports the target player to the moderator's current position.
    void BringPlayerHere(string playFabId);

    // Teleports the moderator to the target player's current position.
    void TeleportToPlayer(string playFabId);

    // Optional: used by the session tab. Leave the body empty if not needed.
    void MuteAllPlayers();
    void RespawnAllPlayers();
    void LockLobby(bool locked);
}

[System.Serializable]
public struct ModPlayerInfo
{
    public string DisplayName;
    public string PlayFabId;

    public ModPlayerInfo(string displayName, string playFabId)
    {
        DisplayName = displayName;
        PlayFabId = playFabId;
    }
}
