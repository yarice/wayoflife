using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InfoText : MonoBehaviour
{
    private GameManager gameManager;
    private TextMeshPro textComponent;
    private UserPreferences userPrefs;
    // Start is called before the first frame update
    void Start()
    {
        userPrefs = UserPreferences.Instance;
        gameManager=GameManager.Instance;
        textComponent = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        textComponent.text =gameManager.gameActive? "Generation: "+gameManager.generation+" | Number of alive cells: "+gameManager.aliveCounter+"/"+userPrefs.size*userPrefs.size:"Press start to begin...";
    }
}
