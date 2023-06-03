using System.Collections;
using System.Collections.Generic;
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
    private int fishCount = 0;

    //reference
    //object
    public CharacterController controller;
    public Vector3 startingPos;
    //jbject
    public Animator anim;
    //object
    private BaseState state;
    //object
    private bool isPaused;
    public bool slideBack;
    public AudioClip hitSound;

    public GameObject fish;

    private float timer;
    private float storeInterval;
    public List<Vector3> locationPoints;
    private int currentIndex;

    //method from tha class MonoBehaviour
    private void Start()
    {
        storeInterval = Time.deltaTime;
        //initialize the list
        locationPoints = new List<Vector3>();

        //get references and initialize state
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        state = GetComponent<RunningState>();
        state.Construct();

        isPaused = true; //player is paused at start

        currentIndex = 0;
    }

    public void changeSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }

    //method from the class MonoBehaviour
    float seconds;
    private void Update()
    {
            seconds++;
        //only update if not paused
        if (!isPaused)
            UpdateMotor();
    }

    //method which updates player.s movement checks if the player is on the ground
    private void UpdateMotor()

    {
        //increment the timer
        

        //check if we were grounded
        if (!slideBack)
        {
            timer += Time.deltaTime;

            //check if timer exceed store interval
            if (timer >= storeInterval)
            {
                //add the current pos to the list 
                locationPoints.Add(transform.position);

               // Debug.Log("Added lod points: " + transform.position);

                //reset
                timer = 0f;
            }

            isGrounded = controller.isGrounded;

            //how should we moving rn? based on state
            moveVector = state.ProcessMotion();

            //are we trying to change state?
            state.Transition();

            //feed animator some values
            anim?.SetBool("IsGrounded", isGrounded);
            anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));
            controller.Move(moveVector * Time.deltaTime);
            //move the player

            //Debug.Log("ccccc  " + moveVector);
        }

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

    public void SlidePlayer()
    {
        ChangeState(GetComponent<SlidingState>());
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

    private void StartGoingThroughPoints()
    {
        if (locationPoints.Count > 0)
        {
            //reset the current index
            currentIndex = 0;

            //start the coroutine 
            StartCoroutine(MoveThroughPointsCoroutine());
        }
        else
        {
            Debug.Log("no location points available.");
        }
    }

    private IEnumerator MoveThroughPointsCoroutine()
    {
        Debug.Log("Moving through points started");
        int fs = 0;
        while (currentIndex < locationPoints.Count)
        {
            //move the character towards the current point
            Vector3 targetPosition = locationPoints[currentIndex];
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = baseRunSpeed;
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            //check if we have reached the current point
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget <= 0.1f)
            {
                fs++;
                if(fs%60 == 0)
                {
                    GameObject f = Instantiate(fish, transform.position, Quaternion.identity);
                    f.GetComponent<Animator>().SetTrigger("popping");
                }
                //move to the next point
                currentIndex++;

                Debug.Log("Reached point " + fs);
                yield return new WaitForFixedUpdate(); //add a delay between each point
            }

            yield return null;
        }

        Debug.Log("finished moving through points");

        //reset the state
        currentIndex = 0;
        locationPoints.Clear();
        rePosition();
    }

    public void rePosition()
    {
        //moveVector = Vector3.zero;
        GameManager.Instance.ChangeState(GameManager.Instance.GetComponent<GameStateDeath>());
        //ResetPlayer();
        anim?.SetTrigger("Idle");
        anim?.SetFloat("Speed", 0);
        controller.Move(Vector3.zero);
        //reset the game session 
        GameStats.Instance.ResetSession();
        //pause playermotor
        GameManager.Instance.motor.PausePlayer();
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);

        if (hitLayerName == "Death" && !slideBack)
        {
            locationPoints.Clear();
            Debug.Log("dead");
            AudioManager.Instance.PlaySFX(hitSound, 0.7f);
            ChangeState(GetComponent<DeathState>());
        }

        if (slideBack)//this if will execute when pengi hit lava
        {
                if (hit.gameObject.name != "Lava")
                {
                   // hit.gameObject.GetComponent<Collider>().isTrigger = true;
                }
        }
  //      Debug.Log(hit.gameObject.name);

        if(hit.gameObject.name == "Lava")
        {//Is the fish collecting process working???

            fishCount--;
            GameStats.Instance.resetFish();
            Debug.Log("hit Lava");
            //startingPos = new Vector3(0, 0, -(seconds / 60));
//            Debug.Log(startingPos+"  "+seconds+" "+Application.targetFrameRate);

            //trigger the sliding anim
            anim?.SetTrigger("Slide");
            slideBack = true;
            // Debug.Log(state.ProcessMotion());

            //reverse the order of loc points
            locationPoints.Reverse();
            //start going throgh the points again
            StartGoingThroughPoints();
            //print out pos
            foreach (Vector3 p in locationPoints)
            {
                Debug.Log("posssss   "+p);
            }
        }

    }

}