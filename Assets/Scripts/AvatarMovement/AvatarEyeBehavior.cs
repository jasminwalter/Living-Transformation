using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class AvatarEyeBehavior : MonoBehaviour
            {
                
                [SerializeField] private List<EyeShapeTable_v2> EyeShapeTables;
                [Space(300)]
                /// <summary>
                /// Customize this curve to fit the blend shapes of your avatar.
                /// </summary>
                [SerializeField] private AnimationCurve EyebrowAnimationCurveUpper;
                /// <summary>
                /// Customize this curve to fit the blend shapes of your avatar.
                /// </summary>
                [SerializeField] private AnimationCurve EyebrowAnimationCurveLower;
                /// <summary>
                /// Customize this curve to fit the blend shapes of your avatar.
                /// </summary>
                [SerializeField] private AnimationCurve EyebrowAnimationCurveHorizontal;

                public bool NeededToGetData = true;
                private Dictionary<EyeShape_v2, float> EyeWeightings = new Dictionary<EyeShape_v2, float>();
                private AnimationCurve[] EyebrowAnimationCurves = new AnimationCurve[(int)EyeShape_v2.Max];
                
                private static EyeData_v2 eyeData = new EyeData_v2();
                private bool eye_callback_registered = false;
                
                // Start is called before the first frame update
                void Start()
                {
                    AnimationCurve[] curves = new AnimationCurve[(int)EyeShape_v2.Max];
                    for (int i = 0; i < EyebrowAnimationCurves.Length; ++i)
                    {
                        if (i == (int)EyeShape_v2.Eye_Left_Up || i == (int)EyeShape_v2.Eye_Right_Up) curves[i] = EyebrowAnimationCurveUpper;
                        else if (i == (int)EyeShape_v2.Eye_Left_Down || i == (int)EyeShape_v2.Eye_Right_Down) curves[i] = EyebrowAnimationCurveLower;
                        else curves[i] = EyebrowAnimationCurveHorizontal;
                    }
                    SetEyeShapeAnimationCurves(curves);

                }

                // Update is called once per frame
                void Update()
                {
                    SRanipal_Eye_API.GetEyeData_v2(ref eyeData);
                    
                    bool isLeftEyeActive = false;
                    bool isRightEyeAcitve = false;
                    if (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING)
                    {
                        isLeftEyeActive = eyeData.no_user; 
                        isRightEyeAcitve = eyeData.no_user;
                    }
                    else if (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT)
                    {
                        isLeftEyeActive = true;
                        isRightEyeAcitve = true;
                    }

                    if (isLeftEyeActive || isRightEyeAcitve)
                    {
                        if (eye_callback_registered == true)
                            SRanipal_Eye_v2.GetEyeWeightings(out EyeWeightings, eyeData);
                        else
                            SRanipal_Eye_v2.GetEyeWeightings(out EyeWeightings);
                        UpdateEyeShapes(EyeWeightings);
                    }
                    else
                    {
                        for (int i = 0; i < (int)EyeShape_v2.Max; ++i)
                        {
                            bool isBlink = ((EyeShape_v2)i == EyeShape_v2.Eye_Left_Blink || (EyeShape_v2)i == EyeShape_v2.Eye_Right_Blink);
                            EyeWeightings[(EyeShape_v2)i] = isBlink ? 1 : 0;
                        }

                        UpdateEyeShapes(EyeWeightings);

                        return;
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
                }
                
                
            }
        }
    }
}
