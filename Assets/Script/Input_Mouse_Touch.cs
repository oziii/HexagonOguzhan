using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Input_Mouse_Touch : MonoBehaviour

{
    public GameObject gm;
    private Vector2 touch_pos;
    private Vector2 endTouch_pos;
    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("gamemanager");
    }
    public void OnMouseOver() //Mouse tıklandığında işleve giriyor
    {
        if (Input.GetMouseButtonDown(0))
        {
            touch_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // mouse position
            gm.GetComponent<Turn_Mechanic>().find_Near_Hex(touch_pos, gameObject);
        }
        if (Input.GetMouseButtonUp(0))
        {
            endTouch_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (endTouch_pos.x > gameObject.transform.position.x || endTouch_pos.y > gameObject.transform.position.y) { StartCoroutine(gm.GetComponent<Turn_Mechanic>().turn_Clockwise()); }    
            else { StartCoroutine(gm.GetComponent<Turn_Mechanic>().turn_Unclockwise()); }
               
        }
    }
}

