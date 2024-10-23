using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendSaveDataExperimentUser : MonoBehaviour
{
    private float _samplingRate = 1.0f/ 90;
    public bool dataSending;

    public GameObject eUser_UI;
    public GameObject recordingButton;
    
    public GameObject hmd_local;
    public GameObject handRight_local;
    public GameObject handLeft_local;

    private Transform _hmd_transform;
    private Transform _handR_transform;
    private Transform _handL_transform;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        dataSending = false;
        
        _hmd_transform = hmd_local.transform;
        _handR_transform = handRight_local.transform;
        _handL_transform = handLeft_local.transform;
        eUser_UI.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
