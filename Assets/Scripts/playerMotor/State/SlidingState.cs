using UnityEngine;

public class SlidingState : BaseState
{
    public float slideDuration = 1.0f;

    //collider logic
    private Vector3 initialCenter;
    private float initialSize;
    private float slideStart;

    //override method from tha class BaseState
    public override void Construct()
    {
        motor.anim?.SetTrigger("Slide");
        slideStart = Time.time;

        initialSize = motor.controller.height;
        initialCenter = motor.controller.center;

        motor.controller.height = initialSize * 0.5f;
        motor.controller.center = initialCenter * 0.5f;
    }

    //override method from tha class BaseState
    // restore the original size and center position of the character
    //controller when the sliding state ends
    public override void Destruct()
    {
        motor.controller.height = initialSize;
        motor.controller.center = initialCenter;
        motor.anim?.SetTrigger("Running");
    }

    //override method from tha class BaseState
    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
            motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            motor.ChangeLane(1);

        if (!motor.isGrounded)
            motor.ChangeState(GetComponent<FallingState>());

        if (InputManager.Instance.SwipeUp)
            motor.ChangeState(GetComponent<JumpimgState>());
        //check if the sliding duration has elapsed and change state to running state
        if (Time.time - slideStart > slideDuration)
            motor.ChangeState(GetComponent<RunningState>());
    }

    //override method from tha class BaseState
    public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;

        m.x = motor.SnapToLane();
        m.y = -1.0f;
        m.z = motor.baseRunSpeed;

        return m;
    }
}
