using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class AvatarEyeMovement_Remote : MonoBehaviour
            {
                public Transform remoteGazeSphere;

                [SerializeField] private AnimationCurve EyebrowAnimationCurveUpper;
                /// <summary>
                /// Customize this curve to fit the blend shapes of your avatar.
                /// </summary>
                [SerializeField] private AnimationCurve EyebrowAnimationCurveLower;
                /// <summary>
                /// Customize this curve to fit the blend shapes of your avatar.
                /// </summary>
                [SerializeField] private AnimationCurve EyebrowAnimationCurveHorizontal;
                
                [SerializeField] private Transform[] EyesModels = new Transform[0];
                [SerializeField] private List<EyeShapeTable_v2> EyeShapeTables;

                /// <summary>
                /// Customize this curve to fit the blend shapes of your avatar.
                /// </summary>

                [SerializeField] private Vector3 eyeRotationOffset;
                
                public bool NeededToGetData = true;
                private Dictionary<EyeShape_v2, float> EyeWeightings = new Dictionary<EyeShape_v2, float>();
                private AnimationCurve[] EyebrowAnimationCurves = new AnimationCurve[(int)EyeShape_v2.Max];
                private GameObject[] EyeAnchors;
                private const int NUM_OF_EYES = 2;
                private static EyeData_v2 eyeData = new EyeData_v2();
                private bool eye_callback_registered = false;
                private void Start()
                {
                    

                    SetEyesModels(EyesModels[0], EyesModels[1]);
                    SetEyeShapeTables(EyeShapeTables);

                    AnimationCurve[] curves = new AnimationCurve[(int)EyeShape_v2.Max];
                    for (int i = 0; i < EyebrowAnimationCurves.Length; ++i)
                    {
                        if (i == (int)EyeShape_v2.Eye_Left_Up || i == (int)EyeShape_v2.Eye_Right_Up) curves[i] = EyebrowAnimationCurveUpper;
                        else if (i == (int)EyeShape_v2.Eye_Left_Down || i == (int)EyeShape_v2.Eye_Right_Down) curves[i] = EyebrowAnimationCurveLower;
                        else curves[i] = EyebrowAnimationCurveHorizontal;
                    }
                    SetEyeShapeAnimationCurves(curves);
                }

                private void Update()
                {
                    UpdateEyeShapes(EyeWeightings);
                       
                    UpdateGazeRay();
                    
                    
                }
                
                private void OnDestroy()
                {
                    DestroyEyeAnchors();
                }

                public void SetEyesModels(Transform leftEye, Transform rightEye)
                {
                    if (leftEye != null && rightEye != null)
                    {
                        EyesModels = new Transform[NUM_OF_EYES] { leftEye, rightEye };
                        DestroyEyeAnchors();
                        CreateEyeAnchors();
                    }
                }

                public void SetEyeShapeTables(List<EyeShapeTable_v2> eyeShapeTables)
                {
                    bool valid = true;
                    if (eyeShapeTables == null)
                    {
                        valid = false;
                    }
                    else
                    {
                        for (int table = 0; table < eyeShapeTables.Count; ++table)
                        {
                            if (eyeShapeTables[table].skinnedMeshRenderer == null)
                            {
                                valid = false;
                                break;
                            }
                            for (int shape = 0; shape < eyeShapeTables[table].eyeShapes.Length; ++shape)
                            {
                                EyeShape_v2 eyeShape = eyeShapeTables[table].eyeShapes[shape];
                                if (eyeShape > EyeShape_v2.Max || eyeShape < 0)
                                {
                                    valid = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (valid)
                        EyeShapeTables = eyeShapeTables;
                }

                public void SetEyeShapeAnimationCurves(AnimationCurve[] eyebrowAnimationCurves)
                {
                    if (eyebrowAnimationCurves.Length == (int)EyeShape_v2.Max)
                        EyebrowAnimationCurves = eyebrowAnimationCurves;
                }

                public void UpdateGazeRay()
                {
                    for (int i = 0; i < EyesModels.Length; ++i)
                    {
                        // EyesModels[i].LookAt(remoteGazeSphere);
                    }
                }

                public void UpdateEyeShapes(Dictionary<EyeShape_v2, float> eyeWeightings)
                {
                    foreach (var table in EyeShapeTables)
                        RenderModelEyeShape(table, eyeWeightings);
                }
                

                private void RenderModelEyeShape(EyeShapeTable_v2 eyeShapeTable, Dictionary<EyeShape_v2, float> weighting)
                {
                    
                    AnimationCurve curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal1_local.x];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(9, curve.Evaluate(ExperimentManager.Instance().eyeShapePart1_local.x) * 100f);

                    curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal1_local.y];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(10, curve.Evaluate(ExperimentManager.Instance().eyeShapePart1_local.y) * 100f);
                    
                    curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal1_local.z];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(11, curve.Evaluate(ExperimentManager.Instance().eyeShapePart1_local.z) * 100f);
                    
                    curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal1_local.w];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(12, curve.Evaluate(ExperimentManager.Instance().eyeShapePart1_local.w) * 100f);
                    
                    
                    
                    curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal2_local.x];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(13, curve.Evaluate(ExperimentManager.Instance().eyeShapePart2_local.x) * 100f);

                    curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal2_local.y];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(14, curve.Evaluate(ExperimentManager.Instance().eyeShapePart2_local.y) * 100f);
                    
                    curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal2_local.z];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(15, curve.Evaluate(ExperimentManager.Instance().eyeShapePart2_local.z) * 100f);
                    
                    curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal2_local.w];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(16, curve.Evaluate(ExperimentManager.Instance().eyeShapePart2_local.w) * 100f);
                    
                    
                    curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal3_local.x];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(17, curve.Evaluate(ExperimentManager.Instance().eyeShapePart3_local.x) * 100f);
                    
                    curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().EyeBlinkVal3_local.y];
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(18, curve.Evaluate(ExperimentManager.Instance().eyeShapePart3_local.y) * 100f);
                    
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(19, ExperimentManager.Instance().eyeShapePart3_local.z * 100f);
                    eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(20, ExperimentManager.Instance().eyeShapePart3_local.w * 100f);
                    
                    
                    for (int i = 0; i < 24; ++i)
                    {
                        // if(i == 19 || i == 20) eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, ExperimentManager.Instance().localEyeShapeTable[i] * 100f);
                        // else
                        {
                            // AnimationCurve curve = EyebrowAnimationCurves[(int)ExperimentManager.Instance().localEyeShape2IntTable[i]];
                            // eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, curve.Evaluate(ExperimentManager.Instance().localEyeShapeTable[i]) * 100f);
                        }

                        
                        // EyeShape_v2 eyeShape = eyeShapeTable.eyeShapes[i];
                        //
                        // if (eyeShape > EyeShape_v2.Max || eyeShape < 0) continue;
                        //
                        // if (eyeShape == EyeShape_v2.Eye_Left_Blink || eyeShape == EyeShape_v2.Eye_Right_Blink)
                        // {
                        //     eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, weighting[eyeShape] * 100f);
                        //     ExperimentManager.Instance().localEyeShapeTable[i] = weighting[eyeShape];
                        // }
                        // else
                        // {
                        //     AnimationCurve curve = EyebrowAnimationCurves[(int)eyeShape];
                        //     eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, curve.Evaluate(weighting[eyeShape]) * 100f);
                        //     
                        //     Debug.Log("Before Issue 1");
                        //     ExperimentManager.Instance().localEyeShapeTable[i] = weighting[eyeShape];
                        //     Debug.Log("Test table "+ ExperimentManager.Instance().localEyeShape2IntTable[i]);
                        //     Debug.Log("Int EyeShape "+ (int)eyeShape);
                        //     ExperimentManager.Instance().localEyeShape2IntTable[i] = (int)eyeShape;
                        // }
                    }
                }

                private void CreateEyeAnchors()
                {
                    EyeAnchors = new GameObject[NUM_OF_EYES];
                    for (int i = 0; i < NUM_OF_EYES; ++i)
                    {
                        EyeAnchors[i] = new GameObject();
                        EyeAnchors[i].name = "EyeAnchor_" + i;
                        EyeAnchors[i].transform.SetParent(gameObject.transform);
                        EyeAnchors[i].transform.localPosition = EyesModels[i].localPosition;
                        EyeAnchors[i].transform.localRotation = EyesModels[i].localRotation;
                        EyeAnchors[i].transform.localScale = EyesModels[i].localScale;
                    }
                }

                private void DestroyEyeAnchors()
                {
                    if (EyeAnchors != null)
                    {
                        foreach (var obj in EyeAnchors)
                            if (obj != null) Destroy(obj);
                    }
                }
               
            }
        }
    }
}
