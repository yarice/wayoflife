using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace WayOfLife.View
{
    public class GameGrid : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private GameObject cellPrefab;
        private GameObject[,] cellsMat;
        private bool[,] shouldAppearInNextGeneration;


        public void InitGrid(int maximalSize, int prob)
        {
            cellsMat = new GameObject[maximalSize, maximalSize];
            shouldAppearInNextGeneration = new bool[maximalSize, maximalSize];
            for (int i = 0; i < maximalSize; i++)
            {
                for (int j = 0; j < maximalSize; j++)
                {
                    bool shouldAppear = Random.Range(0, 101) <= prob;
                    cellsMat[i, j] = Instantiate(cellPrefab, new Vector3(i, j, 0), cellPrefab.transform.rotation);
                    if (!shouldAppear)
                    {
                        cellsMat[i, j].SetActive(false);
                    }

                    shouldAppearInNextGeneration[i, j] = shouldAppear;
                }
            }
        }

        public void OnRestart(int size, int prob)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    bool shouldAppear = Random.Range(0, 101) <= prob;
                    cellsMat[i, j].SetActive(shouldAppear);
                    shouldAppearInNextGeneration[i, j] = shouldAppear;
                }
            }
        }

        public void UpdateGridBySize(int newSize, int oldSize)
        {
            if (newSize >= oldSize)
            {
                return;
            }
            for (int i = 0; i < oldSize; i++)
            {
                for (int j = 0; j < oldSize; j++)
                {
                    if (i >= newSize || j >= newSize) //deactivate cells that are out of bounds
                    {
                        cellsMat[i, j].SetActive(false);
                        shouldAppearInNextGeneration[i, j] = false;
                    }
                }
            }
        }

        public int UpdateGridOnGenerationChange(int size)
        {
            int aliveCounter = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    aliveCounter += cellsMat[i, j].activeSelf ? 1 : 0;
                    int numberOfAliveNeighbors = countAliveNeighbors(cellsMat,i, j, size);
                    setValueForNextGen(shouldAppearInNextGeneration,numberOfAliveNeighbors, i, j);
                }
            }

            updateNextGen(cellsMat,shouldAppearInNextGeneration,size);
            return aliveCounter;
        }

        bool isNeighbor(int neighborX,int neighborY, int homeX, int homeY, int size)
        {
            return (neighborX >= 0 && neighborY >= 0 && neighborX < size && neighborY < size &&
                    !(neighborX == homeX && neighborY == homeY));
        }

        void setValueForNextGen(bool[,] shouldAppearInNextGeneration,int numberOfLiveNeighbors, int i, int j)
        {
            if ((numberOfLiveNeighbors < 2 || numberOfLiveNeighbors > 3)) //kill an alive cell
            {
                shouldAppearInNextGeneration[i, j] = false;
            }

            if (numberOfLiveNeighbors == 3) //bring a dead cell to life
            {
                shouldAppearInNextGeneration[i, j] = true;
            }
        }

        int countAliveNeighbors(GameObject[,] cellsMat, int i, int j, int size)
        {
            int numberOfLiveNeighbors = 0;
            for (int k = i - 1; k < i + 2; k++)
            {
                for (int l = j - 1; l < j + 2; l++)
                {
                           
                    if ( isNeighbor(k, l, i, j, size))
                    {
                        numberOfLiveNeighbors += cellsMat[k, l].activeSelf ? 1 : 0;
                    }
                }
            }

            return numberOfLiveNeighbors;
        }

        void updateNextGen(GameObject[,] cellsMat,bool [,] shouldAppearInNextGeneration, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (cellsMat[i, j].activeSelf &&
                        !shouldAppearInNextGeneration[i, j]) //was enabled and should be disabled
                    {
                        cellsMat[i, j].SetActive(false);
                    }

                    if (!cellsMat[i, j].activeSelf &&
                        shouldAppearInNextGeneration[i, j]) //was disabled and should be enabled
                    {
                        cellsMat[i, j].SetActive(true);
                    }
                }
            }
        }
    }
}