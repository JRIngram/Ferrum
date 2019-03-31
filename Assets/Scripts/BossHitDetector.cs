using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIIIIIIIIT");
       float damage = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>().damage;
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInteractionController>().getHit(damage);
        }
    }
}
