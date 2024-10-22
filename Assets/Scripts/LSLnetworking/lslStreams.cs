using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class lslStreams : MonoBehaviour
{
    
    private string participantUID; 
    private const double NominalRate = LSL.LSL.IRREGULAR_RATE; // irregular sampling rate
    
    
    public StreamInfo lslITimestamps;
    public StreamOutlet lslOTimestamps;
    
    // gaze sphere
    public StreamInfo gazeSpherePos_I;
    public StreamOutlet gazeSpherePos_O;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        // gaze sphere pos
        gazeSpherePos_I = new StreamInfo(
            "gazeSpherePos",
            "Markers",
            3,
            NominalRate,
            LSL.channel_format_t.cf_float32);
        gazeSpherePos_I.desc().append_child("posX");
        gazeSpherePos_I.desc().append_child("posY");
        gazeSpherePos_I.desc().append_child("posZ");
        gazeSpherePos_O = new StreamOutlet(gazeSpherePos_I);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
