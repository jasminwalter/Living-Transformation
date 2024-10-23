using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class lslStreamsExperimentControl : MonoBehaviour
{
    
    public static lslStreamsExperimentControl Instance { get; private set; } // used to allow easy access of this script in other scripts

    public string participantUID; 
    private const double NominalRate = LSL.LSL.IRREGULAR_RATE; // irregular sampling rate
    
    
    // VR headset and controller
    public StreamInfo eCon_hmd_I;
    public StreamOutlet eCon_hmd_O;
    
    public StreamInfo eCon_controller_right_I;
    public StreamOutlet eCon_controller_right_O;
    
    public StreamInfo eCon_controller_left_I;
    public StreamOutlet eCon_controller_left_O;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        #region VR headset and controller
        
        eControlhmdI = new StreamInfo(
            "eConthmd",
            "Markers",
            2,
            NominalRate,
            LSL.channel_format_t.cf_float32,
            participantUID);
        eControlhmdI.desc().append_child("hmdPosx");
        eControlhmdI.desc().append_child("hmdPosy");
        eControlhmdO = new StreamOutlet(eControlhmdI);
           
        #endregion 
        
        eControl_hmd_I.desc().append_child("hmdPos_z");
        eControl_hmd_I.desc().append_child("hmdRot_x");
        eControl_hmd_I.desc().append_child("hmdRot_y");
        eControl_hmd_I.desc().append_child("hmdRot_z");
        eControl_hmd_I.desc().append_child("hmdDirFor_x");
        eControl_hmd_I.desc().append_child("hmdDirFor_y");
        eControl_hmd_I.desc().append_child("hmdDirFor_z");
        */
        
        // VR hmd and controllers
        # region VR hmd and controller
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
        
        
        eCon_controller_left_I = new StreamInfo(
            "eCon_controllerLeft",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eCon_controller_left_I.desc().append_child("eCon_controllerLeft_pos_x");
        eCon_controller_left_I.desc().append_child("eCon_controllerLeft_pos_y");
        eCon_controller_left_I.desc().append_child("eCon_controllerLeft_pos_z");
        eCon_controller_left_I.desc().append_child("eCon_controllerLeft_rot_x");
        eCon_controller_left_I.desc().append_child("eCon_controllerLeft_rot_y");
        eCon_controller_left_I.desc().append_child("eCon_controllerLeft_rot_z");
        eCon_controller_left_I.desc().append_child("eCon_controllerLeft_dirFor_x");
        eCon_controller_left_I.desc().append_child("eCon_controllerLeft_dirFor_y");
        eCon_controller_left_I.desc().append_child("eCon_controllerLeft_dirFor_z");
        eCon_controller_left_O = new StreamOutlet(eCon_controller_left_I);
        
        eCon_controller_right_I = new StreamInfo(
            "eCon_controllerRight",
            "Markers",
            9,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        eCon_controller_right_I.desc().append_child("eCon_controllerRight_pos_x");
        eCon_controller_right_I.desc().append_child("eCon_controllerRight_pos_y");
        eCon_controller_right_I.desc().append_child("eCon_controllerRight_pos_z");
        eCon_controller_right_I.desc().append_child("eCon_controllerRight_rot_x");
        eCon_controller_right_I.desc().append_child("eCon_controllerRight_rot_y");
        eCon_controller_right_I.desc().append_child("eCon_controllerRight_rot_z");
        eCon_controller_right_I.desc().append_child("eCon_controllerRight_dirFor_x");
        eCon_controller_right_I.desc().append_child("eCon_controllerRight_dirFor_y");
        eCon_controller_right_I.desc().append_child("eCon_controllerRight_dirFor_z");
        eCon_controller_right_O = new StreamOutlet(eCon_controller_right_I);
        # endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
