using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class sendSaveDataExperimentControl : MonoBehaviour
{
    private float _samplingRate = 1.0f/ 90;
    public bool dataSending;

    public GameObject recordingButton;
    
    public GameObject hmd_local;
    public GameObject controllerRight_local;
    public GameObject controllerLeft_local;

    private Transform _hmd_transform;
    private Transform _controllerR_transform;
    private Transform _controllerL_transform;
    
    // Start is called before the first frame update
    void Start()
    {
        dataSending = false;
        
        _hmd_transform = hmd_local.transform;
        _controllerR_transform = controllerRight_local.transform;
        _controllerL_transform = controllerLeft_local.transform;
    }

    
    private IEnumerator SendData_ExperimentControl()
    {
        // continuously save data until stopped
        while(true)
        {
            // keep track when starting to send data
            double timeBeginnSample = GetCurrentTimestampInSeconds();
            
            double[] timeSsample = new []{timeBeginnSample};
            lslStreamsExperimentControl.Instance.eCon_timeStamp_StartSample_O.push_sample(timeSsample);

            # region VR hmd controller
            // hmd
            float[] hmdTRD = new[]
            {
                _hmd_transform.position.x,
                _hmd_transform.position.y,
                _hmd_transform.position.z,
                _hmd_transform.rotation.eulerAngles.x,
                _hmd_transform.rotation.eulerAngles.y,
                _hmd_transform.rotation.eulerAngles.z,
                _hmd_transform.forward.x,
                _hmd_transform.forward.y,
                _hmd_transform.forward.z,
            };
            
            lslStreamsExperimentControl.Instance.eCon_hmd_O.push_sample(hmdTRD);
            
            // controller right
            float[] contRTRD = new[]
            {
                _controllerR_transform.position.x,
                _controllerR_transform.position.y,
                _controllerR_transform.position.z,
                _controllerR_transform.rotation.eulerAngles.x,
                _controllerR_transform.rotation.eulerAngles.y,
                _controllerR_transform.rotation.eulerAngles.z,
                _controllerR_transform.forward.x,
                _controllerR_transform.forward.y,
                _controllerR_transform.forward.z,
            };
            
            lslStreamsExperimentControl.Instance.eCon_hand_right_O.push_sample(contRTRD);

            // controller left
            float[] contLTRD = new[]
            {
                _controllerL_transform.position.x,
                _controllerL_transform.position.y,
                _controllerL_transform.position.z,
                _controllerL_transform.rotation.eulerAngles.x,
                _controllerL_transform.rotation.eulerAngles.y,
                _controllerL_transform.rotation.eulerAngles.z,
                _controllerL_transform.forward.x,
                _controllerL_transform.forward.y,
                _controllerL_transform.forward.z,
            };
            
            lslStreamsExperimentControl.Instance.eCon_hand_left_O.push_sample(contLTRD);
            
            # endregion

   
            // wait until restarting coroutine to match sampling rate
            double timeEndSample = GetCurrentTimestampInSeconds();
            if ((timeEndSample - timeBeginnSample) < _samplingRate) 
            {
                yield return new WaitForSeconds((float)(_samplingRate - (timeEndSample - timeBeginnSample)));
            }
        }
        
        
    }
    
    public void StartDataSending_ExperimentControl()
    {
        
        StartCoroutine( SendData_ExperimentControl());
        dataSending = true;
        recordingButton.SetActive(true);
    }

    public void StoppDataSending_ExperimentControl()
    {
        
        StopCoroutine( SendData_ExperimentControl() );
        dataSending = false;
        recordingButton.SetActive(false);
            
    }
    
    private double GetCurrentTimestampInSeconds()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }

    
}
