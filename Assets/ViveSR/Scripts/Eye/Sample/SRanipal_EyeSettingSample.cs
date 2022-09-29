//========= Copyright 2018, HTC Corporation. All rights reserved. ===========
using System;
using UnityEngine;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class SRanipal_EyeSettingSample : MonoBehaviour
            {
                private void OnGUI()
                {
                    if (GUILayout.Button("Set Parameter"))
                    {
                        EyeParameter parameter = new EyeParameter
                        {
                            gaze_ray_parameter = new GazeRayParameter(),
                        };
                        Error error = SRanipal_Eye_API.GetEyeParameter(ref parameter);
                        Debug.Log("GetEyeParameter: " + error + "\n" +
                                  "sensitive_factor: " + parameter.gaze_ray_parameter.sensitive_factor);

                        parameter.gaze_ray_parameter.sensitive_factor = parameter.gaze_ray_parameter.sensitive_factor == 1 ? 0.015f : 1;
                        error = SRanipal_Eye_API.SetEyeParameter(parameter);
                        Debug.Log("SetEyeParameter: " + error + "\n" +
                                  "sensitive_factor: " + parameter.gaze_ray_parameter.sensitive_factor);
                    }

                    if (GUILayout.Button("Launch Calibration"))
                    {
                        bool testCal = false;
                        SRanipal_Eye_API.IsUserNeedCalibration(ref testCal);
                        Debug.Log("Does user need calibration? " + testCal);
                        
                        // SRanipal_Eye_API.LaunchEyeCalibration(IntPtr.Zero);
                        var result = SRanipal_Eye_API.LaunchEyeCalibration(IntPtr.Zero);
                        Debug.Log("Calibration result: " + result);
                        EyeParameter test = new EyeParameter();
                        var sensitivity = SRanipal_Eye_API.GetEyeParameter(ref test);
                        Debug.Log("Eye sensitivity parameter: " + test.gaze_ray_parameter.sensitive_factor);
                        
                        SRanipal_Eye_API.IsUserNeedCalibration(ref testCal);
                        Debug.Log("Does user need calibration? " + testCal);
                    }
                }
            }
        }
    }
}
