using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRobotController : MonoBehaviour
{
    public int scoreValue = 100;
    private int health = 100;
    public bool onHit() {
        health = health - 50;
        if (health == 0)
        {
            Destroy(gameObject);
            return true;
        }
        else {
            return false;
        }
    }
}
