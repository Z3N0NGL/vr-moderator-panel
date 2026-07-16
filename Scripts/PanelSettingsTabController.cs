using UnityEngine;
using UnityEngine.UI;

// Extra tab: SETTINGS. Lets the moderator adjust how the panel itself
// behaves - how far it sits from their face, how fast it follows their
// head, and a lock option that freezes it in world space instead of
// following. Also holds the close button.

public class PanelSettingsTabController : MonoBehaviour
{
    [Header("References")]
    public ModPanelActivator panelActivator;

    [Header("Distance")]
    public Slider distanceSlider;      // Suggested range 0.3 - 1.2
    public Text distanceValueLabel;

    [Header("Follow Speed")]
    public Slider followSpeedSlider;   // Suggested range 2 - 20
    public Text followSpeedValueLabel;

    [Header("Lock In Place")]
    public Toggle lockPanelToggle;

    [Header("Close")]
    public Button closeButton;

    private void Start()
    {
        if (panelActivator == null)
            panelActivator = FindObjectOfType<ModPanelActivator>();

        if (distanceSlider != null)
        {
            distanceSlider.value = panelActivator != null ? panelActivator.distanceFromFace : 0.6f;
            distanceSlider.onValueChanged.AddListener(OnDistanceChanged);
            RefreshDistanceLabel(distanceSlider.value);
        }

        if (followSpeedSlider != null)
        {
            followSpeedSlider.value = panelActivator != null ? panelActivator.followPositionSpeed : 12f;
            followSpeedSlider.onValueChanged.AddListener(OnFollowSpeedChanged);
            RefreshFollowSpeedLabel(followSpeedSlider.value);
        }

        if (lockPanelToggle != null)
            lockPanelToggle.onValueChanged.AddListener(OnLockToggled);

        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);
    }

    private void OnDistanceChanged(float value)
    {
        if (panelActivator != null)
            panelActivator.distanceFromFace = value;

        RefreshDistanceLabel(value);
    }

    private void OnFollowSpeedChanged(float value)
    {
        if (panelActivator != null)
        {
            panelActivator.followPositionSpeed = value;
            panelActivator.followRotationSpeed = value;
        }

        RefreshFollowSpeedLabel(value);
    }

    private void OnLockToggled(bool isOn)
    {
        // When locked, followPositionSpeed/rotationSpeed are effectively
        // set to zero so the panel stops tracking the head and stays
        // wherever it currently is in world space.
        if (panelActivator == null)
            return;

        if (isOn)
        {
            panelActivator.followPositionSpeed = 0f;
            panelActivator.followRotationSpeed = 0f;
        }
        else
        {
            panelActivator.followPositionSpeed = followSpeedSlider != null ? followSpeedSlider.value : 12f;
            panelActivator.followRotationSpeed = panelActivator.followPositionSpeed;
        }
    }

    private void OnCloseClicked()
    {
        panelActivator?.ClosePanel();
    }

    private void RefreshDistanceLabel(float value)
    {
        if (distanceValueLabel != null)
            distanceValueLabel.text = value.ToString("0.00") + "m";
    }

    private void RefreshFollowSpeedLabel(float value)
    {
        if (followSpeedValueLabel != null)
            followSpeedValueLabel.text = value.ToString("0.0");
    }
}
