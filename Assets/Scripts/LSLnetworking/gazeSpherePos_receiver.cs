using System.Collections;
using System.Collections.Generic;
using LSL;
using UnityEngine;

public class gazeSpherePos_receiver : MonoBehaviour
{
    public float InterpolationFactor = 5.0f;
    
    private string[] streamNames = {"gazeSpherePos"};

    private StreamInlet[] streamInletGSP;
    private int[] channelCounts;
    private int[][] intSamples;
    private float[][] floatSamples;
    private string[][] stringSamples;
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        int streamCount = streamNames.Length;
        streamInletGSP = new StreamInlet[streamCount];
        channelCounts = new int[streamCount];
        intSamples = new int[streamCount][];
        floatSamples = new float[streamCount][];
        stringSamples = new string[streamCount][];

        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < streamNames.Length; i++)
        {
            if (streamInletGSP[i] == null)
            {
                ResolveStream(streamNames[i], ref streamInletGSP[i], ref channelCounts[i]);
            }

            if (streamInletGSP[i] != null)
            {
                if (streamInletGSP[i].info().channel_format() == channel_format_t.cf_float32)
                {
                    PullAndProcessFloatSample(streamInletGSP[i], ref floatSamples[i], channelCounts[i], streamNames[i]);
                }

            }
        }
    }
    
    private void PullAndProcessFloatSample(StreamInlet inlet, ref float[] sample, int channelCount, string streamName)
    {
        if (sample == null || sample.Length != channelCount)
        {
            sample = new float[channelCount];
        }

        double lastTimeStamp = inlet.pull_sample(sample, 0.0f);

        if (lastTimeStamp != 0.0)
        {
            ProcessFloatSample(sample, lastTimeStamp, streamName);
        }
    }

        private void ProcessFloatSample(float[] streamSample, double timeStamp, string streamName)
        {
            // Debug.LogWarning($"Received float sample from {streamName} at {timeStamp}: {string.Join(", ", sample)}");

            //this.transform.position = Vector3.Lerp(this.transform.position,sample,Time.deltaTime * InterpolationFactor);
            Debug.Log(streamSample);
                    
        }
                
        private void ResolveStream(string streamName, ref StreamInlet inlet, ref int channelCount)
        {
            StreamInfo[] streamInfos = LSL.LSL.resolve_stream("name", streamName, 1, 0.0);

            if (streamInfos.Length > 0)
            {
                inlet = new StreamInlet(streamInfos[0]);
                channelCount = inlet.info().channel_count();
                inlet.open_stream();
            }
        }





}
