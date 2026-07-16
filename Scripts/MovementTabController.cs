using UnityEngine;
using UnityEngine.UI;

// Movement tab logic. Assumes you have some kind of player controller
// on the local rig - hook the marked sections into your actual
// movement/scale scripts (see comments below).

public class MovementTabController : MonoBehaviour
{
    [Header("Target rig")]
    public Transform playerRig;                 // The object that actually moves/scales

    [Header("Fly")]
    public Toggle flyToggle;
    public Slider flySpeedSlider;                // Range set to 5-25 in Inspector
    public Text flySpeedValueLabel;

    [Header("Speed Boost")]
    public Toggle speedBoostToggle;
    public Slider speedBoostSlider;              // Range set to 1-3 in Inspector
    public Text speedBoostValueLabel;

    [Header("Scale")]
    public Toggle scaleToggle;
    public Slider scaleSlider;                   // Range set to 0.2-5 in Inspector
    public Text scaleValueLabel;

    public const float FlySpeedMin = 5f;
    public const float FlySpeedMax = 25f;
    public const float SpeedBoostMin = 1f;
    public const float SpeedBoostMax = 3f;
    public const float ScaleMin = 0.2f;
    public const float ScaleMax = 5f;

    private bool _flyEnabled;
    private float _flySpeed = 10f;
    private bool _speedBoostEnabled;
    private float _speedBoostMultiplier = 1f;
    private bool _scaleEnabled;
    private float _scaleMultiplier = 1f;
    private Vector3 _baseScale = Vector3.one;

    private void Start()
    {
        if (playerRig != null)
            _baseScale = playerRig.localScale;

        if (flySpeedSlider != null)
        {
            flySpeedSlider.minValue = FlySpeedMin;
            flySpeedSlider.maxValue = FlySpeedMax;
            flySpeedSlider.value = _flySpeed;
            flySpeedSlider.onValueChanged.AddListener(OnFlySpeedChanged);
        }

        if (speedBoostSlider != null)
        {
            speedBoostSlider.minValue = SpeedBoostMin;
            speedBoostSlider.maxValue = SpeedBoostMax;
            speedBoostSlider.value = _speedBoostMultiplier;
            speedBoostSlider.onValueChanged.AddListener(OnSpeedBoostChanged);
        }

        if (scaleSlider != null)
        {
            scaleSlider.minValue = ScaleMin;
            scaleSlider.maxValue = ScaleMax;
            scaleSlider.value = _scaleMultiplier;
            scaleSlider.onValueChanged.AddListener(OnScaleChanged);
        }

        if (flyToggle != null)
            flyToggle.onValueChanged.AddListener(OnFlyToggled);

        if (speedBoostToggle != null)
            speedBoostToggle.onValueChanged.AddListener(OnSpeedBoostToggled);

        if (scaleToggle != null)
            scaleToggle.onValueChanged.AddListener(OnScaleToggled);

        RefreshLabels();
    }

    private void OnFlyToggled(bool isOn)
    {
        _flyEnabled = isOn;

        // TODO: hook into your actual fly/movement controller here, e.g.:
        // myFlightController.enabled = isOn;
        // myFlightController.flySpeed = _flySpeed;
    }

    private void OnFlySpeedChanged(float value)
    {
        _flySpeed = value;
        RefreshLabels();

        // TODO: push live to your flight controller if fly is enabled, e.g.:
        // if (_flyEnabled) myFlightController.flySpeed = _flySpeed;
    }

    private void OnSpeedBoostToggled(bool isOn)
    {
        _speedBoostEnabled = isOn;

        // TODO: hook into your movement speed multiplier here, e.g.:
        // myMovementController.speedMultiplier = isOn ? _speedBoostMultiplier : 1f;
    }

    private void OnSpeedBoostChanged(float value)
    {
        _speedBoostMultiplier = value;
        RefreshLabels();

        // TODO: push live if boost is enabled, e.g.:
        // if (_speedBoostEnabled) myMovementController.speedMultiplier = _speedBoostMultiplier;
    }

    private void OnScaleToggled(bool isOn)
    {
        _scaleEnabled = isOn;
        ApplyScale();
    }

    private void OnScaleChanged(float value)
    {
        _scaleMultiplier = value;
        RefreshLabels();
        ApplyScale();
    }

    private void ApplyScale()
    {
        if (playerRig == null)
            return;

        playerRig.localScale = _scaleEnabled
            ? _baseScale * _scaleMultiplier
            : _baseScale;
    }

    private void RefreshLabels()
    {
        if (flySpeedValueLabel != null)
            flySpeedValueLabel.text = _flySpeed.ToString("0.0");

        if (speedBoostValueLabel != null)
            speedBoostValueLabel.text = _speedBoostMultiplier.ToString("0.0") + "x";

        if (scaleValueLabel != null)
            scaleValueLabel.text = _scaleMultiplier.ToString("0.00") + "x";
    }
}
