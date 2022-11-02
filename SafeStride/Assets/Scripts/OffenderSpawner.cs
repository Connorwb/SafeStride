using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffenderSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject offenderDot;
    
    // Start is called before the first frame update
    void Start()
    {
        DataDownloader downMan = new DataDownloader();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
