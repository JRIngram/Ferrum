using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Debug.Log("entered");
            gameObject.GetComponent<Animator>().SetBool("BossZone", true);
        }
    }
}
