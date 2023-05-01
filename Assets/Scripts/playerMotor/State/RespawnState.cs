using UnityEngine; 

public class RespawnState : BaseState
{
    //private variables 
    [SerializeField] private float verticalDistance = 25.0f;
    [SerializeField] private float immunityTime = 1f;

    private float startTime;

    //method
    public override void Construct()
    {
        startTime = Time.time;

        //disable the controller, set the player's position above the ground, then enable the controller
        motor.controller.enabled = false;
        motor.transform.position = new Vector3(0, verticalDistance, motor.transform.position.z);
        motor.controller.enabled = true;

        motor.verticalVelocity = 0.0f;
        motor.currentLane = 0;
        motor.anim?.SetTrigger("Respawn"); 
    }

    //method
    public override void Destruct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Game);
    }

    //method 
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

    //method 
    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
            motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            motor.ChangeLane(1);

        if (motor.isGrounded && (Time.time - startTime) > immunityTime)
            motor.ChangeState(GetComponent<RunningState>());

    }
}
