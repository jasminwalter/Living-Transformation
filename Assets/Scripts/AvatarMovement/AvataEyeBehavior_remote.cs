using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;


namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class AvataEyeBehavior_remote : MonoBehaviour
            {
                public bool receiveFromECon;
                public bool receiveFromEUser;
                
                [SerializeField] private List<EyeShapeTable_v2> EyeShapeTables;

                [Space(300)]
                /// <summary>
                /// Customize this curve to fit the blend shapes of your avatar.
                /// </summary>
                [SerializeField]
                private AnimationCurve EyebrowAnimationCurveUpper;

                /// <summary>
                /// Customize this curve to fit the blend shapes of your avatar.
                /// </summary>
                [SerializeField] private AnimationCurve EyebrowAnimationCurveLower;

                /// <summary>
                /// Customize this curve to fit the blend shapes of your avatar.
                /// </summary>
                [SerializeField] private AnimationCurve EyebrowAnimationCurveHorizontal;

                private AnimationCurve[] EyebrowAnimationCurves = new AnimationCurve[(int)EyeShape_v2.Max];

                private Dictionary<EyeShape_v2, float> EyeWeightings = new Dictionary<EyeShape_v2, float>();
                
                
                private StreamInlet inlet;
                private float[] eyeMSample = new float[14];
                private string streamName;
                private bool streamInitialized = false;
                
                
                // Start is called before the first frame update
                // Start is called before the first frame update
                void Start()
                {
                    InitializeAnimationCurves();
                    InitializeStream();
                }


                // Update is called once per frame
                void Update()
                {
                    if (!streamInitialized)
                    {
                        TryInitializeStream();
                    }
                    else
                    {
                        PullAndUpdateEyeData();
                    }
                }
                
                private void InitializeAnimationCurves()
                {
                    AnimationCurve[] curves = new AnimationCurve[(int)EyeShape_v2.Max];
                    for (int i = 0; i < EyebrowAnimationCurves.Length; ++i)
                    {
                        if (i == (int)EyeShape_v2.Eye_Left_Up || i == (int)EyeShape_v2.Eye_Right_Up)
                            curves[i] = EyebrowAnimationCurveUpper;
                        else if (i == (int)EyeShape_v2.Eye_Left_Down || i == (int)EyeShape_v2.Eye_Right_Down)
                            curves[i] = EyebrowAnimationCurveLower;
                        else
                            curves[i] = EyebrowAnimationCurveHorizontal;
                    }
                    EyebrowAnimationCurves = curves;
                }

                private void InitializeStream()
                {
                    streamName = ExperimentManager.Instance.isExperimentUserVersion ? "eUser_eyeMovement" : "eCon_eyeMovement";
                }

                private void TryInitializeStream()
                {
                    StreamInfo[] streamInfos = LSL.LSL.resolve_stream("name", streamName, 1, 0.0);
                    if (streamInfos.Length > 0)
                    {
                        inlet = new StreamInlet(streamInfos[0], max_buflen: 1);
                        inlet.open_stream();
                        streamInitialized = true;
                        Debug.Log("Stream opened successfully.");
                    }
                }

                private void PullAndUpdateEyeData()
                {
                    if (inlet.pull_sample(eyeMSample, 0.0f) > 0)
                    {
                        UpdateEyeWeightingsFromSample(eyeMSample);
                        UpdateEyeShapes(EyeWeightings);
                    }
                }

                private void UpdateEyeWeightingsFromSample(float[] sample)
                {
                    EyeWeightings[EyeShape_v2.Eye_Left_Squeeze] = sample[0];
                    EyeWeightings[EyeShape_v2.Eye_Right_Squeeze] = sample[1];
                    EyeWeightings[EyeShape_v2.Eye_Left_Wide] = sample[2];
                    EyeWeightings[EyeShape_v2.Eye_Right_Wide] = sample[3];
                    EyeWeightings[EyeShape_v2.Eye_Left_Down] = sample[4];
                    EyeWeightings[EyeShape_v2.Eye_Right_Down] = sample[5];
                    EyeWeightings[EyeShape_v2.Eye_Left_Up] = sample[6];
                    EyeWeightings[EyeShape_v2.Eye_Right_Up] = sample[7];
                    EyeWeightings[EyeShape_v2.Eye_Left_Right] = sample[8];
                    EyeWeightings[EyeShape_v2.Eye_Right_Right] = sample[9];
                    EyeWeightings[EyeShape_v2.Eye_Left_Left] = sample[10];
                    EyeWeightings[EyeShape_v2.Eye_Right_Left] = sample[11];
                    EyeWeightings[EyeShape_v2.Eye_Left_Blink] = sample[12];
                    EyeWeightings[EyeShape_v2.Eye_Right_Blink] = sample[13];
                }

                public void UpdateEyeShapes(Dictionary<EyeShape_v2, float> eyeWeightings)
                {
                    foreach (var table in EyeShapeTables)
                    {
                        RenderModelEyeShape(table, eyeWeightings);
                    }
                }

                private void RenderModelEyeShape(EyeShapeTable_v2 eyeShapeTable, Dictionary<EyeShape_v2, float> weighting)
                {
                    for (int i = 0; i < eyeShapeTable.eyeShapes.Length; ++i)
                    {
                        EyeShape_v2 eyeShape = eyeShapeTable.eyeShapes[i];
                        if (eyeShape > EyeShape_v2.Max || eyeShape < 0) continue;

                        if (eyeShape == EyeShape_v2.Eye_Left_Blink || eyeShape == EyeShape_v2.Eye_Right_Blink)
                            eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, weighting[eyeShape] * 100f);
                        else
                        {
                            AnimationCurve curve = EyebrowAnimationCurves[(int)eyeShape];
                            eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, curve.Evaluate(weighting[eyeShape]) * 100f);
                        }
                    }
                }
                /*
                 public void UpdateEyeShapesR(bool leftBlink, bool rightBlink, float[] sample)
                {
                    foreach (var table in EyeShapeTables)
                    {
                        for (int i = 0; i < table.eyeShapes.Length; ++i)
                        {
                            EyeShape_v2 eyeShape = table.eyeShapes[i];
                            
                            if(eyeShape == EyeShape_v2.Eye_Left_Squeeze)
                            {
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[0]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Right_Squeeze)
                            {
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[1]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Left_Wide)
                            {
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[2]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Right_Wide)
                            {
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[3]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Left_Down){
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[4]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Right_Down){
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[5]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Left_Up){
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[6]);
                            } 
                            else if(eyeShape == EyeShape_v2.Eye_Right_Up){
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[7]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Left_Right){
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[8]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Right_Right){
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[9]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Left_Left){
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[10]);
                            }
                            else if(eyeShape == EyeShape_v2.Eye_Right_Left){
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[11]);
                            }
                            else if (eyeShape == EyeShape_v2.Eye_Left_Blink)
                            {
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[12]);
                            }
                            else if (eyeShape == EyeShape_v2.Eye_Right_Blink)
                            {
                                table.skinnedMeshRenderer.SetBlendShapeWeight(i, sample[13]);
                            }
                        }

                    }
                }
                
                public void SetEyeShapeAnimationCurves(AnimationCurve[] eyebrowAnimationCurves)
                {
                    if (eyebrowAnimationCurves.Length == (int)EyeShape_v2.Max)
                        EyebrowAnimationCurves = eyebrowAnimationCurves;
                }
                
                public void UpdateEyeShapes(Dictionary<EyeShape_v2, float> eyeWeightings)
                {
                    foreach (var table in EyeShapeTables)
                        RenderModelEyeShape(table, eyeWeightings);
                }

                
                private void RenderModelEyeShape(EyeShapeTable_v2 eyeShapeTable, Dictionary<EyeShape_v2, float> weighting)
                {
                    for (int i = 0; i < eyeShapeTable.eyeShapes.Length; ++i)
                    {
                        EyeShape_v2 eyeShape = eyeShapeTable.eyeShapes[i];
                        if (eyeShape > EyeShape_v2.Max || eyeShape < 0) continue;

                        if (eyeShape == EyeShape_v2.Eye_Left_Blink || eyeShape == EyeShape_v2.Eye_Right_Blink)
                            eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, weighting[eyeShape] * 100f);
                        else
                        {
                            AnimationCurve curve = EyebrowAnimationCurves[(int)eyeShape];
                            eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, curve.Evaluate(weighting[eyeShape]) * 100f);
                        }
                    }
                } */

            }
        }
    }
}
