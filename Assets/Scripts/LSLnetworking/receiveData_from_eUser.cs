using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class receiveData_from_eUser : MonoBehaviour
{
    
    public bool dataReceiving;

    public GameObject receivingButton;
    
    public GameObject hmd_remote;
    public GameObject handRight_remote;
    public GameObject handLeft_remote;

    private Transform _hmd_transform;
    private Transform _handR_transform;
    private Transform _handL_transform;
    
    // Start is called before the first frame update
    void Start()
    {
        dataReceiving = false;
        
        _hmd_transform = hmd_remote.transform;
        _handR_transform = handRight_remote.transform;
        _handL_transform = handLeft_remote.transform;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
