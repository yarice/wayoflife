using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using WayOfLife.Model;
using WayOfLife.View;

namespace WayOfLife
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ButtonScript restartButton;
        [SerializeField] private ButtonScript startStopButton;
        [SerializeField] private ThreeOptionsToggle speedToggle;
        [SerializeField] private ThreeOptionsToggle sizeToggle;
        [SerializeField] private ThreeOptionsToggle probToggle;
        [SerializeField] private InfoText text;
        [SerializeField] private GameGrid grid;
        [SerializeField] [Range(0, 500)] private int speedlarge;
        [SerializeField] [Range(501, 1000)] private int speedmedium;
        [SerializeField] [Range(1001, 2000)] private int speedsmall;
        [SerializeField] [Range(0, 50)] private int sizesmall;
        [SerializeField] [Range(51, 100)] private int sizemedium;
        [SerializeField] [Range(101, 150)] private int sizelarge;
        [SerializeField] [Range(0, 33)] private int probsmall;
        [SerializeField] [Range(34, 66)] private int probmedium;
        [SerializeField] [Range(67, 100)] private int problarge;
        private Options options;
        private bool gameActive;
        private UnityEvent onRestartClick;
        private UnityEvent onGameActiveToggle;
        private UnityEvent onProbChanged;
        private UnityEvent onSizeChanged;
        private UnityEvent onSpeedChanged;
        private int generation = 1;
        private int aliveCounter;

        void Start()
        {
            options = ScriptableObject.CreateInstance<Options>();
            options.InitInputsQueues(speedlarge, speedmedium, speedsmall, sizelarge, sizemedium, sizesmall, problarge,
                probmedium, probsmall);
            InitEventListeners();
            restartButton.Render("Restart", onRestartClick,gameActive);
            renderToggle(ValuesEnum.Prob);
            renderToggle(ValuesEnum.Size);
            renderToggle(ValuesEnum.Speed);
            grid.InitGrid(getMaximalSize(), getProb());
        }

        void InitEventListeners()
        {
            onSizeChanged = new UnityEvent();
            onSpeedChanged = new UnityEvent();
            onProbChanged = new UnityEvent();
            onGameActiveToggle = new UnityEvent();
            onRestartClick = new UnityEvent();
            onSizeChanged.AddListener(() => UpdateParam(ValuesEnum.Size));
            onSpeedChanged.AddListener(() => UpdateParam(ValuesEnum.Speed));
            onProbChanged.AddListener(() => UpdateParam(ValuesEnum.Prob));
            onGameActiveToggle.AddListener(() =>
            {
                gameActive = !gameActive;
                if (gameActive) StartCoroutine(GameFunction());
                renderToggle(ValuesEnum.Prob);
                renderToggle(ValuesEnum.Size);
            });
            onRestartClick.AddListener(() =>
            {
                generation = 1;
                grid.OnRestart(getSize(),getProb());
            });
        }

         int getMaximalSize()
        {
            return sizelarge;
        }

        void UpdateParam(ValuesEnum valueToChange)
        {
            if (valueToChange == ValuesEnum.Size)
            {
                int oldSize = options.GetCurrentValue(ValuesEnum.Size);
                options.GetNextValue(ValuesEnum.Size);
                grid.UpdateGridBySize(getSize(),oldSize);
            }
            else
            {
                options.GetNextValue(valueToChange);
            }

            if (valueToChange != ValuesEnum.Speed)
            {
                generation = 1;
                grid.OnRestart(getSize(),getProb());
            }

            renderToggle(valueToChange);
        }


        IEnumerator GameFunction()
        {
            while (gameActive)
            {
                yield return new WaitForSeconds(getSpeed() / 1000);
                aliveCounter = grid.UpdateGridOnGenerationChange(getSize());
                generation += 1;
            }
        }

        private int getSpeed()
        {
            return options.GetCurrentValue(ValuesEnum.Speed);
        }

        private int getSize()
        {
            return options.GetCurrentValue(ValuesEnum.Size);
        }

        private int getProb()
        {
            return options.GetCurrentValue(ValuesEnum.Prob);
        }

        private void renderToggle(ValuesEnum value)
        {
            switch (value)
            {
                case ValuesEnum.Prob:
                {
                    probToggle.Render(ValuesEnum.Prob, getProb(), onProbChanged,gameActive);
                    break;
                }
                case ValuesEnum.Size:
                {
                    sizeToggle.Render(ValuesEnum.Size, getSize(), onSizeChanged,gameActive);
                    break;
                }
                default:
                {
                    speedToggle.Render(ValuesEnum.Speed, getSpeed(), onSpeedChanged,false);
                    break;
                }
            }
        }

        void Update()
        {
            text.Render(gameActive, generation, aliveCounter, getSize());
            startStopButton.Render(gameActive ? "Stop" : "Start", onGameActiveToggle,false);
        }
    }
}