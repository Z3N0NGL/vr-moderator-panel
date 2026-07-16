using UnityEngine;
using UnityEngine.UI;

// Extra tab: SESSION. Lobby-wide moderator actions - mute everyone,
// respawn everyone, lock the lobby so no new players can join, and a
// plain text readout of basic session info.

public class SessionTabController : MonoBehaviour
{
    [Header("Buttons")]
    public Button muteAllButton;
    public Button respawnAllButton;
    public Toggle lockLobbyToggle;

    [Header("Session Info")]
    public Text playerCountLabel;
    public Text sessionNameLabel;
    public string sessionName = "Lobby";
    public float infoRefreshIntervalSeconds = 3f;

    private IModNetworkAdapter _adapter;
    private float _refreshTimer;

    private void Start()
    {
        _adapter = FindNetworkAdapter();

        if (muteAllButton != null)
            muteAllButton.onClick.AddListener(OnMuteAllClicked);

        if (respawnAllButton != null)
            respawnAllButton.onClick.AddListener(OnRespawnAllClicked);

        if (lockLobbyToggle != null)
            lockLobbyToggle.onValueChanged.AddListener(OnLockLobbyToggled);

        if (sessionNameLabel != null)
            sessionNameLabel.text = sessionName;

        RefreshInfo();
    }

    private void Update()
    {
        _refreshTimer += Time.deltaTime;
        if (_refreshTimer >= infoRefreshIntervalSeconds)
        {
            _refreshTimer = 0f;
            RefreshInfo();
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
        return null;
    }

    private void RefreshInfo()
    {
        if (_adapter == null || playerCountLabel == null)
            return;

        playerCountLabel.text = "Players: " + _adapter.GetPlayers().Count;
    }

    private void OnMuteAllClicked()
    {
        _adapter?.MuteAllPlayers();
    }

    private void OnRespawnAllClicked()
    {
        _adapter?.RespawnAllPlayers();
    }

    private void OnLockLobbyToggled(bool isOn)
    {
        _adapter?.LockLobby(isOn);
    }
}
