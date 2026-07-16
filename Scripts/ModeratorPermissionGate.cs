using System.Collections.Generic;
using UnityEngine;

// Drop this on the same GameObject as ModPanelActivator.
// It disables the activator for anyone whose PlayFab ID is not in the
// list below, so out of the box only approved moderators can open the
// panel. This is the one piece every project needs to fill in itself,
// since "who counts as a moderator" is different for every game.
//
// Simple by design: paste PlayFab IDs into the list in the Inspector.
// If your game already has its own role/permission system, delete this
// script and call ModPanelActivator.SetAuthorized(bool) from your own
// permission check instead - that method exists for exactly this case.

public class ModeratorPermissionGate : MonoBehaviour
{
    [Header("Moderator PlayFab IDs")]
    [Tooltip("Paste the PlayFab ID of every player who should be allowed to open the panel.")]
    public List<string> authorizedPlayFabIds = new List<string>();

    [Header("How to get the local player's ID")]
    [Tooltip("Leave empty to use PlayFabClientAPI automatically (requires the PlayFab SDK to be installed). "
        + "If you are not using PlayFab, set this manually from your own login code by calling "
        + "ModeratorPermissionGate.SetLocalPlayFabId(id) as soon as you know it.")]
    public string localPlayFabId;

    private ModPanelActivator _activator;

    private static ModeratorPermissionGate _instance;

    private void Awake()
    {
        _instance = this;
        _activator = GetComponent<ModPanelActivator>();
    }

    private void Start()
    {
        Evaluate();
    }

    // Call this from your own login/auth code the moment you know the
    // local player's PlayFab ID, if you are not letting this script
    // fetch it automatically.
    public static void SetLocalPlayFabId(string playFabId)
    {
        if (_instance == null)
        {
            Debug.LogWarning("ModeratorPermissionGate: SetLocalPlayFabId called before the gate exists in the scene.");
            return;
        }

        _instance.localPlayFabId = playFabId;
        _instance.Evaluate();
    }

    private void Evaluate()
    {
        if (_activator == null)
        {
            Debug.LogWarning("ModeratorPermissionGate: no ModPanelActivator found on this GameObject. "
                + "Put this script on the same object as ModPanelActivator.");
            return;
        }

        bool authorized = !string.IsNullOrEmpty(localPlayFabId)
            && authorizedPlayFabIds.Contains(localPlayFabId);

        _activator.SetAuthorized(authorized);
    }
}
