using UnityEngine;

public class FallingState : BaseState
{
    //override method from tha class BaseState
    public override void Construct()
    {
        motor.anim?.SetTrigger("Fall");
    }

    //override method from tha class BaseState
    public override Vector3 ProcessMotion()
    {
        //method in the motor component
        //apply gravity
        motor.ApplyGravity();

        //create return vector
        Vector3 m = Vector3.zero;

        m.x = motor.SnapToLane();
        m.y = motor.verticalVelocity;
        m.z = motor.baseRunSpeed;

        return m;
    }

    //override method from tha class BaseState
    public override void Transition()
    {
        //properties
        if (InputManager.Instance.SwipeLeft)
            motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            motor.ChangeLane(1);

        if (motor.isGrounded)
            motor.ChangeState(GetComponent<RunningState>());
    }
}
