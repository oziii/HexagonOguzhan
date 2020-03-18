using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject GameMenuPanel;
    public Text score_Text;

    public void RestartFunc()
    {
        score_Text.text = " " + GetComponent<Grid>().score;
        GameOverPanel.SetActive(false);
        GetComponent<Grid>().create_Hex_Map();
    }

    public void MenuFunc()
    {
        score_Text.text = " " + GetComponent<Grid>().score;
        GameMenuPanel.SetActive(true);
        GameOverPanel.SetActive(false);
    }

    public void StartFunc()
    {
        score_Text.text = " " + GetComponent<Grid>().score;
        GameMenuPanel.SetActive(false);
        GetComponent<Grid>().create_Hex_Map();
    }
}
