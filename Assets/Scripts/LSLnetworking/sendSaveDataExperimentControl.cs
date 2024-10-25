using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;
using LSL;
using System.Linq;
using System;
using Unity.Mathematics;

public class sendSaveDataExperimentControl : MonoBehaviour
{
    public int eCon_partID;
    public int eUser_partID;
    
    private float[] gazeM;
    private float _samplingRate = 1.0f/ 90;
    public bool dataSending;

    public GameObject eControl_UI;
    public GameObject recordingButton;
    
    // vr variables
    public GameObject hmd_local;
    public GameObject handRight_local;
    public GameObject handLeft_local;

    private Transform _hmd_transform;
    private Transform _handR_transform;
    private Transform _handL_transform;
    
    // gaze and eye tracking
    private Dictionary<EyeShape_v2, float> eyeWeightings = new Dictionary<EyeShape_v2, float>();
    private EyeData_v2 eyeData = new EyeData_v2();
    
    private float leftBlink;
    private float rightBlink;
    private float leftWide;
    private float rightWide;
    private float leftSqueeze;
    private float rightSqueeze;
    private float eye_Left_Up;
    private float eye_Left_Down;
    private float eye_Left_Left;
    private float eye_Left_Right;
    private float eye_Right_Up;
    private float eye_Right_Down;
    private float eye_Right_Left;
    private float eye_Right_Right;
    
    // interaction sphere variables
    public GameObject gazeSphere_local;
    public GameObject pointSphere_local;
    public GameObject touchSphere_local;
    
    private Transform _gazeSphere_transform;
    private Transform _pointSphere_transform;
    private Transform _touchSphere_transform;
    
    // Start is called before the first frame update
    void Start()
    {
        eControl_UI.SetActive(true);
        dataSending = false;
        
        // VR vars
        _hmd_transform = hmd_local.transform;
        _handR_transform = handRight_local.transform;
        _handL_transform = handLeft_local.transform;
        
        // interaction spheres
        _gazeSphere_transform = gazeSphere_local.transform;
        _pointSphere_transform = pointSphere_local.transform;
        _touchSphere_transform = touchSphere_local.transform;
        
    }

    
    private IEnumerator SendData_ExperimentControl()
    {
        // continuously save data until stopped
        while(true)
        {
            //Debug.Log("Sending data to experiment");
            // keep track when starting to send data
            double timeBeginnSample = GetCurrentTimestampInSeconds();
            
            double[] timeSsample = new []{timeBeginnSample};
            lslStreamsExperimentControl.Instance.eCon_timeStamp_StartSample_O.push_sample(timeSsample);

            int[] partIDs =
            {
                eCon_partID,
                eUser_partID
            };
            lslStreamsExperimentControl.Instance.participantIDs_O.push_sample(partIDs);

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
                _handR_transform.position.x,
                _handR_transform.position.y,
                _handR_transform.position.z,
                _handR_transform.rotation.eulerAngles.x,
                _handR_transform.rotation.eulerAngles.y,
                _handR_transform.rotation.eulerAngles.z,
                _handR_transform.forward.x,
                _handR_transform.forward.y,
                _handR_transform.forward.z,
            };
            
            lslStreamsExperimentControl.Instance.eCon_hand_right_O.push_sample(contRTRD);

            // controller left
            float[] contLTRD = new[]
            {
                _handL_transform.position.x,
                _handL_transform.position.y,
                _handL_transform.position.z,
                _handL_transform.rotation.eulerAngles.x,
                _handL_transform.rotation.eulerAngles.y,
                _handL_transform.rotation.eulerAngles.z,
                _handL_transform.forward.x,
                _handL_transform.forward.y,
                _handL_transform.forward.z,
            };
            
            lslStreamsExperimentControl.Instance.eCon_hand_left_O.push_sample(contLTRD);
            
            # endregion
            
            // gaze data
            # region gaze and eye tracking
            
            // Get blink weightings
            if (SRanipal_Eye_v2.GetEyeWeightings(out eyeWeightings))
            {
                leftSqueeze = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Left_Squeeze)
                    ? eyeWeightings[EyeShape_v2.Eye_Left_Squeeze]
                    : 0.0f;
                rightSqueeze = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Right_Squeeze)
                    ? eyeWeightings[EyeShape_v2.Eye_Right_Squeeze]
                    : 0.0f;
                leftWide = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Left_Wide)
                    ? eyeWeightings[EyeShape_v2.Eye_Left_Wide]
                    : 0.0f;
                rightWide = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Right_Wide)
                    ? eyeWeightings[EyeShape_v2.Eye_Right_Wide]
                    : 0.0f;
                eye_Left_Down = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Left_Down)
                    ? eyeWeightings[EyeShape_v2.Eye_Left_Down]
                    : 0.0f;
                eye_Right_Down = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Right_Down)
                    ? eyeWeightings[EyeShape_v2.Eye_Right_Down]
                    : 0.0f;
                eye_Left_Up = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Left_Up)
                    ? eyeWeightings[EyeShape_v2.Eye_Left_Up]
                    : 0.0f;
                eye_Right_Up = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Right_Up)
                    ? eyeWeightings[EyeShape_v2.Eye_Right_Up]
                    : 0.0f;
                eye_Left_Right = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Left_Right)
                    ? eyeWeightings[EyeShape_v2.Eye_Left_Right]
                    : 0.0f;
                eye_Right_Right = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Right_Right)
                    ? eyeWeightings[EyeShape_v2.Eye_Right_Right]
                    : 0.0f;
                eye_Left_Left = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Left_Left)
                    ? eyeWeightings[EyeShape_v2.Eye_Left_Left]
                    : 0.0f;
                eye_Right_Left = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Right_Left)
                    ? eyeWeightings[EyeShape_v2.Eye_Right_Left]
                    : 0.0f;
                leftBlink = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Left_Blink)
                    ? eyeWeightings[EyeShape_v2.Eye_Left_Blink]
                    : 0.0f;
                rightBlink = eyeWeightings.ContainsKey(EyeShape_v2.Eye_Right_Blink)
                    ? eyeWeightings[EyeShape_v2.Eye_Right_Blink]
                    : 0.0f;
            }
            gazeM = new float[14];
            gazeM[0] = leftSqueeze;
            gazeM[1] = rightSqueeze;
            gazeM[2] = leftWide;
            gazeM[3] = rightWide;
            gazeM[4] = eye_Left_Down;
            gazeM[5] = eye_Right_Down;
            gazeM[6] = eye_Left_Up;
            gazeM[7] = eye_Right_Up;
            gazeM[8] = eye_Left_Right; 
            gazeM[9] = eye_Right_Right;
            gazeM[10] = eye_Left_Left;
            gazeM[11] = eye_Right_Left;
            gazeM[12] = leftBlink;
            gazeM[13] = rightBlink;
            
            // Send sample via LSL
            lslStreamsExperimentControl.Instance.eCon_eyeMovement_O.push_sample(gazeM);
            
            // gaze data and raycast
            SRanipal_Eye_v2.GetVerboseData(out VerboseData verboseData); 
            
            Vector3 coordinateAdaptedGazeDirectionCombined = new Vector3(verboseData.combined.eye_data.gaze_direction_normalized.x * -1,  verboseData.combined.eye_data.gaze_direction_normalized.y, verboseData.combined.eye_data.gaze_direction_normalized.z);
            var eyePositionCombinedWorld = verboseData.combined.eye_data.gaze_origin_mm / 1000 + _hmd_transform.position;
            var eyeDirectionCombinedWorld = _hmd_transform.rotation * coordinateAdaptedGazeDirectionCombined;
            var eyeDirectionCombinedLocal = coordinateAdaptedGazeDirectionCombined;
           
            float[] eyeTrackingPosDir =
            {
                eyePositionCombinedWorld.x,
                eyePositionCombinedWorld.y,
                eyePositionCombinedWorld.z,
                verboseData.combined.eye_data.gaze_origin_mm.x,
                verboseData.combined.eye_data.gaze_origin_mm.y,
                verboseData.combined.eye_data.gaze_origin_mm.z,
                eyeDirectionCombinedWorld.x,
                eyeDirectionCombinedWorld.y,
                eyeDirectionCombinedWorld.z,
                eyeDirectionCombinedLocal.x,
                eyeDirectionCombinedLocal.y,
                eyeDirectionCombinedLocal.z,

            };
            lslStreamsExperimentControl.Instance.eCon_gazePosDir_O.push_sample(eyeTrackingPosDir);
            
            
            // other gaze data

            float[] otherEyeD =
            {
                verboseData.left.eye_openness,
                verboseData.right.eye_openness,
                verboseData.left.pupil_diameter_mm,
                verboseData.right.pupil_diameter_mm,
                verboseData.left.eye_data_validata_bit_mask,
                verboseData.right.eye_data_validata_bit_mask,
                verboseData.combined.eye_data.eye_data_validata_bit_mask
            };
            
            lslStreamsExperimentControl.Instance.eCon_otherGazeData_O.push_sample(otherEyeD);
            
            // raycasting
            
            // Raycast combined eyes 
            RaycastHit[] raycastHitsCombined;
            raycastHitsCombined = Physics.RaycastAll(eyePositionCombinedWorld, eyeDirectionCombinedWorld,Mathf.Infinity);

            // Make sure something was hit 
            if (raycastHitsCombined.Length > 0)
            {
                // Sort by distance
                raycastHitsCombined = raycastHitsCombined.OrderBy(x=>x.distance).ToArray();
                
                // Use only the specified number of hits 
                raycastHitsCombined = raycastHitsCombined.Take(Math.Min(2,raycastHitsCombined.Length)).ToArray();

                if (raycastHitsCombined.Length == 1)
                {
                    string[] names = new string[]
                    {
                        raycastHitsCombined[0].collider.name,
                        "noHit"
                    };
                    lslStreamsExperimentControl.Instance.eCon_hitColliderName_O.push_sample(names);
                    
                    float[] hitColInfo = new float[]
                    {
                        raycastHitsCombined[0].point.x,
                        raycastHitsCombined[0].point.y,
                        raycastHitsCombined[0].point.z,
                        10000.0f,
                        10000.0f,
                        10000.0f,
                        raycastHitsCombined[0].collider.bounds.center.x,
                        raycastHitsCombined[0].collider.bounds.center.y,
                        raycastHitsCombined[0].collider.bounds.center.z,
                        10000.0f,
                        10000.0f,
                        10000.0f
                    };
                    lslStreamsExperimentControl.Instance.eCon_rayCastData_O.push_sample(hitColInfo);
                    
                }
                else
                {
                    string[] names = new string[]
                    {
                        raycastHitsCombined[0].collider.name,
                        raycastHitsCombined[1].collider.name
                    };
                    lslStreamsExperimentControl.Instance.eCon_hitColliderName_O.push_sample(names);
                    
                    float[] hitColInfo = new float[]
                    {
                        raycastHitsCombined[0].point.x,
                        raycastHitsCombined[0].point.y,
                        raycastHitsCombined[0].point.z,
                        raycastHitsCombined[1].point.x,
                        raycastHitsCombined[1].point.y,
                        raycastHitsCombined[1].point.z,
                        raycastHitsCombined[0].collider.bounds.center.x,
                        raycastHitsCombined[0].collider.bounds.center.y,
                        raycastHitsCombined[0].collider.bounds.center.z,
                        raycastHitsCombined[1].collider.bounds.center.x,
                        raycastHitsCombined[1].collider.bounds.center.y,
                        raycastHitsCombined[1].collider.bounds.center.z,
                    };
                    lslStreamsExperimentControl.Instance.eCon_rayCastData_O.push_sample(hitColInfo);
                }

                
            }
            else
            {
                string[] names = new string[] {"noHit","noHit"};
                lslStreamsExperimentControl.Instance.eCon_hitColliderName_O.push_sample(names);

                float[] hitColInfo = new float[]
                {
                    10000.0f,
                    10000.0f,
                    10000.0f,
                    10000.0f,
                    10000.0f,
                    10000.0f,
                    10000.0f,
                    10000.0f,
                    10000.0f,
                    10000.0f,
                    10000.0f,
                    10000.0f
                };
                lslStreamsExperimentControl.Instance.eCon_rayCastData_O.push_sample(hitColInfo);

            }

      

            # endregion
            
            #region interaction spheres
            // gaze
            float[] gazeSpherePos = new[]
            {
                _gazeSphere_transform.position.x,
                _gazeSphere_transform.position.y,
                _gazeSphere_transform.position.z,
            };
            
            lslStreamsExperimentControl.Instance.eCon_gazeSpherePos_O.push_sample(gazeSpherePos);
            
            // point
            float[] pointSpherePos = new[]
            {
                _pointSphere_transform.position.x,
                _pointSphere_transform.position.y,
                _pointSphere_transform.position.z,
            };
            
            lslStreamsExperimentControl.Instance.eCon_pointSpherePos_O.push_sample(pointSpherePos);
            
            
            // touch
            float[] touchSpherePos = new[]
            {
                _touchSphere_transform.position.x,
                _touchSphere_transform.position.y,
                _touchSphere_transform.position.z,
            };
            
            lslStreamsExperimentControl.Instance.eCon_touchSpherePos_O.push_sample(touchSpherePos);
            #endregion

   
            // wait until restarting coroutine to match sampling rate
            double timeEndSample = GetCurrentTimestampInSeconds();
            if ((timeEndSample - timeBeginnSample) < _samplingRate) 
            {
                yield return new WaitForSeconds((float)(_samplingRate - (timeEndSample - timeBeginnSample)));
            }
        }

        yield return null;

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
