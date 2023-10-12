using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void pause()
    {
        Time.timeScale = 0;
    }

    public void TimeScaleReset()
    {
        Time.timeScale = 1;
    }
    public void RestartLev()
    {
        TimeScaleReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
