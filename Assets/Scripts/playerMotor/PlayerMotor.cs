using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    //how far  away are we were going to move at this frame
    [HideInInspector] public Vector3 moveVector;
    //all about velocity, when jumping or falling 
    [HideInInspector] public float verticalVelocity;
    //whether or not we are touching the floor
    [HideInInspector] public bool isGrounded;
    //move left(-1)/straight(0)/right(1)
    [HideInInspector] public int currentLane;

    //conffigurable fields
    public float distanceInBetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;
    public float baseSidewaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;

    //reference
    //object
    public CharacterController controller;
    //jbject
    public Animator anim;
    //object
    private BaseState state;
    //object
    private bool isPaused;

    //method from tha class MonoBehaviour
    private void Start()
    {
        //get references and initialize state
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        state = GetComponent<RunningState>();
        state.Construct();

        isPaused = true; //player is paused at start
    }

    //method from the class MonoBehaviour
    private void Update()
    {
        //only update if not paused
        if (!isPaused)
            UpdateMotor();
    }

    //method which updates player.s movement checks if the player is on the ground
    private void UpdateMotor()
    {
        //check if we were grounded
        isGrounded = controller.isGrounded;

        //how should we moving rn? based on state
        moveVector = state.ProcessMotion();

        //are we trying to change state?
        state.Transition();

        //feed animator some values
        anim?.SetBool("IsGrounded", isGrounded);
        anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));

        //move the player
        controller.Move(moveVector * Time.deltaTime);
    }
    //method
    public float SnapToLane()
    {
        float r = 0.0f;

        //if we're not directly on top of a lane
        if (transform.position.x != (currentLane * distanceInBetweenLanes)) 
        {
            //calculate the distance to desired position
            float deltaToDesiredPosition = (currentLane * distanceInBetweenLanes) - transform.position.x;
            //determine direction to move
            r = (deltaToDesiredPosition > 0) ? 1 : -1;
            r *= baseSidewaySpeed;

            //calculate the actual distance travelled to snap to the lane
            float actualDistance = r * Time.deltaTime;
            if (Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition))
                r = deltaToDesiredPosition * (1 / Time.deltaTime);

            
        }
        else
        {
            //already in the correct lane
            r = 0;
        }

        return r;
    }

    //to change player lane
    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp( currentLane + direction, -1, 1);
    }
    //function to change player lane
    public void ChangeState(BaseState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }
    public void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity < -terminalVelocity)
            verticalVelocity = -terminalVelocity;
    }

    public void PausePlayer()
    {
        isPaused = true;
    }
    public void ResumePlayer()
    {
        isPaused = false;
    }
    public void RespawnPlayer()
    { 
        ChangeState(GetComponent<RespawnState>());
        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }
    public void ResetPlayer()
    {
        currentLane = 0;
        transform.position = Vector3.zero;
        anim?.SetTrigger("Idle");
        ChangeState(GameManager.Instance.motor.GetComponent<RunningState>());
        PausePlayer();
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);

        if (hitLayerName == "Death")
            ChangeState(GetComponent<DeathState>()); 
    }
}