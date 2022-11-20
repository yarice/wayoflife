using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // Start is called before the first frame update
    private UserPreferences userPrefs;
    public GameObject cellPrefab;
    private GameObject[,]cellsMat;
    private bool[,]shouldAppearInNextGeneration;
    private int maximalSize;
    
    void Start()
    {
        userPrefs=UserPreferences.Instance;
        Options options = Options.Instance;
        maximalSize = options.GetMaximalSize();
        InitGrid();
    }
    

    public void InitGrid()
    {
        cellsMat= new GameObject[maximalSize,maximalSize];
        shouldAppearInNextGeneration= new bool[maximalSize,maximalSize];
        for (int i = 0; i < maximalSize; i++)
        {
            for (int j = 0; j < maximalSize; j++)
            {
                bool shouldAppear=Random.Range(0,101)<=userPrefs.prob;
                cellsMat[i,j]= Instantiate(cellPrefab, new Vector3(i, j, 0), cellPrefab.transform.rotation);
                if (!shouldAppear)
                {
                    cellsMat[i,j].SetActive(false);
                }
                shouldAppearInNextGeneration[i, j] = shouldAppear;
            }
        }
    }

    public void OnRestart()
    {
        for (int i = 0; i < userPrefs.size; i++)
        {
            for (int j = 0; j < userPrefs.size; j++)
            {
                bool shouldAppear=Random.Range(0,101)<=userPrefs.prob;
                cellsMat[i,j].SetActive(shouldAppear);
                shouldAppearInNextGeneration[i, j] = shouldAppear;
            }
        }
    }

    public void UpdateGridBySize(int oldSize)
    {
        int newSize = userPrefs.size;
        if (newSize < oldSize)
        {

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
    }

    public int UpdateGridOnGenerationChange()
    {
        int aliveCounter = 0;
        for (int i = 0; i < userPrefs.size; i++)
        {
            for (int j = 0; j < userPrefs.size; j++)
            {
                aliveCounter += cellsMat[i,j].activeSelf ? 1 : 0;
                int numberOfLiveNeighbors = 0;
                for (int k = i - 1; k < i + 2; k++)
                {
                    for (int l = j - 1; l < j + 2; l++)
                    {
                        if (k >= 0 && l >= 0 && k < userPrefs.size && l < userPrefs.size && !(k == i && l == j))
                        {
                            numberOfLiveNeighbors += cellsMat[k, l].activeSelf ? 1 : 0;
                        }
                    }
                }
                if ((numberOfLiveNeighbors < 2 || numberOfLiveNeighbors>3)) //kill an alive cell
                {
                    shouldAppearInNextGeneration[i,j]=false;
                }
                if (numberOfLiveNeighbors == 3) //bring a dead cell to life
                {
                    shouldAppearInNextGeneration[i,j]=true;
                }
            }
        }
        //update the alive and dead cells for next generation
        for (int i = 0; i < userPrefs.size; i++)
        {
            for (int j = 0; j < userPrefs.size; j++)
            {
                if (cellsMat[i, j].activeSelf && !shouldAppearInNextGeneration[i, j])
                {
                    cellsMat[i,j].SetActive(false);
                }
                if (!cellsMat[i, j].activeSelf && shouldAppearInNextGeneration[i, j])
                {
                    cellsMat[i,j].SetActive(true);
                }
            }
        }
        return aliveCounter;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
