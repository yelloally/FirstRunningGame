using UnityEngine;

public class RunningState : BaseState
{
    //override method from tha class BaseState
    public override void Construct()
    {
        //set vertical velocity to 0, since the player is on the ground
        motor.verticalVelocity = 0;
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

    //override method from tha class BaseState
    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
            motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            motor.ChangeLane(1);

        if (InputManager.Instance.SwipeUp && motor.isGrounded)
            motor.ChangeState(GetComponent<JumpimgState>());

        if (!motor.isGrounded)
            motor.ChangeState(GetComponent<FallingState>());

        if (InputManager.Instance.SwipeDown)
            motor.ChangeState(GetComponent<SlidingState>());

    }
}
