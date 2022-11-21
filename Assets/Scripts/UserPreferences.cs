using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UserPreferences: MonoBehaviour
{

    // Start is called before the first frame update
    public static UserPreferences Instance;
    public int speed;
    public int size;
    public int prob;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
