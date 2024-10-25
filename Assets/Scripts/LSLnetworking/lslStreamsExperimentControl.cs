using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class lslStreamsExperimentControl : MonoBehaviour
{
    
    public static lslStreamsExperimentControl Instance { get; private set; } // used to allow easy access of this script in other scripts

    public string participantUID; 
    private const double NominalRate = LSL.LSL.IRREGULAR_RATE; // irregular sampling rate
    
    // general variables

    public StreamInfo eCon_timeStamp_StartSample_I;
    public StreamOutlet eCon_timeStamp_StartSample_O;
    
    // VR headset and hands
    public StreamInfo eCon_hmd_I;
    public StreamOutlet eCon_hmd_O;
    
    public StreamInfo eCon_hand_right_I;
    public StreamOutlet eCon_hand_right_O;
    
    public StreamInfo eCon_hand_left_I;
    public StreamOutlet eCon_hand_left_O;
    
    // gaze data
    public StreamInfo eCon_eyeMovement_I;
    public StreamOutlet eCon_eyeMovement_O;


    // interaction spheres
    public StreamInfo eCon_gazeSpherePos_I;
    public StreamOutlet eCon_gazeSpherePos_O;
    
    public StreamInfo eCon_pointSpherePos_I;
    public StreamOutlet eCon_pointSpherePos_O;
    
    public StreamInfo eCon_touchSpherePos_I;
    public StreamOutlet eCon_touchSpherePos_O;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Ensure that this instance is the only one and is accessible globally
        if (Instance == null)
        {
            Instance = this;
        }
        
        // general variables
        eCon_timeStamp_StartSample_I = new StreamInfo(
            "eCon_timeStamp_StartSample", 
            "Markers", 
            1, 
            NominalRate,
            LSL.channel_format_t.cf_double64);
        eCon_timeStamp_StartSample_O = new StreamOutlet(eCon_timeStamp_StartSample_I);
        
        // VR hmd and hand
        # region VR hmd and hands
        eCon_hmd_I = new StreamInfo(
            "eCon_hmd",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eCon_hmd_I.desc().append_child("eCon_hmd_pos_x");
        eCon_hmd_I.desc().append_child("eCon_hmd_pos_y");
        eCon_hmd_I.desc().append_child("eCon_hmd_pos_z");
        eCon_hmd_I.desc().append_child("eCon_hmd_rot_x");
        eCon_hmd_I.desc().append_child("eCon_hmd_rot_y");
        eCon_hmd_I.desc().append_child("eCon_hmd_rot_z");
        eCon_hmd_I.desc().append_child("eCon_hmd_dirFor_x");
        eCon_hmd_I.desc().append_child("eCon_hmd_dirFor_y");
        eCon_hmd_I.desc().append_child("eCon_hmd_dirFor_z");
        eCon_hmd_O = new StreamOutlet(eCon_hmd_I);
        
        
        eCon_hand_left_I = new StreamInfo(
            "eCon_handLeft",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eCon_hand_left_I.desc().append_child("eCon_handLeft_pos_x");
        eCon_hand_left_I.desc().append_child("eCon_handLeft_pos_y");
        eCon_hand_left_I.desc().append_child("eCon_handLeft_pos_z");
        eCon_hand_left_I.desc().append_child("eCon_handLeft_rot_x");
        eCon_hand_left_I.desc().append_child("eCon_handLeft_rot_y");
        eCon_hand_left_I.desc().append_child("eCon_handLeft_rot_z");
        eCon_hand_left_I.desc().append_child("eCon_handLeft_dirFor_x");
        eCon_hand_left_I.desc().append_child("eCon_handLeft_dirFor_y");
        eCon_hand_left_I.desc().append_child("eCon_handLeft_dirFor_z");
        eCon_hand_left_O = new StreamOutlet(eCon_hand_left_I);
        
        eCon_hand_right_I = new StreamInfo(
            "eCon_handRight",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eCon_hand_right_I.desc().append_child("eCon_handRight_pos_x");
        eCon_hand_right_I.desc().append_child("eCon_handRight_pos_y");
        eCon_hand_right_I.desc().append_child("eCon_handRight_pos_z");
        eCon_hand_right_I.desc().append_child("eCon_handRight_rot_x");
        eCon_hand_right_I.desc().append_child("eCon_handRight_rot_y");
        eCon_hand_right_I.desc().append_child("eCon_handRight_rot_z");
        eCon_hand_right_I.desc().append_child("eCon_handRight_dirFor_x");
        eCon_hand_right_I.desc().append_child("eCon_handRight_dirFor_y");
        eCon_hand_right_I.desc().append_child("eCon_handRight_dirFor_z");
        eCon_hand_right_O = new StreamOutlet(eCon_hand_right_I);
        # endregion
        // gaze data
        eCon_eyeMovement_I = new StreamInfo(
            "eCon_eyeMovement", 
            "Markers", 
            14, 
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eCon_eyeMovement_I .desc().append_child("eye_left_squeeze");
        eCon_eyeMovement_I .desc().append_child("eye_right_squeeze");
        eCon_eyeMovement_I .desc().append_child("eye_left_wide");
        eCon_eyeMovement_I .desc().append_child("eye_right_wide");
        eCon_eyeMovement_I .desc().append_child("eye_left_down");
        eCon_eyeMovement_I .desc().append_child("eye_right_down");
        eCon_eyeMovement_I .desc().append_child("eye_left_up");
        eCon_eyeMovement_I .desc().append_child("eye_right_up");
        eCon_eyeMovement_I .desc().append_child("eye_left_right");
        eCon_eyeMovement_I .desc().append_child("eye_right_right");
        eCon_eyeMovement_I .desc().append_child("eye_left_left");
        eCon_eyeMovement_I .desc().append_child("eye_right_left");
        eCon_eyeMovement_I .desc().append_child("eye_left_blink");
        eCon_eyeMovement_I .desc().append_child("eye_right_blink");
        eCon_eyeMovement_O = new StreamOutlet(eCon_eyeMovement_I);
        
        // interaction sphere positions
        #region interaction spheres
        
        // gaze sphere
        eCon_gazeSpherePos_I = new StreamInfo(
            "eCon_gazeSpherePos",
            "Markers",
            3,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eCon_gazeSpherePos_I.desc().append_child("eCon_gazeSpherePos_x");
        eCon_gazeSpherePos_I.desc().append_child("eCon_gazeSpherePos_y");
        eCon_gazeSpherePos_I.desc().append_child("eCon_gazeSpherePos_z");
        eCon_gazeSpherePos_O = new StreamOutlet(eCon_gazeSpherePos_I);
        
        // point sphere
        eCon_pointSpherePos_I = new StreamInfo(
            "eCon_pointSpherePos",
            "Markers",
            3,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eCon_pointSpherePos_I.desc().append_child("eCon_pointSpherePos_x");
        eCon_pointSpherePos_I.desc().append_child("eCon_pointSpherePos_y");
        eCon_pointSpherePos_I.desc().append_child("eCon_pointSpherePos_z");
        eCon_pointSpherePos_O = new StreamOutlet(eCon_pointSpherePos_I);
        
        // touch sphere
        eCon_touchSpherePos_I = new StreamInfo(
            "eCon_touchSpherePos",
            "Markers",
            3,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eCon_touchSpherePos_I.desc().append_child("eCon_touchSpherePos_x");
        eCon_touchSpherePos_I.desc().append_child("eCon_touchSpherePos_y");
        eCon_touchSpherePos_I.desc().append_child("eCon_touchSpherePos_z"); 
        eCon_touchSpherePos_O = new StreamOutlet(eCon_touchSpherePos_I);
        
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
