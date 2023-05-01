using UnityEngine;

public class DeathState : BaseState
{
    //constantly apply to the player
    //private field of vector3
    [SerializeField] private Vector3 knockbackForce = new Vector3(0, 4, -3);
    //private field of vector3, w
    private Vector3 currentKockback;

    //override method from tha class BaseState 
    public override void Construct()
    {
        motor.anim?.SetTrigger("Death");
        currentKockback = knockbackForce;
    }

    //override method from tha class BaseState
    public override Vector3 ProcessMotion()
    {
        //store
        Vector3 m = currentKockback;

        //update
            currentKockback = new Vector3(0,
            currentKockback.y -= motor.gravity * Time.deltaTime,
            currentKockback.z += 2.0f * Time.deltaTime);

        //check if value Z has become +(positive)
        if(currentKockback.z > 0)
        {
            currentKockback.z = 0;
            GameManager.Instance.ChangeState(GameManager.Instance.GetComponent<GameStateDeath>());
        }

        return currentKockback;
    }

}
