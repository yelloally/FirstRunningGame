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
        if (other.tag == "Player")
            PickupFish(); 
    }

    private void PickupFish()
    {
        anim?.SetTrigger("PickUp");
        GameStats.Instance.CollectFish();
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
