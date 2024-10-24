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
            LSL.channel_format_t.cf_double64);
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
