using UnityEngine;
using UnityEngine.UI;

// Local tab logic - priority speaker. When enabled, the moderator's
// voice plays louder for everyone (or overrides proximity falloff,
// depending on your voice system). Hook OnPriorityToggled and
// OnVolumeChanged into your actual voice/audio system.

public class LocalTabController : MonoBehaviour
{
    [Header("Priority Speaker")]
    public Toggle prioritySpeakerToggle;
    public Slider priorityVolumeSlider;   // Range set to 1-5 in Inspector
    public Text priorityVolumeValueLabel;

    public const float PriorityVolumeMin = 1f;
    public const float PriorityVolumeMax = 5f;

    private bool _prioritySpeakerEnabled;
    private float _priorityVolumeMultiplier = 1f;

    private void Start()
    {
        if (priorityVolumeSlider != null)
        {
            priorityVolumeSlider.minValue = PriorityVolumeMin;
            priorityVolumeSlider.maxValue = PriorityVolumeMax;
            priorityVolumeSlider.value = _priorityVolumeMultiplier;
            priorityVolumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (prioritySpeakerToggle != null)
            prioritySpeakerToggle.onValueChanged.AddListener(OnPriorityToggled);

        RefreshLabel();
    }

    private void OnPriorityToggled(bool isOn)
    {
        _prioritySpeakerEnabled = isOn;

        // TODO: hook into your voice/audio system here, e.g.:
        // myVoiceSource.overridesProximity = isOn;
        // myVoiceSource.volumeMultiplier = isOn ? _priorityVolumeMultiplier : 1f;
    }

    private void OnVolumeChanged(float value)
    {
        _priorityVolumeMultiplier = value;
        RefreshLabel();

        // TODO: push live if priority speaker is enabled, e.g.:
        // if (_prioritySpeakerEnabled) myVoiceSource.volumeMultiplier = _priorityVolumeMultiplier;
    }

    private void RefreshLabel()
    {
        if (priorityVolumeValueLabel != null)
            priorityVolumeValueLabel.text = _priorityVolumeMultiplier.ToString("0.0") + "x";
    }
}
