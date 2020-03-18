using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public List<GameObject> NeighborList = new List<GameObject>();

    public GameObject gm;
    public GameObject child;

    public int color_Index;
    
    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("gamemanager");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "hex")//Düğümün etrafındaki komşuları belirliyoruz
        {
            if (NeighborList.Contains(null)) { NeighborList[NeighborList.IndexOf(null)] = collision.gameObject; }
            else { NeighborList.Add(collision.gameObject); }
            //Komşuları listeye ekliyoruz
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "hex")//Düğümün etrafındaki önceki komşuları belirliyoruz
        {
           NeighborList[NeighborList.IndexOf(collision.gameObject)] = null;//Komşuları ile ileşiği kesilen komşuyu listeden çıkarıyoruz.
        }
    }
}
