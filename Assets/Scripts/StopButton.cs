using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StopButton : MonoBehaviour
{
    private GameManager gameManager;

    private TextMeshPro txt;
    // Start is called before the first frame update
    void Start()
    {
        gameManager=GameManager.Instance;
        txt = gameObject.GetComponent<TextMeshPro>();
    }

    private void OnMouseDown()
    {
        gameManager.onGameActiveToggle.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = gameManager.gameActive ? "Pause" : "Start";
    }
}
