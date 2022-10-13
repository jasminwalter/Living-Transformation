using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEditor.Rendering;
using UnityEngine;
using Valve.VR;
using ViveSR.anipal;
using ViveSR.anipal.Eye;

public class EyeTrackingManager : MonoBehaviour
{
    
    public static EyeTrackingManager Instance { get; private set; }
    
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
    
    
    public bool fadingOutToValidationRoom = false;
    public bool fadingInToValidationRoom = false;
    public float fadingTime = 3.0f;
    private float _currentFadeTime = 3.0f;
    
    
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
    
    private float _instructionThresholdShort = 3.0f;

    private float _instructionThresholdLong = 8.0f;
    
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
        fixationPoint.SetActive(false);
        _eyeValidationDataFrames = new List<EyeValidationData>();
    }

    // Update is called once per frame
    void Update()
    {
    }

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
        calibrationCount++;
        calibrationTries++;
        
        calibrationResult = SRanipal_Eye_v2.LaunchEyeCalibration();
        Debug.Log("Calibration success: " + calibrationResult);
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
