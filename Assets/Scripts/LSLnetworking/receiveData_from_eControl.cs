using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class receiveData_from_eControl : MonoBehaviour
{
    public static receiveData_from_eControl Instance { get; private set; } // used to allow easy access of this script in other scripts
    
    private float _samplingRate;
    public bool processIncomingData;
    
    public GameObject receivingButton;
    
    // vr vars
    public GameObject hmd_remote;
    public GameObject handRight_remote;
    public GameObject handLeft_remote;

    private Transform _hmd_transform;
    private Transform _handR_transform;
    private Transform _handL_transform;
    
    // interaction sphere variables
    public GameObject gazeSphere_remote;
    public GameObject pointSphere_remote;
    public GameObject touchSphere_remote;
    
    private Transform _gazeSphere_transform;
    private Transform _pointSphere_transform;
    private Transform _touchSphere_transform;

    // receiving data vars
    private string[] streamNames;
    private StreamInlet[] streamInlets;
    private int[] channelCounts;
    private int[][] intSamples;
    private float[][] floatSamples;
    private string[][] stringSamples;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Ensure that this instance is the only one and is accessible globally
        if (Instance == null)
        {
            Instance = this;
        }
        
        processIncomingData = false;
        _samplingRate = 1.0f / 90;
        
        // VR vars
        _hmd_transform = hmd_remote.transform;
        _handR_transform = handRight_remote.transform;
        _handL_transform = handLeft_remote.transform;
        
        // interaction spheres
        _gazeSphere_transform = gazeSphere_remote.transform;
        _pointSphere_transform = pointSphere_remote.transform;
        _touchSphere_transform = touchSphere_remote.transform;
        
        
        streamNames = new string[]
        {
            "eCon_hmd",
            "eCon_handRight",
            "eCon_handLeft",
            "eCon_gazeSpherePos",
            "eCon_pointSpherePos",
            "eCon_touchSpherePos",
            "eCon_eyeMovement"
            
        };
        
        int streamCount = streamNames.Length;
        streamInlets = new StreamInlet[streamCount];
        channelCounts = new int[streamCount];
        intSamples = new int[streamCount][];
        floatSamples = new float[streamCount][];
        stringSamples = new string[streamCount][];
        
    }
    
     private IEnumerator processIncomingData_from_ExperimentControl()
     {
        // continuously process incoming data until stopped
        while(true)
        {
            // keep track when starting to send data
            double timeBeginnSample = GetCurrentTimestampInSeconds();
            
            // pull samples
            for (int i = 0; i < streamNames.Length; i++)
            {
                if (streamInlets[i] == null)
                {
                    ResolveStream(streamNames[i], ref streamInlets[i], ref channelCounts[i]);
                }

                if (streamInlets[i] != null)
                {
                    if (streamInlets[i].info().channel_format() == channel_format_t.cf_float32)
                    {
                        PullAndProcessFloatSample(streamInlets[i], ref floatSamples[i], channelCounts[i],
                            streamNames[i]);
                    }
                    else if (streamInlets[i].info().channel_format() == channel_format_t.cf_int32)
                    {
                        PullAndProcessIntSample(streamInlets[i], ref intSamples[i], channelCounts[i], streamNames[i]);
                    }
                    else if (streamInlets[i].info().channel_format() == channel_format_t.cf_string)
                    {
                        PullAndProcessStringSample(streamInlets[i], ref stringSamples[i], channelCounts[i],
                            streamNames[i]);
                    }
                }
            }
            
            // wait until restarting coroutine to match sampling rate
            double timeEndSample = GetCurrentTimestampInSeconds();
            //Debug.Log(1/(timeEndSample- timeBeginnSample));
            if ((timeEndSample - timeBeginnSample) < _samplingRate) 
            {
                yield return new WaitForSeconds((float)(_samplingRate - (timeEndSample - timeBeginnSample)));
            }
        }
        
    }

     // processing functions
    private void ResolveStream(string streamName, ref StreamInlet inlet, ref int channelCount)
    {
        StreamInfo[] streamInfos = LSL.LSL.resolve_stream("name", streamName, 1, 0.0);

        if (streamInfos.Length > 0)
        {
            inlet = new StreamInlet(streamInfos[0], max_buflen:1);
            channelCount = inlet.info().channel_count();
            inlet.open_stream();
        }
    }
    
    private void PullAndProcessIntSample(StreamInlet inlet, ref int[] sample, int channelCount, string streamName)
    {
        if (sample == null || sample.Length != channelCount)
        {
            sample = new int[channelCount];
        }

        double lastTimeStamp = inlet.pull_sample(sample, 0.0f);
        double mostRecentTimeStamp = lastTimeStamp;
        
        while (lastTimeStamp != 0.0)
        {
            mostRecentTimeStamp = lastTimeStamp;
            lastTimeStamp = inlet.pull_sample(sample, 0.0f);   
        }
        
        ProcessIntSample(sample, mostRecentTimeStamp, streamName);
        
    }


    private void PullAndProcessFloatSample(StreamInlet inlet, ref float[] sample, int channelCount, string streamName)
    {
        if (sample == null || sample.Length != channelCount)
        {
            sample = new float[channelCount];
        }

        double lastTimeStamp = inlet.pull_sample(sample, 0.0f);
        
        double mostRecentTimeStamp = lastTimeStamp;
        
        while (lastTimeStamp != 0.0)
        {
            mostRecentTimeStamp = lastTimeStamp;
            lastTimeStamp = inlet.pull_sample(sample, 0.0f);   
        }

        ProcessFloatSample(sample, mostRecentTimeStamp, streamName);
        
    }

    private void PullAndProcessStringSample(StreamInlet inlet, ref string[] sample, int channelCount, string streamName)
    {
        if (sample == null || sample.Length != channelCount)
        {
            sample = new string[channelCount];
        }

        double lastTimeStamp = inlet.pull_sample(sample, 0.0f);
        double mostRecentTimeStamp = lastTimeStamp;
        
        while (lastTimeStamp != 0.0)
        {
            mostRecentTimeStamp = lastTimeStamp;
            lastTimeStamp = inlet.pull_sample(sample, 0.0f);   
        }

        
        ProcessStringSample(sample, mostRecentTimeStamp, streamName);
        
    }

    
    // actual data handeling
     private void ProcessIntSample(int[] sample, double timeStamp, string streamName)
    {
        Debug.LogWarning($"Received int sample from {streamName} at {timeStamp}: {string.Join(", ", sample)}");


    }

    private void ProcessFloatSample(float[] sample, double timeStamp, string streamName)
    {
        // Debug.LogWarning($"Received float sample from {streamName} at {timeStamp}: {string.Join(", ", sample)}");

        switch (streamName)
        {
          
            case "eCon_hmd":
                
                Vector3 hmdPos = new Vector3(sample[0], sample[1], sample[2]);
                Vector3 hmdRot = new Vector3(sample[3], sample[4], sample[5]);
                
                _hmd_transform.position = hmdPos;
                _hmd_transform.rotation = Quaternion.Euler(hmdRot);
                
                break;
            
            case "eCon_handRight":
                
                Vector3 handRPos = new Vector3(sample[0], sample[1], sample[2]);
                Vector3 handRRot = new Vector3(sample[3], sample[4], sample[5]);

                _handR_transform.position = handRPos;
                _handR_transform.rotation = Quaternion.Euler(handRRot);
                
                break;
            
            case "eCon_handLeft":
                
                Vector3 handLPos = new Vector3(sample[0], sample[1], sample[2]);
                Vector3 handLRot = new Vector3(sample[3], sample[4], sample[5]);

                _handL_transform.position = handLPos;
                _handL_transform.rotation = Quaternion.Euler(handLRot);

                break;
            
            case "eCon_gazeSpherePos":
                Vector3 gazeSPos = new Vector3(sample[0], sample[1], sample[2]);
                _gazeSphere_transform.position = gazeSPos;
                break;
            
            case "eCon_pointSpherePos":
                Vector3 pointSPos = new Vector3(sample[0], sample[1], sample[2]);
                _pointSphere_transform.position = pointSPos;
                break;
            
            case "eCon_touchSpherePos":
                Vector3 tSPos = new Vector3(sample[0], sample[1], sample[2]);
                _touchSphere_transform.position = tSPos;
                break;
            
            case "eCon_eyeMovement":
                Debug.Log("received eye blenshapes");
                break;
                

            
        }
    }

    private void ProcessStringSample(string[] sample, double timeStamp, string streamName)
    {
        Debug.LogWarning($"Received string sample from {streamName} at {timeStamp}: {string.Join(", ", sample)}");

    }
    
    
    

    // start stop of coroutine & timestamp function
    public void StartProcessingIncomingData_from_ExperimentControl()
    {
        
        StartCoroutine( processIncomingData_from_ExperimentControl());
        processIncomingData = true;
        receivingButton.SetActive(true);
    }

    public void StoppProcessingIncomingData_from_ExperimentControl()
    {
        
        StopCoroutine( processIncomingData_from_ExperimentControl() );
        processIncomingData = false;
        receivingButton.SetActive(false);
            
    }
    
        
    private double GetCurrentTimestampInSeconds()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }

}
