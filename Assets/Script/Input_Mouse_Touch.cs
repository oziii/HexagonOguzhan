using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Input_Mouse_Touch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler

{
    public GameObject gm;
    private Vector2 downHand;
    private Vector2 upHand;
    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("gamemanager");
    }
    public void OnMouseOver() //Mouse tıklandığında işleve giriyor
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touch_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // mouse position
            gm.GetComponent<Turn_Mechanic>().find_Near_Hex(touch_pos, gameObject);
            gm.GetComponent<Turn_Mechanic>().turn_Clockwise();

        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        downHand = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        upHand = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        upHand = eventData.position;

            if (downHand == upHand)
            {
                    Vector2 touch_pos = Camera.main.ScreenToWorldPoint(upHand);

                    RaycastHit2D hit = Physics2D.Raycast(touch_pos, Vector2.zero);
                    if (hit.collider != null)
                         gm.GetComponent<Turn_Mechanic>().find_Near_Hex(touch_pos, hit.collider.gameObject);
                        
            }
            else
            {
                if (upHand.x > downHand.x || upHand.y > downHand.y)
                    StartCoroutine(gm.GetComponent<Turn_Mechanic>().turn_Clockwise());
                else
                    StartCoroutine(gm.GetComponent<Turn_Mechanic>().turn_Unclockwise());
            }
        
    }
}

