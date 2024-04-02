using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using System.Timers;
using UnityEditor.Rendering;
using UnityEngine;
using Valve.VR;
using ViveSR.anipal;
using ViveSR.anipal.Eye;
using Valve.VR.InteractionSystem;
using Hand = Valve.VR.InteractionSystem.Hand;
//OpenXR Vive Plugin
using VIVE.OpenXR.FacialTracking;
using System.Runtime.InteropServices;

public class EyeTrackingManager : MonoBehaviour
{
    
    public static EyeTrackingManager Instance { get; private set; }
    private bool showGazeSphere = true;
    
    // public variables assigned in the inspector

    // public QuestionsManager questionsManager;
    // public GameObject calibrationFailText;
    // public GameObject validationFailText;
    // public GameObject calibrationSuccessText;
    // public GameObject validationSuccessText;
    
    //
    // public bool startCalibration;
    // public bool startValidation;

    public bool isCalibratingSystem = false;
    
    public int calibrationTries = 0;
    // public List<int> calibrationOverviewAmountOfTries;
    // public List<bool> calibrationOverviewSuccessOfTries;
    public int calibrationCount = 0;
    public bool calibrationResult;
    
    public bool calibrationSuccess;
    public bool calibrationFailed;

    public int validationTries = 0;
    public bool validationSuccess;
    public bool validationFail;
    
    
    public bool validationOnGoing;
    public bool validationCountdown = false;

    public bool calibrationProblem = false;

    public bool increasesETThreshold = false;
    public bool noEyeTrackingFunctionality = false;

    [Header("visual transition variables")]

    public FadingCamera fadingCamera;
    public GameObject preparationRoom;

    public GameObject validationRoomObjects;
    public GameObject etInstructionCanvas;
    
    public GameObject avatarSelection;
    public GameObject mirrorCamera;
    public GameObject localAvatar;
    public Vector3 footOffsetPrepRoom = new Vector3(0,0,0);
    
    
    public bool fadingOutToValidationRoom = false;
    public bool fadingInToValidationRoom = false;
    public float fadingTime = 3.0f;
    private float _currentFadeTime = 3.0f;

    public bool startCal = false;
    
    
    [Header("Validation variables")]
    
    #region Fields

    [Space] [Header("Eye-tracker validation field")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject fixationPoint;
    [SerializeField] private List<Vector3> keyPositions;

    private bool _isValidationRunning;
    private bool _isErrorCheckRunning;
    private bool _isExperiment;
    
    private int _validationId;
    private int _calibrationFreq;
    
    private string _participantId;
    private string _sessionId;

    private Coroutine _runValidationCo;
    private Coroutine _runErrorCheckCo;
    private Transform _hmdTransform;
    private List<EyeValidationData> _eyeValidationDataFrames;
    private EyeValidationData _eyeValidationData;
    private const float ErrorThreshold = 1.0f;
    private float _totalError;
    
    #endregion

    #region InstructionTexts
    
    // English instructions
    [Header("English Instructions")] 
    

    public GameObject instCalSuccessE;
    public GameObject instCalFailE;
    
    public GameObject instValSuccessE;
    public GameObject instValFailE;
    
    public GameObject instCalValProblemE;

    // German instructions
    [Header("German Instructions")]

    public GameObject instCalSuccessG;
    public GameObject instCalFailG;
    
    public GameObject instValSuccessG;
    public GameObject instValFailG;
    
    public GameObject instCalValProblemG;
    
    // instruction placeholders

    private GameObject _instructionCalibrationSuccess;
    private GameObject _instructionCalibrationFail;
    
    private GameObject _instructionValidationSuccess;
    private GameObject _instructionValidationFail;
    
    private GameObject _instructionCalValProblem;

    // threshold variables
    private float _instructionThresholdShort = 3.0f;

    private float _instructionThresholdLong = 8.0f;

    

    #endregion

    #region Recording Variables

    // Do ray cast for left and right individually as well
    [Header ("Settings")]
    public bool rayCastLeftAndRightEye;
    public int numberOfRaycastHitsToSave; // if set to 0 or lower, save all 
    
    // SteamVR
    [Header ("Referenced SteamVR entities")]
    public Hand steamVrLeftHand;
    public Hand steamVrRightHand;
    
    // Body
    [Header("NavMeshAgent entity")] 
    // public GameObject playerBody;
    
    // Debug 
    [Header ("Debug")]
    // public LineRenderer debugLineRendererLeft; 
    // public LineRenderer debugLineRendererRight;
    // public LineRenderer debugLineRendererCombined;
    // public bool activateDebugLineRenderers;
   
    
    // BodyTracker
    // private int bodyTrackerIndex;
    // private GameObject bodyTracker;
    
    // Keep track of last timestamp of data point 
    private double lastDataPointTimeStamp; 
    
    // Keep track of current trial data
    // private ExperimentTrialData currentTrialData;
    
    // Sampling Rate, default 90Hz
    private float samplingRate = 90.0f;
    
    // Sampling interval in seconds 
    private float samplingInterval;
    
    // Store recorded data in memory until saving to disk
    // private List<ExperimentTrialData> trials; 
    
    // Is recording? 
    private bool isRecording;
    
    
    // Calibration 
    private int numberOfCalibrationAttempts;
    private bool calibrationIsRunning;
    private bool calibrated;

    // gaze spheres
    public float localGazeSpherePosition;
    public GameObject localGazeSphere;

    /*
    //Map OpenXR eye shapes to avatar blend shapes
    private static Dictionary<XrEyeShapeHTC, SkinnedMeshRendererShape> ShapeMap;
    public SkinnedMeshRenderer HeadskinnedMeshRenderer;
    private static float[] blendshapes = new float[60]; */

    #endregion
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        localGazeSphere = GameObject.Find("local_gazeSphere");
        
        fixationPoint.SetActive(false);
        _eyeValidationDataFrames = new List<EyeValidationData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (showGazeSphere)
        {
            // StartRecording();
            // testGazeSphere = false;
            int layerGazeSphereLocal = 1 << LayerMask.NameToLayer("LocalGazeSphere");
            
            Transform hmdTransform = Player.instance.hmdTransform;;
            SRanipal_Eye_v2.GetVerboseData(out VerboseData verboseData); 
            var eyePositionCombinedWorld = verboseData.combined.eye_data.gaze_origin_mm / 1000 + hmdTransform.position;
            Vector3 coordinateAdaptedGazeDirectionCombined = new Vector3(verboseData.combined.eye_data.gaze_direction_normalized.x * -1,  verboseData.combined.eye_data.gaze_direction_normalized.y, verboseData.combined.eye_data.gaze_direction_normalized.z);
            var eyeDirectionCombinedWorld = hmdTransform.rotation * coordinateAdaptedGazeDirectionCombined;
            
            RaycastHit firstHit;
            if (Physics.Raycast(eyePositionCombinedWorld, eyeDirectionCombinedWorld, out firstHit, Mathf.Infinity))
            {
                localGazeSphere.transform.position = firstHit.point;
            
            }

            if (startCal)
            {
                StartCalButton();
                startCal = false;
            }
        }
    }

    #region RecordingData

    public void StartRecording()
    {
        StartCoroutine(RecordData());
    }

    public void EndRecording()
    {
        StopCoroutine(RecordData());
    }
        
    // Get a timestamp 
    private double GetCurrentTimestampInSeconds()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }
    
    
    // Holds data of one data point  

    // Record Data 
    private IEnumerator RecordData()  // orig: RecordControllerTriggerAndPositionData
    {
        Debug.Log("[EyeTrackingRecorder] Started Coroutine to Record Data.");
        
        // Measure until stopped
        while (true)
        {
            Debug.Log("Run RecordData");
            // Create new data point for current measurement data point 
            ExperimentDataPoint dataPoint = new ExperimentDataPoint(); // orig: FrameData(), custom class do not confuse with Unity class  
            
            
            //// ** 
            // Add supplementary info to data point before raycasting 

            // TimeStamp at start 
            double timeAtStart = GetCurrentTimestampInSeconds();
            dataPoint.timeStampDataPointStart = timeAtStart;
            
            // HMD and Hand Transforms
            Transform hmdTransform = Player.instance.hmdTransform;;
            Transform leftHandTransform = steamVrLeftHand.transform; 
            Transform rightHandTransform = steamVrRightHand.transform; // right hand
        
            // Set Hand Data 
            dataPoint.handLeftPosition = leftHandTransform.position; 
            dataPoint.handLeftRotation = leftHandTransform.rotation.eulerAngles; 
            dataPoint.handLeftScale = leftHandTransform.lossyScale;
            dataPoint.handLeftDirectionForward = leftHandTransform.forward;
            dataPoint.handLeftDirectionRight = leftHandTransform.right;
            dataPoint.handLeftDirectionUp = leftHandTransform.up;
            dataPoint.handRightPosition = rightHandTransform.position; 
            dataPoint.handRightRotation = rightHandTransform.rotation.eulerAngles; 
            dataPoint.handRightScale = rightHandTransform.lossyScale;
            dataPoint.handRightDirectionForward = rightHandTransform.forward;
            dataPoint.handRightDirectionRight = rightHandTransform.right;
            dataPoint.handRightDirectionUp = rightHandTransform.up;
            
            // Set HMD Data  
            dataPoint.hmdPosition = hmdTransform.position; 
            dataPoint.hmdDirectionForward = hmdTransform.forward; 
            dataPoint.hmdDirectionUp = hmdTransform.up; 
            dataPoint.hmdDirectionRight = hmdTransform.right; 
            dataPoint.hmdRotation = hmdTransform.rotation.eulerAngles; 
            
            // Set Body data
            // dataPoint.playerBodyPosition = playerBody.transform.position;

            // EyeData 
            
            // Time stamp before obtaining verbose data 
            double timeBeforeGetVerboseData = GetCurrentTimestampInSeconds();
            dataPoint.timeStampGetVerboseData = timeBeforeGetVerboseData;
            
            // Obtain verbose data and later extract all relevant data from it 
            SRanipal_Eye_v2.GetVerboseData(out VerboseData verboseData); 
            
            // Extract gaze information for left, right and combined eye 
            //
            // verboseData's gaze_origin_mm has the same value as the ray's origin gotten through GetGazeRay, only multiplied by a factor of 1000 (millimeter vs meter) 
            // verboseData's gaze_direction_normalized has the same value as the ray's direction gotten through GetGazeRay, only the x axis needs to be inverted (according to SRanipal Docs: verboseData's gaze has right handed coordinate system) 
            // 
            Vector3 coordinateAdaptedGazeDirectionCombined = new Vector3(verboseData.combined.eye_data.gaze_direction_normalized.x * -1,  verboseData.combined.eye_data.gaze_direction_normalized.y, verboseData.combined.eye_data.gaze_direction_normalized.z);
            dataPoint.eyePositionCombinedWorld = verboseData.combined.eye_data.gaze_origin_mm / 1000 + hmdTransform.position;
            dataPoint.eyeDirectionCombinedWorld = hmdTransform.rotation * coordinateAdaptedGazeDirectionCombined;
            dataPoint.eyeDirectionCombinedLocal = coordinateAdaptedGazeDirectionCombined;
            Vector3 coordinateAdaptedGazeDirectionLeft = new Vector3(verboseData.left.gaze_direction_normalized.x * -1,  verboseData.left.gaze_direction_normalized.y, verboseData.left.gaze_direction_normalized.z);
            dataPoint.eyePositionLeftWorld = verboseData.left.gaze_origin_mm / 1000 + hmdTransform.position;
            dataPoint.eyeDirectionLeftWorld = hmdTransform.rotation * coordinateAdaptedGazeDirectionLeft;
            dataPoint.eyeDirectionLeftLocal = coordinateAdaptedGazeDirectionLeft;
            Vector3 coordinateAdaptedGazeDirectionRight = new Vector3(verboseData.right.gaze_direction_normalized.x * -1,  verboseData.right.gaze_direction_normalized.y, verboseData.right.gaze_direction_normalized.z);
            dataPoint.eyePositionRightWorld = verboseData.right.gaze_origin_mm / 1000 + hmdTransform.position;
            dataPoint.eyeDirectionRightWorld = hmdTransform.rotation * coordinateAdaptedGazeDirectionRight;
            dataPoint.eyeDirectionRightLocal = coordinateAdaptedGazeDirectionRight;
           
            // RaycastHit firstHit;
            // if (Physics.Raycast(dataPoint.eyePositionCombinedWorld, dataPoint.eyeDirectionCombinedWorld, out firstHit, Mathf.Infinity))
            // {
            //     localGazeSphere.transform.position = firstHit.point;
            //
            // }

            

            
            // Raycast combined eyes 
            RaycastHit[] raycastHitsCombined;
            raycastHitsCombined = Physics.RaycastAll(dataPoint.eyePositionCombinedWorld, dataPoint.eyeDirectionCombinedWorld,Mathf.Infinity);
            
            // Make sure something was hit 
            if (raycastHitsCombined.Length > 0)
            {
                // Sort by distance
                raycastHitsCombined = raycastHitsCombined.OrderBy(x=>x.distance).ToArray();
                
                // Use only the specified number of hits 
                if (numberOfRaycastHitsToSave > 0)
                {
                    raycastHitsCombined = raycastHitsCombined.Take(Math.Min(numberOfRaycastHitsToSave,raycastHitsCombined.Length)).ToArray();
                }
                
                // Make data serializable and save 
                dataPoint.rayCastHitsCombinedEyes = makeRayCastListSerializable(raycastHitsCombined);
                
                // // Debug
                // if (activateDebugLineRenderers)
                // {
                //     Debug.Log("[EyeTrackingRecorder] Combined eyes first hit: " + raycastHitsCombined[0].collider.name);
                //     debugLineRendererCombined.SetPosition(0,dataPoint.eyePositionCombinedWorld);
                //     debugLineRendererCombined.SetPosition(1, raycastHitsCombined[0].point);
                // }
            }
            
            
            
            
            // ** If intended, ray cast for left and right eye individually as well 
            if (rayCastLeftAndRightEye)
            {
                
                // Raycast left eye, calculate all hits 
                RaycastHit[] raycastHitsLeft;
                raycastHitsLeft = Physics.RaycastAll(dataPoint.eyePositionLeftWorld, dataPoint.eyeDirectionLeftWorld,
                    Mathf.Infinity);

                // Make sure something was hit 
                if (raycastHitsLeft.Length > 0)
                {
                    // Sort by distance
                    raycastHitsLeft = raycastHitsLeft.OrderBy(x => x.distance).ToArray();
                    
                    // Use only the specified number of hits 
                    if (numberOfRaycastHitsToSave > 0)
                    {
                        raycastHitsLeft = raycastHitsLeft.Take(Math.Min(numberOfRaycastHitsToSave,raycastHitsLeft.Length)).ToArray();
                    }
                    
                    // Make data serializable and save 
                    dataPoint.rayCastHitsLeftEye = makeRayCastListSerializable(raycastHitsLeft);

                    // // Debug
                    // if (activateDebugLineRenderers)
                    // {
                    //     Debug.Log("[EyeTrackingRecorder] Left eye first hit: " + raycastHitsLeft[0].collider.name);
                    //     debugLineRendererLeft.SetPosition(0,dataPoint.eyePositionLeftWorld);
                    //     debugLineRendererLeft.SetPosition(1, raycastHitsLeft[0].point);
                    // }
                }
                
                // Raycast right eye, calculate all hits 
                RaycastHit[] raycastHitsRight;
                raycastHitsRight = Physics.RaycastAll(dataPoint.eyePositionRightWorld, dataPoint.eyeDirectionRightWorld,
                    Mathf.Infinity);

                // Make sure something was hit 
                if (raycastHitsRight.Length > 0)
                {
                    // Sort by distance
                    raycastHitsRight = raycastHitsRight.OrderBy(x => x.distance).ToArray();

                    // Use only the specified number of hits 
                    if (numberOfRaycastHitsToSave > 0)
                    {
                        raycastHitsRight = raycastHitsRight.Take(Math.Min(numberOfRaycastHitsToSave,raycastHitsRight.Length)).ToArray();
                    }
                    
                    // Make data serializable and save 
                    dataPoint.rayCastHitsRightEye = makeRayCastListSerializable(raycastHitsRight);

                    // // Debug
                    // if (activateDebugLineRenderers)
                    // {
                    //    Debug.Log("[EyeTrackingRecorder] Right eye first hit: " + raycastHitsRight[0].collider.name);
                    //    debugLineRendererRight.SetPosition(0,dataPoint.eyePositionRightWorld);
                    //    debugLineRendererRight.SetPosition(1, raycastHitsRight[0].point);
                    // }

                }
                
            }
            
            
            
            
            // Eye Openness
            dataPoint.eyeOpennessLeft = verboseData.left.eye_openness;
            dataPoint.eyeOpennessRight = verboseData.right.eye_openness;
            
            // Pupil Diameter
            dataPoint.pupilDiameterMillimetersLeft = verboseData.left.pupil_diameter_mm;
            dataPoint.pupilDiameterMillimetersRight = verboseData.right.pupil_diameter_mm;

            // Gaze validity
            dataPoint.leftGazeValidityBitmask = verboseData.left.eye_data_validata_bit_mask;
            dataPoint.rightGazeValidityBitmask = verboseData.right.eye_data_validata_bit_mask;
            dataPoint.combinedGazeValidityBitmask = verboseData.combined.eye_data.eye_data_validata_bit_mask;

            
          
            // TimeStamp at end 
            double timeAtEnd = GetCurrentTimestampInSeconds();
            dataPoint.timeStampDataPointEnd = timeAtEnd;
            
            // End of EyeData 
            //// **
            
            
            
            
            // Add data point to current subject data 
            // currentTrialData.dataPoints.Add(dataPoint);
            
            
            
            //// **
            // Wait time to meet sampling rate  
            
            double timeBeforeWait = GetCurrentTimestampInSeconds();
            
           
            // 
            // Check how much time needs to be waited to meet sampling rate measuring next data point
            // (If lastDataPointTimeStamp is not yet set, i.e. 0, timeBeforeWait will be greater than samplingInterval so no waiting will occur)  

            // Computation was faster than sampling rate, i.e. wait to match sampling rate
            // Else: Computation was slower, i.e. continue directly with next data point 
            if ((timeBeforeWait - lastDataPointTimeStamp) < samplingInterval) 
            {
                // Debug.Log("waiting for " + (float)(samplingInterval - (timeBeforeWait - lastDataPointTimeStamp)));
                // Debug.Log(getCurrentTimestamp());

                // Wait for seconds that fill time to meet sampling interval 
                yield return new WaitForSeconds((float)(samplingInterval - (timeBeforeWait - lastDataPointTimeStamp)));
            }

            

            // Debug.Log("Real Framerate: " + 1 / (timeBeforeWait - lastDataPointTimeStamp));
            
            // Update last time stamp 
            lastDataPointTimeStamp = GetCurrentTimestampInSeconds();
            
            // Wait time End
            //// ** 
            
            

        }

        yield break;  // coroutine stops, when loop breaks 


    }
    
    private List<SerializableRayCastHit> makeRayCastListSerializable(RaycastHit[] rayCastHits)
    {
        List<SerializableRayCastHit> serilizableList = new List<SerializableRayCastHit>();

        // Keep track of the number of the hit 
        int ordinal = 1;
        
        // Go through each hit and add to list 
        foreach (RaycastHit hit in rayCastHits)
        {
            serilizableList.Add(new SerializableRayCastHit {
                hitPointOnObject = hit.point,
                hitObjectColliderName = hit.collider.name,
                hitObjectColliderBoundsCenter = hit.collider.bounds.center,
                ordinalOfHit = ordinal
            });

            ordinal += 1;
        }

        return serilizableList;

    }

    #endregion

    #region InstructionsAssignment

    public void AssignInstructionLanguage()
    {
        if(QuestionsManager.Instance.english)
        {
            
            _instructionCalibrationSuccess = instCalSuccessE;
            _instructionCalibrationFail= instCalFailE;
                
            _instructionValidationSuccess = instValSuccessE;
            _instructionValidationFail = instValFailE;
                
            _instructionCalValProblem = instCalValProblemE;
        }

        
        
        if (QuestionsManager.Instance.german)
        {
            _instructionCalibrationSuccess = instCalSuccessG;
            _instructionCalibrationFail= instCalFailG;
                
            _instructionValidationSuccess = instValSuccessG;
            _instructionValidationFail = instValFailG;
                
            _instructionCalValProblem = instCalValProblemG;
        }
    }
    
    
    #endregion
    
    #region VisualFunctionalities

    private IEnumerator SwitchToValidationRoom()
    {
        // fade out
        fadingCamera.FadeOut();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);
        
        // switch rooms
        preparationRoom.SetActive(false);
        validationRoomObjects.SetActive(true);
        etInstructionCanvas.SetActive(true);

        // fade in
        fadingCamera.FadeIn();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);

        StartCalibration();
        yield return null;

    }

    private IEnumerator SwitchBackToPreparationRoom()
    {
        // fade out
        fadingCamera.FadeOut();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);
        
        // switch rooms
        etInstructionCanvas.SetActive(false);
        validationRoomObjects.SetActive(false);
        preparationRoom.SetActive(true);
        avatarSelection.SetActive(true);
        mirrorCamera.SetActive(true);
        
        // set the normal VR Hands inactive------------------------------------
        // show avatar
        localAvatar.SetActive(true);
        localAvatar.GetComponent<VRFootIK>().footOffset = footOffsetPrepRoom;
        localGazeSphere.SetActive(true);
        showGazeSphere = true;
        

        // fade in
        fadingCamera.FadeIn();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);
        // QuestionsManager.Instance.calibratingSystemFinished = true;
        yield return null;
    }

    #endregion

    #region CalibrationRegion
    
    public void StartCalButton()
    {
        AssignInstructionLanguage();
        isCalibratingSystem = true;
        StartCoroutine(SwitchToValidationRoom());
    }

    public void StartCalibration()
    {
        // calibrationCount++;
        // calibrationTries++;
        
        calibrationResult = SRanipal_Eye_v2.LaunchEyeCalibration();
        Debug.Log("Calibration success: " + calibrationResult);
        showGazeSphere = true;
        StartCoroutine(EvaluateCalibration());

    }


    private IEnumerator EvaluateCalibration()
    {
        if (calibrationResult)
        {
            // calibrationSuccess = true;
            _instructionCalibrationSuccess.SetActive(true);
            yield return new WaitForSeconds(_instructionThresholdShort);
            _instructionCalibrationSuccess.SetActive(false);
            // calibrationSuccess = false;
            validationTries++;
            ValidateEyeTracking();
            
        }
        else
        {
            if (calibrationTries < 3)
            {
                // calibrationFailed = true;
                _instructionCalibrationFail.SetActive(true);
                yield return new WaitForSeconds(_instructionThresholdLong);
                _instructionCalibrationFail.SetActive(false);
                
                Debug.Log("Restarting calibration...");
                // calibrationFailed = false;
                StartCalibration();
                
            }
            else
            {
                calibrationProblem = true;
                _instructionCalValProblem.SetActive(true);
                yield return new WaitForSeconds(_instructionThresholdLong);
                _instructionCalValProblem.SetActive(false);
                StartCoroutine(SwitchBackToPreparationRoom());

            }

        }

        yield return null;

    }
    #endregion

    #region ValidationRegion

     private void SaveValidationFile()
      {
          var fileName = _participantId + "_EyeValidation_" + GetCurrentUnixTimeStamp();

          Debug.Log("Eye Validation Data Frame");
          Debug.Log(_eyeValidationDataFrames);
          if (_isExperiment)
          {
              // DataSavingManager.Instance.SaveList(_eyeValidationDataFrames, fileName + "_TA");
          }
          else
          {
              // DataSavingManager.Instance.SaveList(_eyeValidationDataFrames, fileName + "_Expl" + "_S_" + _sessionId);
          }
      }
                  
      private Vector3 GetValidationError()
      {
          return _eyeValidationData.EyeValidationError;
      }
              
      private IEnumerator ValidateEyeTracker(float delay=2)
      {
          if (_isValidationRunning) yield break;
          _isValidationRunning = true;
  
          Debug.Log("Start Validation");
          _validationId++;
  
          fixationPoint.transform.parent = mainCamera.gameObject.transform;
  
          _hmdTransform = Camera.main.transform;
  
          fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * new Vector3(0,0,30);
  
          fixationPoint.transform.LookAt(_hmdTransform);
          
          yield return new WaitForSeconds(.15f);
          
          fixationPoint.SetActive(true);
  
          yield return new WaitForSeconds(delay);
          
          var anglesX = new List<float>();
          var anglesY = new List<float>();
          var anglesZ = new List<float>();
          
          for (var i = 1; i < keyPositions.Count; i++)
          {
              var startTime = Time.time;
              float timeDiff = 0;
  
              while (timeDiff < 1f)
              {
                  fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * Vector3.Lerp(keyPositions[i-1], keyPositions[i], timeDiff / 1f);   
                  fixationPoint.transform.LookAt(_hmdTransform);
                  yield return new WaitForEndOfFrame();
                  timeDiff = Time.time - startTime;
              }
              
              // _validationPointIdx = i;
              startTime = Time.time;
              timeDiff = 0;
              
              while (timeDiff < 2f)
              {
                  fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * keyPositions[i] ;
                  fixationPoint.transform.LookAt(_hmdTransform);
                  EyeValidationData validationData = GetEyeValidationData();
                  
                  if (validationData != null)
                  {
                      anglesX.Add(validationData.CombinedEyeAngleOffset.x);
                      anglesY.Add(validationData.CombinedEyeAngleOffset.y);
                      anglesZ.Add(validationData.CombinedEyeAngleOffset.z);
                      
                      validationData.EyeValidationError.x = CalculateValidationError(anglesX);
                      validationData.EyeValidationError.y = CalculateValidationError(anglesY);
                      validationData.EyeValidationError.z = CalculateValidationError(anglesZ);
  
                      _eyeValidationData = validationData;
                  }
                  
                  yield return new WaitForEndOfFrame();
                  timeDiff = Time.time - startTime;
              }
          }
  
          fixationPoint.transform.position = Vector3.zero;
  
          _isValidationRunning = false;
          
          fixationPoint.transform.parent = gameObject.transform;
  
          Debug.Log( "Get validation error" + GetValidationError() + " + " + _eyeValidationData.EyeValidationError);
          
          _eyeValidationDataFrames.Add(_eyeValidationData);
          SaveValidationFile();
  
  
          fixationPoint.SetActive(false);
          
          // give feedback whether the error was too large or not
          if (CalculateValidationError(anglesX) > ErrorThreshold || 
              CalculateValidationError(anglesY) > ErrorThreshold ||
              CalculateValidationError(anglesZ) > ErrorThreshold ||
              _eyeValidationData.EyeValidationError == Vector3.zero)
          {
              _totalError = CalculateValidationError(anglesX) + CalculateValidationError(anglesY) +
                               CalculateValidationError(anglesZ);
              validationFail = true;
              Debug.Log("Validation failed");
              
              if (calibrationTries < 4)
              {
                  Debug.Log("restart calibration and validation process");
                  _instructionValidationFail.SetActive(true);
                  yield return new WaitForSeconds(_instructionThresholdLong);
                  _instructionValidationFail.SetActive(false);
                  
                  validationFail = false;
                  StartCalibration();
              }
              else
              {
                  if (_totalError < 3.0f)
                  {
                      Debug.Log("Increase validation error to 3.0f + start avatar selection");
                      increasesETThreshold = true;
                      validationFail = false;
                      _instructionValidationSuccess.SetActive(true);
                      yield return new WaitForSeconds(_instructionThresholdShort);
                      _instructionValidationSuccess.SetActive(false);
                      StartCoroutine(SwitchBackToPreparationRoom());
                      // increase the gaze sphere

                  }
                  else
                  {
                      calibrationProblem = true;
                      validationFail = false;
                      noEyeTrackingFunctionality = true;
                      
                      _instructionCalValProblem.SetActive(true);
                      yield return new WaitForSeconds(_instructionThresholdLong);
                      _instructionCalValProblem.SetActive(false);
                      StartCoroutine(SwitchBackToPreparationRoom());
                  }

              }

          }
          else
          {
              validationSuccess = true;
              calibrationTries = 0;
              validationTries = 0;
              _instructionValidationSuccess.SetActive(true);
              yield return new WaitForSeconds(_instructionThresholdShort);
              _instructionValidationSuccess.SetActive(false);
              StartCoroutine(SwitchBackToPreparationRoom());

          }

          yield return null;
      }
                  
      private IEnumerator CheckErrorEyeTracker(float delay=5)
      {
          if (_isErrorCheckRunning) yield break;
          _isErrorCheckRunning = true;
  
          _validationId++;
          
          fixationPoint.transform.parent = mainCamera.gameObject.transform;
  
          _hmdTransform = Camera.main.transform;
  
          fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * new Vector3(0,0,45);
  
          fixationPoint.transform.LookAt(_hmdTransform);
  
          // show instructions -------------------------------------------------------------------------------to do-----
          if (_isExperiment)
          {
              // ExperimentManager.Instance.SetInstructionText(_instructions.ErrorCheckInstruction);
  
              yield return new WaitForSeconds(2);
  
              // ExperimentManager.Instance.SetInstructionText("");
          }
          else
          {
              // ExplorationManager.Instance.SetInstructionText(_instructions.ErrorCheckInstruction);
  
              yield return new WaitForSeconds(2);
  
              // ExplorationManager.Instance.SetInstructionText("");
          }
          
          yield return new WaitForSeconds(.15f);
  
          fixationPoint.SetActive(true);
  
          yield return new WaitForSeconds(delay);
          
          var anglesX = new List<float>();
          var anglesY = new List<float>();
          var anglesZ = new List<float>();
          
          EyeValidationData validationData = GetEyeValidationData();
              
          if (validationData != null)
          {
              anglesX.Add(validationData.CombinedEyeAngleOffset.x);
              anglesY.Add(validationData.CombinedEyeAngleOffset.y);
              anglesZ.Add(validationData.CombinedEyeAngleOffset.z);
                      
              validationData.EyeValidationError.x = CalculateValidationError(anglesX);
              validationData.EyeValidationError.y = CalculateValidationError(anglesY);
              validationData.EyeValidationError.z = CalculateValidationError(anglesZ);
  
              _eyeValidationData = validationData;
          }
  
          fixationPoint.transform.position = Vector3.zero;
  
          _isErrorCheckRunning = false;
          
          fixationPoint.transform.parent = gameObject.transform;
  
          Debug.Log( "Get validation error" + GetValidationError() + " + " + _eyeValidationData.EyeValidationError);
          
          _eyeValidationDataFrames.Add(_eyeValidationData);
          SaveValidationFile();
  
  
          fixationPoint.SetActive(false);
          
          // give feedback whether the error was too large or not
          if (CalculateValidationError(anglesX) > ErrorThreshold || 
              CalculateValidationError(anglesY) > ErrorThreshold ||
              CalculateValidationError(anglesZ) > ErrorThreshold ||
              _eyeValidationData.EyeValidationError == Vector3.zero)
          {
              // give instructions ---------------------------------------------------------------------------- to do---
              if (_isExperiment)
              {
                  // ExperimentManager.Instance.SetValidationSuccessStatus(false);
              }
              else
              {
                  // ExplorationManager.Instance.SetValidationSuccessStatus(false);
              }
          }
          else
          {
              if (_isExperiment)
              {
                  // ExperimentManager.Instance.SetValidationSuccessStatus(true);
              }
              else
              {
                  // ExplorationManager.Instance.SetValidationSuccessStatus(true);
              }
          }
      }
                  
      private EyeValidationData GetEyeValidationData()
      {
          EyeValidationData eyeValidationData = new EyeValidationData();
          
          Ray ray;
          
          eyeValidationData.UnixTimestamp = GetCurrentUnixTimeStamp();
          eyeValidationData.IsErrorCheck = _isErrorCheckRunning;
          
          eyeValidationData.ParticipantID = _participantId;
          eyeValidationData.ValidationID = _validationId;
          eyeValidationData.CalibrationFreq = _calibrationFreq;
          
          eyeValidationData.PointToFocus = fixationPoint.transform.position;
  
          if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out ray))
          {
              var angles = Quaternion.FromToRotation((fixationPoint.transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                  .eulerAngles;
              
              eyeValidationData.LeftEyeAngleOffset = angles;
          }
          
          if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out ray))
          {
              var angles = Quaternion.FromToRotation((fixationPoint.transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                  .eulerAngles;
  
              eyeValidationData.RightEyeAngleOffset = angles;
          }
  
          if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out ray))
          {
              var angles = Quaternion.FromToRotation((fixationPoint.transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                  .eulerAngles;
  
              eyeValidationData.CombinedEyeAngleOffset = angles;
          }
  
          return eyeValidationData;
      }
                  
      private float CalculateValidationError(List<float> angles)
      {
          return angles.Select(f => f > 180 ? Mathf.Abs(f - 360) : Mathf.Abs(f)).Sum() / angles.Count;
      }
                  
    #endregion
              
    #region Public methods
              
      public void SetExperimentStatus(bool status)
      {
          _isExperiment = status;
      }

      public void ValidateEyeTracking()
      {
          if(!_isValidationRunning) _runValidationCo = StartCoroutine(ValidateEyeTracker());
      }
      
      public void CheckErrorEyeTracking()
      {
          if(!_isErrorCheckRunning) _runErrorCheckCo = StartCoroutine(CheckErrorEyeTracker());
      }

      public void SetParticipantId(string id)
      {
          _participantId = id;
      }
      
      public void SetSessionId(string id)
      {
          _sessionId = id;
      }
      
      public void NotifyCalibrationFrequency()
      {
          _calibrationFreq++;
      }
              
      // public void SetInstructions(Instructions instruct)
      // {
      //     _instructions = instruct;
      // }
                  


    #endregion
    
    public double GetCurrentUnixTimeStamp()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }
}

#region Serializable

// Make RayCastHits Serializable
[Serializable]
public struct SerializableRayCastHit
{
    public Vector3 hitPointOnObject;
    public string hitObjectColliderName;
    public Vector3 hitObjectColliderBoundsCenter;
    public int ordinalOfHit; // starts at 1 
}


[Serializable]
public class ExperimentDataPoint
{
    // TimeStamps 
    public double timeStampDataPointStart;
    public double timeStampDataPointEnd;
    public double timeStampGetVerboseData;

    // EyeTracking 
    public float eyeOpennessLeft;
    public float eyeOpennessRight;
    public float pupilDiameterMillimetersLeft;
    public float pupilDiameterMillimetersRight;
    public Vector3 eyePositionCombinedWorld;
    public Vector3 eyeDirectionCombinedWorld;
    public Vector3 eyeDirectionCombinedLocal;
    public Vector3 eyePositionLeftWorld;
    public Vector3 eyeDirectionLeftWorld;
    public Vector3 eyeDirectionLeftLocal;
    public Vector3 eyePositionRightWorld;
    public Vector3 eyeDirectionRightWorld;
    public Vector3 eyeDirectionRightLocal;
    public ulong leftGazeValidityBitmask;
    public ulong rightGazeValidityBitmask;
    public ulong combinedGazeValidityBitmask;
    
    // GazeRay hit object 
    public List<SerializableRayCastHit> rayCastHitsCombinedEyes;
    public List<SerializableRayCastHit> rayCastHitsLeftEye;
    public List<SerializableRayCastHit> rayCastHitsRightEye;
    
    // HMD 
    public Vector3 hmdPosition;
    public Vector3 hmdDirectionForward;
    public Vector3 hmdDirectionRight;
    public Vector3 hmdRotation;
    public Vector3 hmdDirectionUp;
    
    // Hands
    public Vector3 handLeftPosition;
    public Vector3 handLeftRotation;
    public Vector3 handLeftScale;
    public Vector3 handLeftDirectionForward;
    public Vector3 handLeftDirectionRight;
    public Vector3 handLeftDirectionUp;
    public Vector3 handRightPosition;
    public Vector3 handRightRotation;
    public Vector3 handRightScale;
    public Vector3 handRightDirectionForward;
    public Vector3 handRightDirectionRight;
    public Vector3 handRightDirectionUp;
    
    // Body
    public Vector3 playerBodyPosition; 
    
    // Body Tracker 
    public Vector3 bodyTrackerPosition;
    public Vector3 bodyTrackerRotation;


}

#endregion