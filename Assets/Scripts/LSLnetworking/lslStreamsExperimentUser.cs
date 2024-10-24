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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
