using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRobotController : MonoBehaviour
{

    private int health = 100;
    public void onHit() {
        health = health - 50;
        Debug.Log("YAAAAAAAAAOW");
        if(health == 0){
            Destroy(gameObject);
        }
    }
}
