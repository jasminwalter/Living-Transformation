using System;
using Tobii.XR;
using UnityEngine;
using ViveSR.anipal.Eye;
using System.Collections.Generic;
using System.Linq;


public class EyeTrackingManager : MonoBehaviour
{
// https://vr.tobii.com/sdk/develop/unity/documentation/api-reference/core/
    // http://developer.tobiipro.com/unity/unity-getting-started.html

    // public variables assigned in the inspector
    public Validation validation; // call to validation script
    //public ETRecorder etRecorder; // call to ETRecorder script
    //public NavMeshMovement playerScript; // to access the player
    public GameObject body; // access to change position
    public bool valOngoing;

    public Vector3 gazeRayOrigin;
    public Vector3 gazeRayDirection;
    
    // private variables 
    private TobiiXR_Settings settings; 
    private bool recordingEyeTracker = false; // set to false during validation, set to true during exploration phase
    private bool valCurrently = false; // so we only start one Coroutine 

    
    
    
    public GameObject localgazeSphere;

    
    private TobiiXR_EyeTrackingData _eyeTrackingWorld;
    
    void Start()
    {
        // if you record eye tracking, start this part (otherwise this script will not be called)
        // if (ExpManager.Instance.etRecord)
        // {
        //     // get eye tracker started
        //     settings = new TobiiXR_Settings();
        //     // settings.FieldOfUse = FieldOfUse.Interactive; // new API, not working with ours
        //     TobiiXR.Start(settings);
        //
        //     // check the booleans to make sure you start with the validation/calibration part
        //     if (ExpManager.Instance.validating && !ExpManager.Instance.recording)
        //     {
        //         // Start the experiment with calibration and then validation 
        //         // SRanipal_Eye_v2.LaunchEyeCalibration();
        //         // validation.ValidationRoutine();
        //         // validation.StartValidation();
        //     }
        //
        // }
    }

    private void OnEnable()
    {
        TobiiXR.Start();
    }

    void FixedUpdate()
    {
        
        
        // Get eye tracking data in world space
        var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
        
        Debug.Log("Gaze validity " +eyeTrackingData.GazeRay.IsValid);
        // Check if gaze ray is valid
        if(eyeTrackingData.GazeRay.IsValid)
        {
            Debug.Log("yessssssssssssssssssssssssss");
            // The origin of the gaze ray is a 3D point
            gazeRayOrigin = eyeTrackingData.GazeRay.Origin;

            // The direction of the gaze ray is a normalized direction vector
            gazeRayDirection = eyeTrackingData.GazeRay.Direction;
            RaycastHit hit;
            // if (Physics.Raycast(gazeRayOrigin, gazeRayDirection, out hit))
            if (Physics.Raycast(gazeRayOrigin, gazeRayDirection, out hit))
            {
                localgazeSphere.transform.position = hit.point;
                Debug.Log("Worked! hit is: " + hit.collider.gameObject.name);
            }
        }
        // gazeRayOrigin = _eyeTrackingWorld.GazeRay.Origin;
        // gazeRayDirection = _eyeTrackingWorld.GazeRay.Direction;
        // Debug.Log("EyeTrackingWolrd " + _eyeTrackingWorld.IsLeftEyeBlinking);

        
        // if you record eye tracking, start this part (otherwise this script will not be called)
        if (ExperimentManager._Instance.etRecord)
        {
            // manually start Calibration when pressing C
            if (Input.GetKeyDown(KeyCode.C))
            {
                // change the variables to ensure that the timer is paused
                //ExperimentManager._Instance.recording = false;
                ExperimentManager._Instance.validating = true;
                // start calibration
                SRanipal_Eye_v2.LaunchEyeCalibration();
            } 
            // manually start validation when pressing V
            else if (Input.GetKeyDown(KeyCode.V)) // TODO: add something to check you are not doing validation
            {
                // change the variables to ensure that the timer is paused
                //ExperimentManager._Instance.recording = false;
                ExperimentManager._Instance.validating = true;
                validation.StartValidation();
            }
            // manually transport the player to the scene from the validation plane when pressing B (probably not necessary)
            else if (Input.GetKeyDown(KeyCode.B)) // TODO: add something to check you are doing validation
            {
                // change the variables to ensure that the timer is counted down
                //ExperimentManager._Instance.recording = true;
                ExperimentManager._Instance.validating = false;
            }

            // if the validation was successful and there is no eye-tracking recording, start recording
            // if (!ExperimentManager._Instance.validating && !ExperimentManager._Instance.valOngoing && !ExpManager.Instance.recording)
            // if (!ExperimentManager._Instance.validating && !ExperimentManager._Instance.valOngoing)
            //
            // {
            //     // set recording to true --> checked in ETRecorder
            //     ExperimentManager._Instance.recording = true;
            //
            // }
            // else if (ExperimentManager._Instance.validating && !ExperimentManager._Instance.valOngoing && !ExpManager.Instance.recording)
            else if (ExperimentManager._Instance.validating && !ExperimentManager._Instance.valOngoing)
            {
                // stop recording during validation
                // etRecorder.StopRecording();
                validation.StartValidation();
            }

        }
    }
}
