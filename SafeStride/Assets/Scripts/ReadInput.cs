using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
    private double input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ReadInputs(double sCoord) //, double eCoord, int startID, int endID
    {
        input = sCoord;
        Debug.Log(input);
    }
}
