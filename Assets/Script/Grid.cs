using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject gHexagon;
    public GameObject nobj;
    public GameObject BombHexagon;
    public GameObject GameOverPanel;

    [Header("Slider")]
    public Slider rowCount;
    public Slider columnCount;
    public Slider colorCount;

    [Header("İnt")]
    public int iGridRow;
    public int iGridColumn;
    public int iColorListCount;
    public int _counter;
    public int score;
    public int Bombindex;
    int counter = 6;
    int random;
    int reward = 5;

    [Header("Float")]
    float fXiteration = 0.77f;
    float fYiteration = 0.89f;
    float yAdd;

    [Header("Bool")]
    bool BombHexIs = false;

    [Header("Text")]
    public Text score_Text;

    [Header("List")]
    public List<Color> colorList = new List<Color>();
    public List<int> color_Count_Array = new List<int>();
    public List<GameObject> hex_Array = new List<GameObject>();
    public List<GameObject> same_Color = new List<GameObject>();
    public List<GameObject> destroy_List = new List<GameObject>();
    public List<GameObject> hex_Row_Array = new List<GameObject>();

    private void Update()
    {
        //slide ayarları
        iGridRow = (int)rowCount.value;
        iGridColumn = (int)columnCount.value;
        iColorListCount = (int)colorCount.value;
    }

    public void create_Hex_Map()
    {
        //Hexagon mapı yaratır ve kontrol eder.
        for (float x = 0f; x < iGridColumn; x++)
        {
            if (x % 2 == 0)
            {
                yAdd = 0.0f;
            }
            else
            {
                yAdd = -0.45f;
            }
            for (float y = 0f; y < iGridRow; y++)
            {
                float xPos = x * fXiteration;
                float yPos = y * fYiteration + yAdd;
                array_add_Hex(create_Hex(xPos, yPos));
            }
        }
        for (int i = iGridRow; i < (hex_Array.Count); i += (iGridRow * 2))
        {
            for (int y = 0; y < iGridRow; y++) { StartCoroutine(hex_Choice_Color(hex_Array[i + y])); }
        }
    }
    GameObject create_Hex(float x ,float y)
    {
        //Hex yaratır
        nobj = (GameObject)GameObject.Instantiate(gHexagon);
        nobj.transform.position = new Vector2(x, y);
        nobj.name ="x: "+ x + ", y: " + y;
        random = UnityEngine.Random.Range(0, iColorListCount);
        nobj.GetComponent<Hex>().color_Index = random;
        nobj.GetComponent<SpriteRenderer>().color = new Color(colorList[random].r, colorList[random].g, colorList[random].b);
        nobj.SetActive(true);
        Rigidbody2D nobjRigid = nobj.AddComponent<Rigidbody2D>();
        nobjRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        return nobj;
    }

     public IEnumerator hex_Choice_Color(GameObject hex)
    {
        //hex renk seçimini yapar oyunun ilk başladığında 3 aynı rengin bir üçgen oluşturmaması için
        yield return new WaitForSeconds(0.01f);
        for (int i= 0; i < iColorListCount; i++) { color_Count_Array.Add(0); }
        foreach (GameObject e in hex.GetComponent<Hex>().NeighborList)
        {
            color_Count_Array[e.GetComponent<Hex>().color_Index] += 1;
        }
        int index = color_Count_Array.IndexOf(color_Count_Array.Min());
        hex.GetComponent<Hex>().color_Index = index;
        hex.GetComponent<SpriteRenderer>().color = new Color(colorList[index].r, colorList[index].g, colorList[index].b);
        color_Count_Array.Clear();
    }
    void array_add_Hex(GameObject hex)
    {
        //Listeye hexagonlar eklenir.
        hex_Array.Add(hex);
    }

    public IEnumerator hex_Destroy_Create(GameObject destory_hex)
    {
        
        int hex_index = hex_Array.IndexOf(destory_hex);
        float x = destory_hex.transform.position.x;
        hex_Row_Array.Clear();
        //arrayden aynı destroy x olanları çek bir destroy_grid_arraye aktar detroy y den büyük olanların y sini fYiteration kadar azalt
        foreach (GameObject e in hex_Array)
        {
            if (e.transform.position.x == x &&
                e.transform.position.y > destory_hex.transform.position.y) 
            { hex_Row_Array.Add(e); }
        }
        if(destory_hex.name == "BombHexagon") { BombHexIs = false; }
        Destroy(destory_hex);
        Score();

        //destroy sonucu oluşan boşluğu doldurma
        foreach (GameObject e in hex_Row_Array)
        {
            e.transform.position = new Vector2(x , e.transform.position.y - fYiteration);
        }

        //destroy olmuş gameobjectin x hizasında yeni obje create edicez.
        float y;
        if ((x / fXiteration) % 2 == 0) { y = (iGridRow-1) * fYiteration; } else { y = (iGridRow-1) * fYiteration - 0.45f; }
        if (score % 1000 == 0) { reward = 5;/*sil*/ Bombindex = hex_index; hex_Array[hex_index] = bombHexagon(x,y);  } 
        else { hex_Array[hex_index] = create_Hex(x, y); }
        
        //hex_Row_Array.Add(hex_Array[hex_index]);
        yield return new WaitForSeconds(0.5f);
        if (GetComponent<Turn_Mechanic>().same_Color_Hex.Count == 0) { GetComponent<Turn_Mechanic>().turn_Group_Array.Clear(); GetComponent<Turn_Mechanic>().same_color_Control(hex_Array); }
        hex_Row_Array.Clear();

    }

    void Score()
    {
        //Skor puanı text olarak yazılır ve bir int değerde tutulur.
        score += reward;
        score_Text.text = " " + score;
    }

    GameObject bombHexagon(float x, float y) 
    {
        //bombHexagon Create
        BombHexIs = true;
        nobj = (GameObject)GameObject.Instantiate(BombHexagon);
        nobj.transform.position = new Vector2(x, y);
        nobj.name = "BombHexagon";
        random = UnityEngine.Random.Range(0, iColorListCount);
        nobj.GetComponent<Hex>().color_Index = random;
        nobj.GetComponent<SpriteRenderer>().color = new Color(colorList[random].r, colorList[random].g, colorList[random].b);
        nobj.SetActive(true);
        _counter = counter;
        nobj.GetComponentInChildren<TextMeshPro>().text = _counter.ToString();
        Rigidbody2D nobjRigid = nobj.AddComponent<Rigidbody2D>();
        nobjRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        return nobj;
    }

    public void gameOver() 
    {
        //her turda bombHexagon patladı mı diye bakar.
        if (BombHexIs)
        {
            _counter--;
            hex_Array[Bombindex].GetComponentInChildren<TextMeshPro>().text = _counter.ToString();
            if (_counter == 0)
            {
                GameOverPanel.SetActive(true);
                foreach(GameObject e in hex_Array) { Destroy(e); }
                hex_Array.Clear();
                score = 0;
                BombHexIs = false;
            }
        }
    }
}

