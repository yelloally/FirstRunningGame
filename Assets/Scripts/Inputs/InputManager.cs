using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputManager : MonoBehaviour
{
    //there should be one InputManager in the scene
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }

    //object 
    //action schemes
    private RunnerInputAction actionScheme;

    // parameter 
    //configuration
    [SerializeField] private float sqrSwipeDeadZone = 50.0f;

    #region public properties
    public bool Tap { get { return tap; } }
    public Vector2 TouchPosition { get { return touchPosition; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    #endregion

    #region privates
    private bool tap;
    private Vector2 touchPosition;
    private Vector2 startDrag;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;
    #endregion

    //method 
    private void Awake()
    {
        instance = this;
        //when the object will be spawn anywhere in the map, it is going to
        //set itself as the static instance
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }

    //method
    private void LateUpdate()
    {
        ResetsInputs();
    }

    //method
    private void ResetsInputs()
    {
        tap = swipeDown = swipeLeft = swipeRight = swipeUp = false;
    }


    //extension of the Awake 
    private void SetupControl()
    {
        actionScheme = new RunnerInputAction();

        //register different actions
        actionScheme.Gameplay.Tap.performed += ctx => OnTap(ctx);
        actionScheme.Gameplay.TouchPosition.performed += ctx => OnPosition(ctx);
        actionScheme.Gameplay.StartDrag.performed += ctx => OnStartDrag(ctx);
        actionScheme.Gameplay.EndDrag.performed += ctx => OnEndDrag(ctx);

    }

    //method 
    private void OnEndDrag(InputAction.CallbackContext ctx)
    {
        Vector2 delta = touchPosition - startDrag;
        float sqrDistaance = delta.sqrMagnitude;

        //confirmed swipe
        if (sqrDistaance > sqrSwipeDeadZone)
        {
            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);

            if (x > y) //left or right
            {
                if (delta.x > 0)
                    swipeRight = true;
                else
                    swipeLeft = true;
            }
            else      //up or down 
            {
                if (delta.y > 0)
                    swipeUp = true;
                else
                    swipeDown = true;
            }
        }
            startDrag = Vector2.zero;
    }

    //methods that are called in response to their respective input signals
    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        startDrag = touchPosition;
    }
    private void OnPosition(InputAction.CallbackContext ctx)
    {
        touchPosition = ctx.ReadValue<Vector2>();
    }
    private void OnTap(InputAction.CallbackContext ctx)
    {
        tap = true;
    }

    public void OnEnable()
    {
        actionScheme.Enable();
    }

    public void OnDisable()
    {
        actionScheme.Disable();
    }
}
