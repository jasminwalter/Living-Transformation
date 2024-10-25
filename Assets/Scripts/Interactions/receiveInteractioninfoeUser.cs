using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class receiveInteractioninfoeUser : MonoBehaviour
{
    
    public static receiveInteractioninfoeUser Instance;
    
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    
    public GameObject gazeSphereRemoteEffect;
    public GameObject interactionEffect;

    
    private float _samplingRate = 1.0f/ 90;
    
    private string[] streamNames;
    private StreamInlet[] streamInlets;
    private int[] channelCounts;
    private int[][] intSamples;
    private float[][] floatSamples;
    private string[][] stringSamples;

    private bool recentTransitionO1 = false;
    private bool recentTransitionO2 = false;
    private bool recentTransitionO3 = false;
    
    
    private bool isCoolOff = false;
    private float coolOffTimerDefault = 8.0f;
    public float coolOffTimer;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        string[] streamNames = new string[]
        {
            "eCon_makeTransitionO1",
            "eCon_makeTransitionO2",
            "eCon_makeTransitionO3",
        
        };
        
        int streamCount = streamNames.Length;
        streamInlets = new StreamInlet[streamCount];
        channelCounts = new int[streamCount];
        intSamples = new int[streamCount][];
        floatSamples = new float[streamCount][];
        stringSamples = new string[streamCount][];
        
        gazeSphereRemoteEffect.SetActive(true);
    }

    
    private IEnumerator processIncomingInteraction()
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
        switch (streamName)
        {
            case "eCon_makeTransitionO1":
                if (!recentTransitionO1)
                {
                    recentTransitionO1 = true;
                    gazeSphereRemoteEffect.SetActive(false);
                    interactionEffect.SetActive(false);
                    eUser_GazeSphere.Instance.setCoolOffPeriod();
                    object1.GetComponent<ObjectTransitions>().MakeTransition();
                }

                return;
            
            case "eCon_makeTransitionO2":
                
                if (!recentTransitionO2)
                {
                    recentTransitionO2 = true;
                    gazeSphereRemoteEffect.SetActive(false);
                    interactionEffect.SetActive(false);
                    eUser_GazeSphere.Instance.setCoolOffPeriod();
                    object2.GetComponent<ObjectTransitions>().MakeTransition();
                }

                return;
            
            case "eCon_makeTransitionO3":
                
                if (!recentTransitionO3)
                {
                    recentTransitionO3 = true;
                    gazeSphereRemoteEffect.SetActive(false);
                    interactionEffect.SetActive(false);
                    eUser_GazeSphere.Instance.setCoolOffPeriod();
                    object3.GetComponent<ObjectTransitions>().MakeTransition();
                }

                return;

        }


    }

    private void Update()
    {
        if (isCoolOff)
        {
            if (coolOffTimer > 0)
            {
                coolOffTimer -= Time.deltaTime;
            }
            else
            {
                isCoolOff = false;
                coolOffTimer = coolOffTimerDefault;
                
            }
        }
    }

    private void ProcessFloatSample(float[] sample, double timeStamp, string streamName)
    {
        Debug.LogWarning($"Received float sample from {streamName} at {timeStamp}: {string.Join(", ", sample)}");

        
    }

    private void ProcessStringSample(string[] sample, double timeStamp, string streamName)
    {
        Debug.LogWarning($"Received string sample from {streamName} at {timeStamp}: {string.Join(", ", sample)}");

    }
    
    
    

    // start stop of coroutine & timestamp function
    public void Start_processIncomingInteraction()
    {
        
        StartCoroutine( processIncomingInteraction());
    }

    public void Stopp_processIncomingInteraction()
    {
        
        StopCoroutine( processIncomingInteraction() );

            
    }
    
        
    private double GetCurrentTimestampInSeconds()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }

    
}
