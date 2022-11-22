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
        [SerializeField] private  OptionsScriptableObject options;
        private bool gameActive;
        private int generation;
        private int aliveCounter;
        private WaitForSeconds waitForSeconds;
        private Coroutine coroutine;


        private void Start()
        {
            generation = 1;
            options.InitInputsQueues();
            InitEventListeners();
            restartButton.Render("Restart", gameActive);
            RenderToggle(ValuesEnum.Prob);
            RenderToggle(ValuesEnum.Size);
            RenderToggle(ValuesEnum.Speed);
            text.Render(gameActive, generation, aliveCounter, GetSize());
            grid.InitGrid(options.GetMaximalSize(), GetProb());
        }

        private void InitEventListeners()
        {
            sizeToggle.onClick += OnSizeChange;
            speedToggle.onClick += OnSpeedChange;
            probToggle.onClick += OnProbChange;
            startStopButton.onClick += OnGameActiveToggle;
            restartButton.onClick += OnRestart;
        }

        private void OnSizeChange()
        {
            UpdateParam(ValuesEnum.Size);
        }

        private void OnSpeedChange()
        {
            UpdateParam(ValuesEnum.Speed);
        }

        private void OnProbChange()
        {
            UpdateParam(ValuesEnum.Prob);
        }

        private void OnGameActiveToggle()
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

            RenderToggle(ValuesEnum.Prob);
            RenderToggle(ValuesEnum.Size);
            text.Render(gameActive, generation, aliveCounter, GetSize());
            startStopButton.Render(gameActive ? "Stop" : "Start", false);
        }

        private void OnRestart()
        {
            generation = 1;
            grid.OnRestart(GetSize(), GetProb());
        }


        private void UpdateParam(ValuesEnum valueToChange)
        {
            if (valueToChange == ValuesEnum.Size)
            {
                int oldSize = options.GetCurrentValue(ValuesEnum.Size);
                options.GetNextValue(ValuesEnum.Size);
                grid.UpdateGridBySize(GetSize(), oldSize);
            }
            else
            {
                options.GetNextValue(valueToChange);
            }

            if (valueToChange == ValuesEnum.Speed)
            {
                waitForSeconds = new WaitForSeconds(GetSpeed() /  1_000);
            }

            else
            {
                generation = 1;
                grid.OnRestart(GetSize(), GetProb());
            }

            RenderToggle(valueToChange);
        }


        private IEnumerator GameFunction()
        {
            if (waitForSeconds == null)
            {
                waitForSeconds = new WaitForSeconds(GetSpeed() /  1_000);
            }

            while (gameActive)
            {
                yield return waitForSeconds;
                aliveCounter = grid.UpdateGridOnGenerationChange(GetSize());
                text.Render(gameActive, generation, aliveCounter, GetSize());
                generation += 1;
            }
        }

        private int GetSpeed()
        {
            return options.GetCurrentValue(ValuesEnum.Speed);
        }

        private int GetSize()
        {
            return options.GetCurrentValue(ValuesEnum.Size);
        }

        private int GetProb()
        {
            return options.GetCurrentValue(ValuesEnum.Prob);
        }

        private void RenderToggle(ValuesEnum value)
        {
            switch (value)
            {
                case ValuesEnum.Prob:
                {
                    probToggle.Render(ValuesEnum.Prob, GetProb(), gameActive);
                    break;
                }
                case ValuesEnum.Size:
                {
                    sizeToggle.Render(ValuesEnum.Size, GetSize(), gameActive);
                    break;
                }
                default:
                {
                    speedToggle.Render(ValuesEnum.Speed, GetSpeed(), false);
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            sizeToggle.onClick -= OnSizeChange;
            speedToggle.onClick -= OnSpeedChange;
            probToggle.onClick -= OnProbChange;
            startStopButton.onClick -= OnGameActiveToggle;
            restartButton.onClick -= OnRestart;
        }
    }
}