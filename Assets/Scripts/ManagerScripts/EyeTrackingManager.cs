using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrackingManager : MonoBehaviour
{
    
    // public variables assigned in the inspector

    public bool startCalibration;
    public bool startValidation;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startCalibration)
        {
            
        }
        
    }

    public void StartCalButton()
    {
        startCalibration = true;
    }
}
