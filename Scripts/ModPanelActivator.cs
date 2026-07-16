using UnityEngine;
using UnityEngine.XR;

// Put this on a persistent GameObject in the scene (e.g. an empty object
// under your XR Rig, or a manager object). Assign the panel and the
// player's head transform (usually the XR Camera) in the Inspector.
//
// Activation: click either the left or right joystick (thumbstick press).
// While open, the panel repositions/rotates in front of the player's
// face every frame so it stays readable no matter where they look.
// This is plain positional tracking only - the panel does not "look at"
// or track the player in any smarter way than that.

public class ModPanelActivator : MonoBehaviour
{
    [Header("References")]
    public GameObject panelRoot;          // Root object of the mod panel UI
    public Transform headTransform;       // XR Camera / head transform

    [Header("Placement")]
    public float distanceFromFace = 0.6f;
    public float heightOffset = 0.0f;
    public float followPositionSpeed = 12f;
    public float followRotationSpeed = 12f;

    [Header("Input")]
    [Tooltip("How far the joystick must be pushed to count as a click-toggle if you are not using a physical button press.")]
    public float joystickClickThreshold = 0.9f;

    private bool _panelOpen;
    private bool _authorized = true; // Open by default until a permission gate says otherwise
    private InputDevice _leftDevice;
    private InputDevice _rightDevice;
    private bool _leftPrevPressed;
    private bool _rightPrevPressed;
    private float _deviceRetryTimer;
    private bool _loggedNoDeviceWarning;

    private void Start()
    {
        if (panelRoot == null)
            Debug.LogWarning("ModPanelActivator: Panel Root is not assigned in the Inspector. The panel cannot open until this is set.", this);

        if (headTransform == null)
            Debug.LogWarning("ModPanelActivator: Head Transform is not assigned in the Inspector. Assign your XR Camera or the panel will not be able to position itself.", this);

        if (panelRoot != null)
            panelRoot.SetActive(false);

        TryGetDevices();
    }

    // Called by ModeratorPermissionGate (or your own permission code) to
    // decide whether this player is allowed to open the panel at all.
    // Defaults to true if nothing ever calls this, so the panel still
    // works out of the box for testing before you've hooked up real
    // permission checks.
    public void SetAuthorized(bool authorized)
    {
        _authorized = authorized;

        if (!authorized && _panelOpen)
            ClosePanel();
    }

    private void TryGetDevices()
    {
        if (!_leftDevice.isValid)
            _leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (!_rightDevice.isValid)
            _rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    private void Update()
    {
        if (!_authorized)
            return;

        if (!_leftDevice.isValid || !_rightDevice.isValid)
        {
            _deviceRetryTimer += Time.deltaTime;

            // Retry a few times a second rather than every single frame -
            // devices often aren't ready on the exact first frame.
            if (_deviceRetryTimer >= 0.2f)
            {
                _deviceRetryTimer = 0f;
                TryGetDevices();

                if (!_leftDevice.isValid && !_rightDevice.isValid && !_loggedNoDeviceWarning)
                {
                    Debug.LogWarning("ModPanelActivator: no left or right XR controller detected yet. "
                        + "This is normal for the first second or two after the app starts. "
                        + "If this warning keeps appearing after that, check that a VR headset/controllers "
                        + "are connected and that your XR Plug-in Management settings have a provider enabled.", this);
                    _loggedNoDeviceWarning = true;
                }
            }
        }

        HandleJoystickClick(_leftDevice, ref _leftPrevPressed);
        HandleJoystickClick(_rightDevice, ref _rightPrevPressed);

        if (_panelOpen)
            FollowHead();
    }

    private void HandleJoystickClick(InputDevice device, ref bool prevPressed)
    {
        if (!device.isValid)
            return;

        bool pressed = false;

        // Physical thumbstick click button.
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool clicked))
            pressed = clicked;

        // Fallback for controllers/setups without a real click button:
        // treat a hard push in any direction as a click.
        if (!pressed && device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis))
        {
            if (axis.magnitude >= joystickClickThreshold)
                pressed = true;
        }

        if (pressed && !prevPressed)
            TogglePanel();

        prevPressed = pressed;
    }

    private void TogglePanel()
    {
        if (panelRoot == null)
        {
            Debug.LogWarning("ModPanelActivator: tried to open the panel but Panel Root is not assigned. "
                + "Assign it in the Inspector.", this);
            return;
        }

        _panelOpen = !_panelOpen;

        if (_panelOpen)
        {
            PlacePanelInFrontOfFace(instant: true);
            panelRoot.SetActive(true);
        }
        else
        {
            panelRoot.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        _panelOpen = false;
        if (panelRoot != null)
            panelRoot.SetActive(false);
    }

    private void PlacePanelInFrontOfFace(bool instant)
    {
        if (headTransform == null)
            return;

        Vector3 targetPos = headTransform.position
            + headTransform.forward * distanceFromFace
            + Vector3.up * heightOffset;

        Quaternion targetRot = Quaternion.LookRotation(
            targetPos - headTransform.position, Vector3.up);

        if (instant)
        {
            panelRoot.transform.position = targetPos;
            panelRoot.transform.rotation = targetRot;
        }
    }

    private void FollowHead()
    {
        if (headTransform == null || panelRoot == null)
            return;

        Vector3 targetPos = headTransform.position
            + headTransform.forward * distanceFromFace
            + Vector3.up * heightOffset;

        Quaternion targetRot = Quaternion.LookRotation(
            targetPos - headTransform.position, Vector3.up);

        panelRoot.transform.position = Vector3.Lerp(
            panelRoot.transform.position, targetPos, Time.deltaTime * followPositionSpeed);

        panelRoot.transform.rotation = Quaternion.Slerp(
            panelRoot.transform.rotation, targetRot, Time.deltaTime * followRotationSpeed);
    }
}
