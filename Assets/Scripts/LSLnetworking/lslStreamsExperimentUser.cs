using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class lslStreamsExperimentUser : MonoBehaviour
{
    public static lslStreamsExperimentUser Instance { get; private set; } // used to allow easy access of this script in other scripts

    public string participantUID; 
    private const double NominalRate = LSL.LSL.IRREGULAR_RATE; // irregular sampling rate
    
    
    // VR headset and controller
    public StreamInfo eUser_hmd_I;
    public StreamOutlet eUser_hmd_O;
    
    public StreamInfo eUser_controller_right_I;
    public StreamOutlet eUser_controller_right_O;
    
    public StreamInfo eUser_controller_left_I;
    public StreamOutlet eUser_controller_left_O;

    // Start is called before the first frame update
    void Start()
    {
        // VR hmd and controllers
        # region VR hmd and controller
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
        
        
        eUser_controller_left_I = new StreamInfo(
            "eUser_controllerLeft",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_controller_left_I.desc().append_child("eUser_controllerLeft_pos_x");
        eUser_controller_left_I.desc().append_child("eUser_controllerLeft_pos_y");
        eUser_controller_left_I.desc().append_child("eUser_controllerLeft_pos_z");
        eUser_controller_left_I.desc().append_child("eUser_controllerLeft_rot_x");
        eUser_controller_left_I.desc().append_child("eUser_controllerLeft_rot_y");
        eUser_controller_left_I.desc().append_child("eUser_controllerLeft_rot_z");
        eUser_controller_left_I.desc().append_child("eUser_controllerLeft_dirFor_x");
        eUser_controller_left_I.desc().append_child("eUser_controllerLeft_dirFor_y");
        eUser_controller_left_I.desc().append_child("eUser_controllerLeft_dirFor_z");
        eUser_controller_left_O = new StreamOutlet(eUser_controller_left_I);
        
        eUser_controller_right_I = new StreamInfo(
            "eUser_controllerRight",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eUser_controller_right_I.desc().append_child("eUser_controllerRight_pos_x");
        eUser_controller_right_I.desc().append_child("eUser_controllerRight_pos_y");
        eUser_controller_right_I.desc().append_child("eUser_controllerRight_pos_z");
        eUser_controller_right_I.desc().append_child("eUser_controllerRight_rot_x");
        eUser_controller_right_I.desc().append_child("eUser_controllerRight_rot_y");
        eUser_controller_right_I.desc().append_child("eUser_controllerRight_rot_z");
        eUser_controller_right_I.desc().append_child("eUser_controllerRight_dirFor_x");
        eUser_controller_right_I.desc().append_child("eUser_controllerRight_dirFor_y");
        eUser_controller_right_I.desc().append_child("eUser_controllerRight_dirFor_z");
        eUser_controller_right_O = new StreamOutlet(eUser_controller_right_I);
        # endregion
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
