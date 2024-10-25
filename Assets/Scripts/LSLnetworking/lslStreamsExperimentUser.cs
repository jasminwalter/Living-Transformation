using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class lslStreamsExperimentUser : MonoBehaviour
{
    public static lslStreamsExperimentUser Instance { get; private set; } // used to allow easy access of this script in other scripts

    public string participantUID; 
    private const double NominalRate = LSL.LSL.IRREGULAR_RATE; // irregular sampling rate
    
    // general variables

    public StreamInfo eUser_timeStamp_StartSample_I;
    public StreamOutlet eUser_timeStamp_StartSample_O;
    
    // VR headset and hands
    public StreamInfo eUser_hmd_I;
    public StreamOutlet eUser_hmd_O;
    
    public StreamInfo eUser_hand_right_I;
    public StreamOutlet eUser_hand_right_O;
    
    public StreamInfo eUser_hand_left_I;
    public StreamOutlet eUser_hand_left_O;
    
    // gaze data
    public StreamInfo eUser_eyeMovement_I;
    public StreamOutlet eUser_eyeMovement_O;

    public StreamInfo eUser_gazePosDir_I;
    public StreamOutlet eUser_gazePosDir_O;
    
    public StreamInfo eUser_otherGazeData_I;
    public StreamOutlet eUser_otherGazeData_O;

    public StreamInfo eUser_hitColliderName_I;
    public StreamOutlet eUser_hitColliderName_O;
    
    public StreamInfo eUser_rayCastData_I;
    public StreamOutlet eUser_rayCastData_O;
    
    // interaction spheres
    public StreamInfo eUser_gazeSpherePos_I;
    public StreamOutlet eUser_gazeSpherePos_O;

    public StreamInfo eUser_pointSpherePos_I;
    public StreamOutlet eUser_pointSpherePos_O;
    
    public StreamInfo eUser_touchSpherePos_I;
    public StreamOutlet eUser_touchSpherePos_O;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Ensure that this instance is the only one and is accessible globally
        if (Instance == null)
        {
            Instance = this;
        }
        
        // general variables
        eUser_timeStamp_StartSample_I = new StreamInfo(
            "eUser_timeStamp_StartSample", 
            "Markers", 
            1, 
            NominalRate,
            LSL.channel_format_t.cf_double64);
        eUser_timeStamp_StartSample_O = new StreamOutlet(eUser_timeStamp_StartSample_I);
        
        // VR hmd and hands
        # region VR hmd and hands
        eUser_hmd_I = new StreamInfo(
            "eUser_hmd",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_hmd_I.desc().append_child("eUser_hmd_pos_x");
        eUser_hmd_I.desc().append_child("eUser_hmd_pos_y");
        eUser_hmd_I.desc().append_child("eUser_hmd_pos_z");
        eUser_hmd_I.desc().append_child("eUser_hmd_rot_x");
        eUser_hmd_I.desc().append_child("eUser_hmd_rot_y");
        eUser_hmd_I.desc().append_child("eUser_hmd_rot_z");
        eUser_hmd_I.desc().append_child("eUser_hmd_dirFor_x");
        eUser_hmd_I.desc().append_child("eUser_hmd_dirFor_y");
        eUser_hmd_I.desc().append_child("eUser_hmd_dirFor_z");
        eUser_hmd_O = new StreamOutlet(eUser_hmd_I);
        
        
        eUser_hand_left_I = new StreamInfo(
            "eUser_handLeft",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_hand_left_I.desc().append_child("eUser_handLeft_pos_x");
        eUser_hand_left_I.desc().append_child("eUser_handLeft_pos_y");
        eUser_hand_left_I.desc().append_child("eUser_handLeft_pos_z");
        eUser_hand_left_I.desc().append_child("eUser_handLeft_rot_x");
        eUser_hand_left_I.desc().append_child("eUser_handLeft_rot_y");
        eUser_hand_left_I.desc().append_child("eUser_handLeft_rot_z");
        eUser_hand_left_I.desc().append_child("eUser_handLeft_dirFor_x");
        eUser_hand_left_I.desc().append_child("eUser_handLeft_dirFor_y");
        eUser_hand_left_I.desc().append_child("eUser_handLeft_dirFor_z");
        eUser_hand_left_O = new StreamOutlet(eUser_hand_left_I);
        
        eUser_hand_right_I = new StreamInfo(
            "eUser_handRight",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_hand_right_I.desc().append_child("eUser_handRight_pos_x");
        eUser_hand_right_I.desc().append_child("eUser_handRight_pos_y");
        eUser_hand_right_I.desc().append_child("eUser_handRight_pos_z");
        eUser_hand_right_I.desc().append_child("eUser_handRight_rot_x");
        eUser_hand_right_I.desc().append_child("eUser_handRight_rot_y");
        eUser_hand_right_I.desc().append_child("eUser_handRight_rot_z");
        eUser_hand_right_I.desc().append_child("eUser_handRight_dirFor_x");
        eUser_hand_right_I.desc().append_child("eUser_handRight_dirFor_y");
        eUser_hand_right_I.desc().append_child("eUser_handrRight_dirFor_z");
        eUser_hand_right_O = new StreamOutlet(eUser_hand_right_I);
        # endregion
        
        // gaze data
        eUser_eyeMovement_I = new StreamInfo(
            "eUser_eyeMovement", 
            "Markers", 
            14, 
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_eyeMovement_I .desc().append_child("eye_left_squeeze");
        eUser_eyeMovement_I .desc().append_child("eye_right_squeeze");
        eUser_eyeMovement_I .desc().append_child("eye_left_wide");
        eUser_eyeMovement_I .desc().append_child("eye_right_wide");
        eUser_eyeMovement_I .desc().append_child("eye_left_down");
        eUser_eyeMovement_I .desc().append_child("eye_right_down");
        eUser_eyeMovement_I .desc().append_child("eye_left_up");
        eUser_eyeMovement_I .desc().append_child("eye_right_up");
        eUser_eyeMovement_I .desc().append_child("eye_left_right");
        eUser_eyeMovement_I .desc().append_child("eye_right_right");
        eUser_eyeMovement_I .desc().append_child("eye_left_left");
        eUser_eyeMovement_I .desc().append_child("eye_right_left");
        eUser_eyeMovement_I .desc().append_child("eye_left_blink");
        eUser_eyeMovement_I .desc().append_child("eye_right_blink");
        eUser_eyeMovement_O = new StreamOutlet(eUser_eyeMovement_I);
        
        eUser_gazePosDir_I =  new StreamInfo(
            "eUser_gazePosDir", 
            "Markers", 
            12, 
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_gazePosDir_I.desc().append_child("eUser_eyePositionCombinedWorld_x");
        eUser_gazePosDir_I.desc().append_child("eUser_eyePositionCombinedWorld_y");
        eUser_gazePosDir_I.desc().append_child("eUser_eyePositionCombinedWorld_z");
        eUser_gazePosDir_I.desc().append_child("eUser_eyePosition_GazeOrigin_x");
        eUser_gazePosDir_I.desc().append_child("eUser_eyePosition_GazeOrigin_y");
        eUser_gazePosDir_I.desc().append_child("eUser_eyePosition_GazeOrigin_z");
        eUser_gazePosDir_I.desc().append_child("eUser_eyeDirectionCombinedWorld_x");
        eUser_gazePosDir_I.desc().append_child("eUser_eyeDirectionCombinedWorld_y");
        eUser_gazePosDir_I.desc().append_child("eUser_eyeDirectionCombinedWorld_z");
        eUser_gazePosDir_I.desc().append_child("eUser_eyeDirectionCombinedLocal_x");
        eUser_gazePosDir_I.desc().append_child("eUser_eyeDirectionCombinedLocal_y");
        eUser_gazePosDir_I.desc().append_child("eUser_eyeDirectionCombinedLocal_z");
        eUser_gazePosDir_O = new StreamOutlet(eUser_gazePosDir_I);
        
        
        eUser_otherGazeData_I = new StreamInfo(
            "eUser_otherGazeData", 
            "Markers", 
            7, 
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_otherGazeData_I.desc().append_child("eUser_eyeOpennes_left");
        eUser_otherGazeData_I.desc().append_child("eUser_eyeOpennes_right");
        eUser_otherGazeData_I.desc().append_child("eUser_pupilDiameter_left");
        eUser_otherGazeData_I.desc().append_child("eUser_pupilDiameter_right");
        eUser_otherGazeData_I.desc().append_child("eUser_validityBitmask_left");
        eUser_otherGazeData_I.desc().append_child("eUser_validityBitmask_right");
        eUser_otherGazeData_I.desc().append_child("eUser_validityBitmask_combined");
        eUser_otherGazeData_O = new StreamOutlet(eUser_otherGazeData_I);
        
        eUser_hitColliderName_I =  new StreamInfo(
            "eUser_hitColliderName", 
            "Markers", 
            2, 
            NominalRate,
            LSL.channel_format_t.cf_string);
        eUser_hitColliderName_I.desc().append_child("hitColliderName_1");
        eUser_hitColliderName_I.desc().append_child("hitColliderName_2");
        eUser_hitColliderName_O = new StreamOutlet(eUser_hitColliderName_I);
        
        
        eUser_rayCastData_I  = new StreamInfo(
            "eUser_rayCastData", 
            "Markers", 
            12, 
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_rayCastData_I.desc().append_child("hitPointOnObject_x_1");
        eUser_rayCastData_I.desc().append_child("hitPointOnObject_y_1");
        eUser_rayCastData_I.desc().append_child("hitPointOnObject_z_1");
        eUser_rayCastData_I.desc().append_child("hitPointOnObject_x_2");
        eUser_rayCastData_I.desc().append_child("hitPointOnObject_y_2");
        eUser_rayCastData_I.desc().append_child("hitPointOnObject_z_2");
        eUser_rayCastData_I.desc().append_child("hitObjectColliderBoundsCenter_x_1");
        eUser_rayCastData_I.desc().append_child("hitObjectColliderBoundsCenter_y_1");
        eUser_rayCastData_I.desc().append_child("hitObjectColliderBoundsCenter_z_1");
        eUser_rayCastData_I.desc().append_child("hitObjectColliderBoundsCenter_x_2");
        eUser_rayCastData_I.desc().append_child("hitObjectColliderBoundsCenter_y_2");
        eUser_rayCastData_I.desc().append_child("hitObjectColliderBoundsCenter_z_2");
        eUser_rayCastData_O = new StreamOutlet(eUser_rayCastData_I);
        
        // interaction sphere positions
        #region interaction spheres
        // gaze sphere
        eUser_gazeSpherePos_I = new StreamInfo(
            "eUser_gazeSpherePos",
            "Markers",
            3,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_gazeSpherePos_I.desc().append_child("eUser_gazeSpherePos_x");
        eUser_gazeSpherePos_I.desc().append_child("eUser_gazeSpherePos_y");
        eUser_gazeSpherePos_I.desc().append_child("eUser_gazeSpherePos_z");
        eUser_gazeSpherePos_O = new StreamOutlet(eUser_gazeSpherePos_I);
        
        // point sphere
        eUser_pointSpherePos_I = new StreamInfo(
            "eUser_pointSpherePos",
            "Markers",
            3,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_pointSpherePos_I.desc().append_child("eUser_pointSpherePos_x");
        eUser_pointSpherePos_I.desc().append_child("eUser_pointSpherePos_y");
        eUser_pointSpherePos_I.desc().append_child("eUser_pointSpherePos_z");
        eUser_pointSpherePos_O = new StreamOutlet(eUser_pointSpherePos_I);
        
        // touch sphere
        eUser_touchSpherePos_I = new StreamInfo(
            "eUser_touchSpherePos",
            "Markers",
            3,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_touchSpherePos_I.desc().append_child("eUser_touchSpherePos_x");
        eUser_touchSpherePos_I.desc().append_child("eUser_touchSpherePos_y");
        eUser_touchSpherePos_I.desc().append_child("eUser_touchSpherePos_z");
        eUser_touchSpherePos_O = new StreamOutlet(eUser_touchSpherePos_I);
        #endregion
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
