using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelCompleteTrigger : MonoBehaviour
{
    public GameObject UI;

    private void Start()
    {
        UI = GameObject.FindObjectOfType<UiTag>().gameObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        GameConstant.LevelNumber++;

        if (GameConstant.LevelNumber > 2)
        {
            GameConstant.LevelNumber = Random.Range(0, 3);
        }
        UI.SetActive(false);
        StartCoroutine(LevelLoadDelay());
    }

    IEnumerator LevelLoadDelay()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}

