using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected PlayerMotor motor;
    // method
    //first enter the state 
   public virtual void Construct() { }
    //method
    //leave the state 
   public virtual void Destruct() { }
    // method
    //update loop 
   public virtual void Transition() { }

    //constructor 
    //called when the script instance is being loaded
    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }
    //method
    // returns a vector3
    public virtual  Vector3 ProcessMotion()
    {
        Debug.Log("Process motion is not implemented in " + this.ToString());
        return Vector3.zero;
    }
}
