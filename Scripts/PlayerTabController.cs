using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Player tab logic. Lists everyone in the lobby. Clicking a name selects
// them and reveals their PlayFab ID plus the three action buttons.
// Needs an IModNetworkAdapter implementation somewhere in the scene
// (see ExampleNetworkAdapter.cs for a template).

public class PlayerTabController : MonoBehaviour
{
    [Header("List")]
    public Transform playerListContent;      // Content object of a ScrollRect
    public GameObject playerListEntryPrefab; // Prefab with PlayerListEntry component
    public float refreshIntervalSeconds = 2f;

    [Header("Selected Player Detail")]
    public GameObject selectedPlayerPanel;   // Shown only once a player is selected
    public Text selectedPlayerNameLabel;
    public Text selectedPlayerIdLabel;
    public Button kickButton;
    public Button bringHereButton;
    public Button teleportToButton;

    private IModNetworkAdapter _adapter;
    private readonly List<GameObject> _spawnedEntries = new List<GameObject>();
    private ModPlayerInfo? _selectedPlayer;
    private float _refreshTimer;

    private void Start()
    {
        _adapter = FindNetworkAdapter();

        if (selectedPlayerPanel != null)
            selectedPlayerPanel.SetActive(false);

        if (kickButton != null)
            kickButton.onClick.AddListener(OnKickClicked);

        if (bringHereButton != null)
            bringHereButton.onClick.AddListener(OnBringHereClicked);

        if (teleportToButton != null)
            teleportToButton.onClick.AddListener(OnTeleportToClicked);

        RefreshPlayerList();
    }

    private void Update()
    {
        _refreshTimer += Time.deltaTime;
        if (_refreshTimer >= refreshIntervalSeconds)
        {
            _refreshTimer = 0f;
            RefreshPlayerList();
        }
    }

    private IModNetworkAdapter FindNetworkAdapter()
    {
        MonoBehaviour[] all = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour m in all)
        {
            if (m is IModNetworkAdapter adapter)
                return adapter;
        }

        Debug.LogWarning("PlayerTabController: no IModNetworkAdapter found in scene.");
        return null;
    }

    public void RefreshPlayerList()
    {
        if (_adapter == null || playerListContent == null || playerListEntryPrefab == null)
            return;

        List<ModPlayerInfo> players = _adapter.GetPlayers();

        foreach (GameObject entry in _spawnedEntries)
            Destroy(entry);
        _spawnedEntries.Clear();

        foreach (ModPlayerInfo player in players)
        {
            GameObject entryObj = Instantiate(playerListEntryPrefab, playerListContent);
            PlayerListEntry entry = entryObj.GetComponent<PlayerListEntry>();

            if (entry != null)
                entry.Setup(player, SelectPlayer);

            _spawnedEntries.Add(entryObj);
        }
    }

    private void SelectPlayer(ModPlayerInfo player)
    {
        _selectedPlayer = player;

        if (selectedPlayerPanel != null)
            selectedPlayerPanel.SetActive(true);

        if (selectedPlayerNameLabel != null)
            selectedPlayerNameLabel.text = player.DisplayName;

        if (selectedPlayerIdLabel != null)
            selectedPlayerIdLabel.text = player.PlayFabId;
    }

    private void OnKickClicked()
    {
        if (_adapter == null || _selectedPlayer == null)
            return;

        _adapter.KickPlayer(_selectedPlayer.Value.PlayFabId);
        RefreshPlayerList();
    }

    private void OnBringHereClicked()
    {
        if (_adapter == null || _selectedPlayer == null)
            return;

        _adapter.BringPlayerHere(_selectedPlayer.Value.PlayFabId);
    }

    private void OnTeleportToClicked()
    {
        if (_adapter == null || _selectedPlayer == null)
            return;

        _adapter.TeleportToPlayer(_selectedPlayer.Value.PlayFabId);
    }
}
