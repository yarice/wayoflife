
using TMPro;
using UnityEngine;

public class ThreeOptionsToggle : MonoBehaviour
{

    // Start is called before the first frame update
    public Options.changableValues valueToChange;
    private int selectedOption= 0;
    private GameManager gameManager;
    private UserPreferences userPreferences;

    void Start()
    {
        userPreferences=UserPreferences.Instance;
        gameManager = GameManager.Instance;
        gameManager.onGameActiveToggle.AddListener(()=>{gameObject.SetActive(!gameManager.gameActive || valueToChange==Options.changableValues.Speed);});
    }

    void Update()
    {
        switch (valueToChange)
        {
            case Options.changableValues.Prob:
                selectedOption = userPreferences.prob;
                break;
            case Options.changableValues.Size:
                selectedOption = userPreferences.size;
                break;
            case Options.changableValues.Speed:
                selectedOption = userPreferences.speed;
                break;
        }
        gameObject.GetComponent<TextMeshPro>().text = valueToChange+":"+selectedOption;
    }
    

    private void OnMouseDown()
    {
        switch (valueToChange)
            {
                case Options.changableValues.Prob:
                      gameManager.onProbChanged.Invoke();
                      break;
                case Options.changableValues.Size:
                       gameManager.onSizeChanged.Invoke();
                    break;
                case Options.changableValues.Speed:
                       gameManager.onSpeedChanged.Invoke();
                    break;
            }
    }
}


