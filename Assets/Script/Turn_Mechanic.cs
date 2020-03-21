using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Mechanic : MonoBehaviour
{
    [Header("List")]
    public List<GameObject> turn_Group_Array = new List<GameObject>();
    public List<GameObject> same_Color_Hex = new List<GameObject>();
    public List<GameObject> destroy_List = new List<GameObject>();

    [Header("Vector2")]
    Vector2 distance , neigbor_Vec;

    [Header("Float")]
    float distance2;

    [Header("Gameobject")]
    public GameObject gm;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("gamemanager");
    }
    public void find_Near_Hex(Vector2 touch_pos, GameObject hex)
    {
        try
        {
            foreach (GameObject e in turn_Group_Array) { e.GetComponent<Hex>().child.SetActive(false); }
        turn_Group_Array.Clear();
         distance = new Vector2(hex.GetComponent<Hex>().NeighborList[0].transform.position.x, hex.GetComponent<Hex>().NeighborList[0].transform.position.y);}
        catch (NullReferenceException) {}
        distance2 = Vector2.Distance( distance, touch_pos);
        turn_Group_Array.Add(hex.GetComponent<Hex>().NeighborList[0]);
        turn_Group_Array.Add(hex);


        //Dokunulan objenin içinde mouse positiona en yakın komşu
        foreach (GameObject e in hex.GetComponent<Hex>().NeighborList)
        {
            if (e != null) { neigbor_Vec = new Vector2(e.transform.position.x, e.transform.position.y); }
            if (Vector2.Distance(neigbor_Vec, touch_pos) < distance2)
            {
                turn_Group_Array[0] = e;
                distance2 = Vector2.Distance(touch_pos, neigbor_Vec);
            }
        }
        find_three_Group(hex);
    }

    void find_three_Group(GameObject hex)
    {
        // en yakın komşu ile ortak komşu ve dokunulan 3 objeyi listeye ekle
        /* listede 
        0 = en yakın komşu 
        1 = dokunulan obje 
        2 = ortak komşu 
        */
        foreach (GameObject e in hex.GetComponent<Hex>().NeighborList)
        {
            if (turn_Group_Array[0].GetComponent<Hex>().NeighborList.Contains(e)) { turn_Group_Array.Add(e);  break; };
        }
        foreach(GameObject e in turn_Group_Array) { e.GetComponent<Hex>().child.SetActive(true); }
        StartCoroutine(turn_Clockwise());//bittiğinde sil
    }

    public IEnumerator  turn_Clockwise()
    {
        //Dödnürme işlemi
        Vector2 start_Pos;
        for (int i = 0; i < turn_Group_Array.Count; i++)
        {
            start_Pos = turn_Group_Array[1].transform.position;
            turn_Group_Array[1].transform.position = turn_Group_Array[2].transform.position;
            turn_Group_Array[2].transform.position = turn_Group_Array[0].transform.position;
            turn_Group_Array[0].transform.position = start_Pos;

            yield return new WaitForSeconds(0.5f);
            same_color_Control(turn_Group_Array);
            if (turn_Group_Array.Count == 0) {  GetComponent<Grid>().gameOver(); break; }

        }
    }

    public IEnumerator turn_Unclockwise()
    {
        Vector2 start_Pos;
        for (int i = 0; i < turn_Group_Array.Count; i++)
        {
            start_Pos = turn_Group_Array[1].transform.position;
            turn_Group_Array[1].transform.position = turn_Group_Array[0].transform.position;
            turn_Group_Array[0].transform.position = turn_Group_Array[2].transform.position;
            turn_Group_Array[2].transform.position = start_Pos;
            
            yield return new WaitForSeconds(0.5f);
            same_color_Control(turn_Group_Array);
            if (turn_Group_Array.Count == 0) { GetComponent<Grid>().gameOver(); break; }
        }
    }

    public void same_color_Control(List<GameObject> list)
    {
        //her dönüşte üçgen oluşmuş mu kontrolü ve yukardan yeni düşen objelerin üçgen oluşturmuş mu kontrolü
        destroy_List.Clear();
        same_Color_Hex.Clear();
        try
        {
            foreach(GameObject e in list)
            {
                //objenin komşuları içinde aynı renk indexine sahip komşu aranıyor.
                foreach(GameObject g in e.GetComponent<Hex>().NeighborList)
                {   
                    if (g != null && e.GetComponent<Hex>().color_Index == g.GetComponent<Hex>().color_Index)
                    {
                        //aynı renk indexine sahip komşular same_Color_Hex listesine ekleniyor.
                        if (same_Color_Hex.Contains(e) == false) { same_Color_Hex.Add(e); }
                        same_Color_Hex.Add(g);                   
                    }
                }
                if(same_Color_Hex.Count > 1)
                {
                    for (int i = 1; i < same_Color_Hex.Count - 1; i++)
                    {
                        //aynı renk indexine sahip objeler komşu mu kontrolü yapılıyor.
                        if (same_Color_Hex[i].GetComponent<Hex>().NeighborList.Contains(same_Color_Hex[i + 1]))
                        {
                            
                            foreach (GameObject ej in turn_Group_Array) { ej.GetComponent<Hex>().child.SetActive(false); }
                            try
                            {
                                StartCoroutine(gm.GetComponent<Grid>().hex_Destroy_Create(same_Color_Hex[i + 1]));
                                StartCoroutine(gm.GetComponent<Grid>().hex_Destroy_Create(same_Color_Hex[i]));
                                same_Color_Hex.Clear();
                                StartCoroutine(gm.GetComponent<Grid>().hex_Destroy_Create(e));

                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                break;
                            }

                            if(list != GetComponent<Grid>().hex_Array) { list.Clear(); }
                        
                        }
                    }
                }
                same_Color_Hex.Clear();
                if (list.Count == 0) { break;} 
            }
        }
        catch (InvalidOperationException) { }
    }
}
