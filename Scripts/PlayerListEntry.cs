using System;
using UnityEngine;
using UnityEngine.UI;

// Attach to the player list entry prefab, alongside a Text (name) and a Button.
// No icon/avatar image needed - name text only, per design requirements.

public class PlayerListEntry : MonoBehaviour
{
    public Text nameLabel;
    public Button selectButton;

    private ModPlayerInfo _player;
    private Action<ModPlayerInfo> _onSelected;

    public void Setup(ModPlayerInfo player, Action<ModPlayerInfo> onSelected)
    {
        _player = player;
        _onSelected = onSelected;

        if (nameLabel != null)
            nameLabel.text = player.DisplayName;

        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => _onSelected?.Invoke(_player));
        }
    }
}
