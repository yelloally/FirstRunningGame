using UnityEngine;

public class JumpimgState : BaseState
{
    public float jumpForce = 7.0f;

    //override method from tha class BaseState
    public override void Construct()
    {
        motor.verticalVelocity = jumpForce;
        motor.anim?.SetTrigger("Jump");
    }

    //override method from tha class BaseState
    public override Vector3 ProcessMotion()
    {
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
        if (InputManager.Instance.SwipeLeft)
            motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            motor.ChangeLane(1);

        if (motor.verticalVelocity < 0)
            motor.ChangeState(GetComponent<FallingState>());

    }
}
