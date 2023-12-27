using UnityEngine;
using UnityEngine.UI;

public class InputSystemController : MonoBehaviour
{
    public static InputSystemController Instance;

    private DebugController debugController;

    [SerializeField] private GameObject mainCamera;

    [Header(" Anchor JoyStick")]
    [SerializeField] private FixedJoystick anchorJoyStick;
    private Vector3 lastAnchorJoystickPos;
    private bool anchorBeingDrag = false;

    [Header("Movement Joystick")]
    [SerializeField] private FixedJoystick movementJoystick;
    private bool moveJoyStickBeingDrag;

    [Header("Claw Rotation Slider")]
    [SerializeField] private Slider clawRotationSlider;

    private bool clawLifting;

    private Vector3 rotationDirectionPoint;


    private void Awake()
    {
        #region Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion
    }


    // Start is called before the first frame update
    void Start()
    {
        debugController = DebugController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetAnchorJoyStickUseStatus())
        {
            anchorBeingDrag = true;
            return;
        }


    }

    #region Mouse
    public bool GetMouseButton()
    {
        return (Input.GetMouseButton(0));
    }

    public Vector3 GetMeasureScreenDirection(Vector3 initialPosition)
    {
        Vector3 target = new Vector3(initialPosition.x, initialPosition.y, 0f);

        //Jika mouse dipencet dan Joystick sedang tidak digunakan
        if (Input.GetMouseButton(0) && !GetAnchorJoyStickUseStatus())
        {
            Vector3 ray = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

            Vector3 touchPoint = new Vector3(ray.x, ray.y, 0f);
            Vector3 dir = (touchPoint - target).normalized;

            return dir;
        }

        return Vector3.zero;
    }

    public Quaternion GetRotationFromScreen(Vector3 initialPosition)
    {
        //Jika mouse dipencet dan Joystick sedang tidak digunakan
        if (Input.GetMouseButton(0) && !GetAnchorJoyStickUseStatus())
        {
            Vector3 ray = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            rotationDirectionPoint = new Vector3(ray.x, ray.y, 0f);

        }

        Vector3 lookPos = rotationDirectionPoint - initialPosition;
        lookPos.z = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        return rotation;
    }
    #endregion

    #region Movement Joystick
    public Vector3 GetMovementJoyStickPosition()
    {
        Vector3 position = new Vector3(movementJoystick.Horizontal, movementJoystick.Vertical, 0f);

        return position;
    }

    public bool GetMovementJoyStickBeingDragStatus()
    {
        return moveJoyStickBeingDrag;
    }

    public void SetMovementJoyStickBeingDragStatus(bool drag)
    {
        moveJoyStickBeingDrag = drag;
    }

    public bool GetMovementJoyStickUseStatus()
    {
        if (movementJoystick.Horizontal == 0 && movementJoystick.Vertical == 0)
        {
            return false;
        }

        return true;
    }

    #endregion

    #region Anchor JoyStick
    public Vector3 GetAnchorJoyStickPosition()
    {
        Vector3 position = new Vector3(anchorJoyStick.Horizontal, anchorJoyStick.Vertical, 0f);

        return position;
    }

    public bool GetAnchorJoyStickBeingDragStatus()
    {
        return anchorBeingDrag;
    }

    public void SetAnchorJoyStickBeingDragStatus(bool drag)
    {
        anchorBeingDrag = drag;
    }

    public bool GetAnchorJoyStickUseStatus()
    {
        if (anchorJoyStick.Horizontal == 0 && anchorJoyStick.Vertical == 0)
        {
            return false;
        }

        return true;
    }

    public void SetLastAnchorJoystickPosition()
    {
        if (GetAnchorJoyStickPosition() != Vector3.zero)
            lastAnchorJoystickPos = GetAnchorJoyStickPosition();
    }

    public Vector3 GetLastAnchorJoystickPosition()
    {
        return lastAnchorJoystickPos;
    }
    #endregion


    #region LiftButton
    public void HoldButtonLift()
    {
        clawLifting = true;
    }

    public void ReleaseButtonLift()
    {
        clawLifting = false;
    }

    public bool GetClawLiftingStatus()
    {
        return clawLifting; 
    }
    #endregion

    #region Rotating Claw Slider
    public float GetValueClawSlider()
    {
        return clawRotationSlider.value;
    }

    public void SetValueClawSlider(float myValue)
    {
        clawRotationSlider.value = myValue;
    }
    #endregion
}
