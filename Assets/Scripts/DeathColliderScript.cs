using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Should be attached to an object with a trigger collider that kills the player once the player collides with it.
 */

public class DeathColliderScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        /**
         * If the object that collides with the trigger, then remove 100 hitpoints (the max player's health).
         */ 
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInteractionController>().getHit(100);
        }
    }
}
