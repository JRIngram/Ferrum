using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRobotController : MonoBehaviour
{
    public void onHit() {
        Debug.Log("YAAAAAAAAAOW");
        Destroy(gameObject);
    }
}
