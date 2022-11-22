using System;
using System.Collections;
using Unity.VisualScripting;
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
        [SerializeField] private OptionsScriptableObject options;
        private bool gameActive;
        private int generation = 1;
        private int aliveCounter;
        private int timeConversionConstant = 1_000;
        private WaitForSeconds waitForSeconds;
        private Coroutine coroutine;


        void Start()
        {
            options.InitInputsQueues();
            InitEventListeners();
            restartButton.Render("Restart", gameActive);
            renderToggle(ValuesEnum.Prob);
            renderToggle(ValuesEnum.Size);
            renderToggle(ValuesEnum.Speed);
            text.Render(gameActive, generation, aliveCounter, getSize());
            grid.InitGrid(options.getMaximalSize(), getProb());
        }

        void InitEventListeners()
        {
            sizeToggle.onClick += onSizeChange;
            speedToggle.onClick += onSpeedChange;
            probToggle.onClick += onProbChange;
            startStopButton.onClick += onGameActiveToggle;
            restartButton.onClick += onRestart;
        }

        void onSizeChange()
        {
            UpdateParam(ValuesEnum.Size);
        }

        void onSpeedChange()
        {
            UpdateParam(ValuesEnum.Speed);
        }

        void onProbChange()
        {
            UpdateParam(ValuesEnum.Prob);
        }

        void onGameActiveToggle()
        {
            gameActive = !gameActive;
            if (gameActive)
            {
                coroutine = StartCoroutine(GameFunction());
            }
            else
            {
                StopCoroutine(coroutine);
            }

            renderToggle(ValuesEnum.Prob);
            renderToggle(ValuesEnum.Size);
            text.Render(gameActive, generation, aliveCounter, getSize());
            startStopButton.Render(gameActive ? "Stop" : "Start", false);
        }

        void onRestart()
        {
            generation = 1;
            grid.OnRestart(getSize(), getProb());
        }


        void UpdateParam(ValuesEnum valueToChange)
        {
            if (valueToChange == ValuesEnum.Size)
            {
                int oldSize = options.GetCurrentValue(ValuesEnum.Size);
                options.GetNextValue(ValuesEnum.Size);
                grid.UpdateGridBySize(getSize(), oldSize);
            }
            else
            {
                options.GetNextValue(valueToChange);
            }

            if (valueToChange == ValuesEnum.Speed)
            {
                waitForSeconds = new WaitForSeconds(getSpeed() / timeConversionConstant);
            }

            else
            {
                generation = 1;
                grid.OnRestart(getSize(), getProb());
            }

            renderToggle(valueToChange);
        }


        IEnumerator GameFunction()
        {
            if (waitForSeconds == null)
            {
                waitForSeconds = new WaitForSeconds(getSpeed() / timeConversionConstant);
            }

            while (gameActive)
            {
                yield return waitForSeconds;
                aliveCounter = grid.UpdateGridOnGenerationChange(getSize());
                text.Render(gameActive, generation, aliveCounter, getSize());
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
                    probToggle.Render(ValuesEnum.Prob, getProb(), gameActive);
                    break;
                }
                case ValuesEnum.Size:
                {
                    sizeToggle.Render(ValuesEnum.Size, getSize(), gameActive);
                    break;
                }
                default:
                {
                    speedToggle.Render(ValuesEnum.Speed, getSpeed(), false);
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            sizeToggle.onClick -= onSizeChange;
            speedToggle.onClick -= onSpeedChange;
            probToggle.onClick -= onProbChange;
            startStopButton.onClick -= onGameActiveToggle;
            restartButton.onClick -= onRestart;
        }
    }
}