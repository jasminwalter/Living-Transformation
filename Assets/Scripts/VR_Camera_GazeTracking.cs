using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;

public class VR_Camera_GazeTracking : MonoBehaviour
{

    
    public GameObject localgazeSphere;

    public bool withEyeTracking = false;
    
    private TobiiXR_EyeTrackingData _eyeTrackingWorld;

    
    // Start is called before the first frame update
    private void OnEnable()
    {
        localgazeSphere = GameObject.Find("local_gazeSphere");
        withEyeTracking = false;
    }
    
    // Update is called once per frame
    void Update()
    {

        // if EyeTracking is not activated, use the nose vector (or camera position)
        // for the raycast to estimate and network the gaze sphere
        // if (!withEyeTracking)
        // {
        //     _eyeTrackingWorld = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
        //     Vector3 gazeRayOrigin = _eyeTrackingWorld.GazeRay.Origin;
        //     Vector3 gazeRayDirection = _eyeTrackingWorld.GazeRay.Direction;
        //     RaycastHit hit;
        //     if (Physics.Raycast(gazeRayOrigin, gazeRayDirection, out hit, 100f))
        //     {
        //         localgazeSphere.transform.position = hit.point;
        //     }
        // }
        // else
        // {
        //     Vector3 direction = transform.TransformDirection(Vector3.forward);
        //
        //     RaycastHit hit;
        //     if (Physics.Raycast(transform.position, direction, out hit, 100f))
        //     {
        //         localgazeSphere.transform.position = hit.point;
        //     }
        // }

    }

}
