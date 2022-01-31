using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    #endregion



    private static InputManager _instance;

    public static InputManager Instance { get { return _instance; } }

    [SerializeField]
    private float swipeThreshold = 1.2f;

    InputMaster controls;

    private Camera cam;

    [HideInInspector]
    public SwipeDetection swipeDetection;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        controls = new InputMaster();

        Input.backButtonLeavesApp = true;

        cam = Camera.main;

        swipeDetection = GetComponent<SwipeDetection>();
    }

    private void Start()
    {
        controls.Player.Touch.started += ctx => StartTouchPrimary(ctx);
        controls.Player.Touch.canceled += ctx => EndTouchPrimary(ctx);
    }

    public bool PlayerShotThisFrame()
    {
        return controls.Player.Shoot.triggered;
    }

    public bool PlayerSlidThisFrame()
    {
        if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            return controls.Player.Slide.triggered;
        }else if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (!controls.Player.Touch.triggered)
            {
                return false;
            }
            Vector2 delta = controls.Player.Swipe.ReadValue<Vector2>();
            return delta.y < -Screen.height / 500;
        }
        return false;
    }

    public bool PlayerJumpedThisFrame()
    {
        
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            return controls.Player.Jump.triggered;
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (!controls.Player.Touch.triggered)
            {
                return false;
            }
            Vector2 delta = controls.Player.Swipe.ReadValue<Vector2>();
            return delta.y > Screen.height / 500;
        }
        return false;

    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if(OnStartTouch != null)
        {

            OnStartTouch(ScreenToWorld(cam, controls.Player.TouchPos.ReadValue<Vector2>()), (float)context.startTime);
        }
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
        {
            OnEndTouch(ScreenToWorld(cam, controls.Player.TouchPos.ReadValue<Vector2>()), (float)context.time);
        }
    }

    Vector2 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
