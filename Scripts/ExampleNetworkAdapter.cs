using System.Collections.Generic;
using UnityEngine;

// EXAMPLE ONLY. This is not connected to any real networking system.
// Delete this file once you've written your real adapter, or use it
// as a template. Attach whatever adapter you build to the same
// GameObject as ModPanelActivator and it will be found automatically.

public class ExampleNetworkAdapter : MonoBehaviour, IModNetworkAdapter
{
    public List<ModPlayerInfo> GetPlayers()
    {
        // TODO: replace with a real call to your networking backend.
        // Example (Mirror): loop NetworkServer.connections and read
        // a PlayerData component off each connection's player object.
        return new List<ModPlayerInfo>
        {
            new ModPlayerInfo("ExamplePlayerOne", "PF-0000000001"),
            new ModPlayerInfo("ExamplePlayerTwo", "PF-0000000002")
        };
    }

    public void KickPlayer(string playFabId)
    {
        // TODO: send a disconnect/close-game command to that client's connection.
        Debug.Log("Kick requested for " + playFabId);
    }

    public void BringPlayerHere(string playFabId)
    {
        // TODO: send an RPC/command telling that client to teleport to
        // the moderator's transform.position.
        Debug.Log("Bring here requested for " + playFabId);
    }

    public void TeleportToPlayer(string playFabId)
    {
        // TODO: move the local moderator's rig/character controller to
        // that player's current position.
        Debug.Log("Teleport to requested for " + playFabId);
    }

    public void MuteAllPlayers()
    {
        Debug.Log("Mute all requested");
    }

    public void RespawnAllPlayers()
    {
        Debug.Log("Respawn all requested");
    }

    public void LockLobby(bool locked)
    {
        Debug.Log("Lock lobby set to " + locked);
    }
}
