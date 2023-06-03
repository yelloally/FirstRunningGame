using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !GameManager.Instance.motor.slideBack)
            PickupFish(); 
    }

    private void PickupFish()
    {
        anim?.SetTrigger("Pickup");
        GameStats.Instance.CollectFish();//hit command
        //incremenrr the fish count 
        //incremenr the score
        //play sfx
        //trigger an animation 
    }

    public void OnShowChunk()
    {
        anim?.SetTrigger("Idle");
    }
}
