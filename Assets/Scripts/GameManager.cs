using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private Options options;
    private UserPreferences userPrefs;
    public static GameManager Instance;
    public bool gameActive;
    public UnityEvent onRestartClick;
    public UnityEvent onGameActiveToggle;
    public UnityEvent onProbChanged;
    public UnityEvent onSizeChanged;
    public UnityEvent onSpeedChanged;
    public int generation = 1;
    public int aliveCounter;
    public Grid grid;
    void Awake()
    {
        if(Instance == null) 
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if(Instance != this) 
        {
            Destroy(gameObject); 
        }
    }
    void Start()
    {
        options = Options.Instance;
        userPrefs = UserPreferences.Instance;
        userPrefs.speed = options.GetNextValue(Options.changableValues.Speed);
        userPrefs.size = options.GetNextValue(Options.changableValues.Size);
        userPrefs.prob = options.GetNextValue(Options.changableValues.Prob);
        grid.InitGrid();
        InitEventListeners();
    }

    void InitEventListeners()
    {
        onSizeChanged.AddListener(()=>UpdateParam(Options.changableValues.Size));
        onSpeedChanged.AddListener(()=>UpdateParam(Options.changableValues.Speed));
        onProbChanged.AddListener(()=>UpdateParam(Options.changableValues.Prob));
        onGameActiveToggle.AddListener(() =>
        {
            gameActive = !gameActive;
            if (gameActive)StartCoroutine(GameFunction());
        });
        onRestartClick.AddListener(()=>
        {
            generation = 1;
            grid.OnRestart();
        });
    }
    
    void UpdateParam(Options.changableValues valueToChange)
    {
        switch (valueToChange)
        {
            case Options.changableValues.Prob:
                userPrefs.prob = options.GetNextValue(Options.changableValues.Prob);
                break;
            case Options.changableValues.Size:
                int oldSize = userPrefs.size;
                userPrefs.size = options.GetNextValue(Options.changableValues.Size);
                grid.UpdateGridBySize(oldSize);
                break;
            default:
                userPrefs.speed = options.GetNextValue(Options.changableValues.Speed);
                break;
        }

        if (valueToChange != Options.changableValues.Speed)
        {
            generation = 1;
            grid.OnRestart();
        }
    }
    

    IEnumerator GameFunction()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(userPrefs.speed/1000);
            aliveCounter= grid.UpdateGridOnGenerationChange();
            generation += 1;
        }
    }

    void Update()
    {
    }
}
