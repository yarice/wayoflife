using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    private GameManager gameManager;



    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onGameActiveToggle.AddListener(() => { gameObject.SetActive(!gameManager.gameActive);});
    }


    private void OnMouseDown()
    {
        gameManager.onRestartClick.Invoke();
    }
}


