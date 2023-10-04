using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject[] Levels;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject lev in Levels)
        {
            lev.SetActive(false);
        }
        Levels[GameConstant.LevelNumber].SetActive(true);
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
