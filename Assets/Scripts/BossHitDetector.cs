using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used to manage the boss hitting the player.
 * Should be attached to the boss's HitDetector
 */

public class BossHitDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        /**
         * If the object colliding with the trigger is the player, take the boss's damage way from the player's health.
         */ 
       float damage = gameObject.transform.parent.gameObject.GetComponent<BossController>().damage;
       if (other.tag == "Player")
       {
            gameObject.GetComponent<AudioSource>().Play();
            other.GetComponent<PlayerInteractionController>().getHit(damage);
       }
    }
}
