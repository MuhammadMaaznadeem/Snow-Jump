using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
