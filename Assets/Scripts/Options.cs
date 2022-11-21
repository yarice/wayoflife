using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public enum changableValues
    {
        Speed,
        Prob,
        Size
    }
    
    // Start is called before the first frame update
    [Range(1001,2000)] public int speedsmall;
    [Range(501,1000)]public int speedmedium;
    [Range(0,500)]public int speedlarge;
    [Range(0,50)]public int sizesmall;
    [Range(51,100)]public int sizemedium;
    [Range(101,150)]public int sizelarge;
    [Range(0,33)]public int probsmall;
    [Range(34,66)]public int probmedium;
    [Range(67,100)]public int problarge;
    public Queue<int> speeds;
    public Queue<int> probs;
    public Queue<int> sizes;
    public static Options Instance;
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        InitInputsQueues();
    }
    
    void InitInputsQueues()
    {
        speeds = new Queue<int>();
        sizes = new Queue<int>();
        probs = new Queue<int>();
        speeds.Enqueue(speedlarge);
        speeds.Enqueue(speedmedium);
        speeds.Enqueue(speedsmall);
        sizes.Enqueue(sizelarge);
        sizes.Enqueue(sizemedium);
        sizes.Enqueue(sizesmall);
        probs.Enqueue(problarge);
        probs.Enqueue(probmedium);
        probs.Enqueue(probsmall);
    }

    public int GetMaximalSize()
    {
        return sizelarge;
    }

    public int GetNextValue(changableValues value)
    {
        Queue<int> queue;
        switch (value)
        {
            case Options.changableValues.Prob:
                
                queue = probs;
                break;
            case Options.changableValues.Size:
                queue = sizes;
                break;
            default:
                queue = speeds;
                break;
        }

        int temp = queue.Dequeue();
        queue.Enqueue(temp);
        return temp;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
