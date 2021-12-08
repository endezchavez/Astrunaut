using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance { get { return _instance; } }

    [SerializeField]
    private float swipeThreshold = 1.2f;

    InputMaster controls;

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
            return delta.y < -swipeThreshold;
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
            return delta.y > swipeThreshold;
        }
        return false;

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
