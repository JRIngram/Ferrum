using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathColliderScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInteractionController>().getHit(100);
        }
    }
}
