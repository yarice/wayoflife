using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


namespace WayOfLife.View
{


    public class InfoText : MonoBehaviour
    {


        // Start is called before the first frame update

        public void Render(bool gameActive, int generation, int aliveCounter, int size)
        {
            GetComponent<TextMeshPro>().text = gameActive
                ? "Generation: " + generation + " | Number of alive cells: " + aliveCounter + "/" + size * size
                : "Press start to begin...";
        }
    }
}
    



