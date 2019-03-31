using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       float damage = gameObject.transform.parent.gameObject.GetComponent<BossController>().damage;
       if (other.tag == "Player")
       {
            gameObject.GetComponent<AudioSource>().Play();
            other.GetComponent<PlayerInteractionController>().getHit(damage);
       }
    }
}
