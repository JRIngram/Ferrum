using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractionController : MonoBehaviour {
    public Sprite crosshairImage;
    public Sprite crosshairSelectable;
    public Image crosshair;
    public GraphicRaycaster graphicRaycaster;

    void Update()
    {
        PhysicsRaycasts();
    }

    void PhysicsRaycasts()
    {
        Vector3 centreOfScreen = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);
        float distanceToFireRay = 20;
        Ray centreOfScreenRay = Camera.main.ScreenPointToRay(centreOfScreen);
        RaycastHit hit;
        if (Physics.Raycast(centreOfScreenRay, out hit, distanceToFireRay))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.tag == "Enemy")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    crosshair.color = Color.red;
                    Debug.Log("Raycast hit: " + hit.transform.name);
                    if (hitObject.tag == "Enemy")
                    {
                        hitObject.GetComponent<BasicRobotController>().onHit();
                        Debug.Log(hitObject);
                        Debug.Log("Bang bang!");
                        //hitObject.onHit();
                    }
                }
                ToggleSelectedCursor(true);
            }
            else
            {
                ToggleSelectedCursor(false);
            } 
        }
        else
        {
            ToggleSelectedCursor(false);
        }

    }

    void GraphicsRaycasts()    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);
        bool hitButton = false;
        if(results.Count > 0)
        {
            for(int i = 0; i < results.Count; i++)
            {
                Button button = results[i].gameObject.GetComponent<Button>();
                if(button != null)
                {
                    hitButton = true;
                    if(Input.GetMouseButtonDown(0)) button.onClick.Invoke();
                }
            }
            if(hitButton)
            {
               ToggleSelectedCursor(true);
                /* change this to the function that you use to
                                                            change the cursor */
            }
        }
    }

    void ToggleSelectedCursor(bool interactable)
    {
        if (interactable)
        {
            crosshair.color = Color.yellow;
        }
        else {
            crosshair.color = Color.green;
        }
    }
}
