using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class receiveData_from_eControl : MonoBehaviour
{
    private float _samplingRate;
    public bool processIncomingData;

    public GameObject receivingButton;
    
    public GameObject hmd_remote;
    public GameObject handRight_remote;
    public GameObject handLeft_remote;

    private Transform _hmd_transform;
    private Transform _handR_transform;
    private Transform _handL_transform;
    
    // Start is called before the first frame update
    void Start()
    {
        processIncomingData = false;
        _samplingRate = 1.0f / 90;
        
        _hmd_transform = hmd_remote.transform;
        _handR_transform = handRight_remote.transform;
        _handL_transform = handLeft_remote.transform;
    }
    
     private IEnumerator processIncomingData_from_ExperimentControl()
    {
        // continuously process incoming data until stopped
        while(true)
        {
            // keep track when starting to send data
            double timeBeginnSample = GetCurrentTimestampInSeconds();
           
           

   
            // wait until restarting coroutine to match sampling rate
            double timeEndSample = GetCurrentTimestampInSeconds();
            if ((timeEndSample - timeBeginnSample) < _samplingRate) 
            {
                yield return new WaitForSeconds((float)(_samplingRate - (timeEndSample - timeBeginnSample)));
            }
        }
        
    }


    public void StartProcessingIncomingData_from_ExperimentControl()
    {
        
        StartCoroutine( processIncomingData_from_ExperimentControl());
        processIncomingData = true;
        receivingButton.SetActive(true);
    }

    public void StoppProcessingIncomingData_from_ExperimentControl()
    {
        
        StopCoroutine( processIncomingData_from_ExperimentControl() );
        processIncomingData = false;
        receivingButton.SetActive(false);
            
    }
    
        
    private double GetCurrentTimestampInSeconds()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }

}
