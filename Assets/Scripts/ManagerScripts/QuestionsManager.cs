using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;
using Random = System.Random;

public class QuestionsManager : MonoBehaviour
{
    public static QuestionsManager Instance { get; private set; } 
    
    // Question Overview Game Objects
    [Header("Game Objects")]
    public GameObject languageSelection;
    public GameObject englishUI;
    public GameObject germanUI;

    public GameObject playerVR;
    public GameObject cameraVR;
    public GameObject heightQuestion;
    public GameObject canvasUIAll;

    public GameObject welcomeText;
    public GameObject welcomeNext;
    
    public GameObject consentCheck;
    public GameObject consentInputField;

    public GameObject numVisitsQuestion;
    public GameObject numVisitsQRound1;
    public GameObject numVisitsQRound2;
    public GameObject numVisitInput;

    public GameObject ageQuestion;
    public GameObject ageInput;

    public GameObject genderQuestion;

    public GameObject emotionQuestions;
    public GameObject emotionQuestionText;
    public GameObject emotionTable;
    public GameObject emotionHandle;
    public GameObject emotionHandleOrigin;
    public GameObject emotionNext;

    public GameObject relationQuestion;

    public GameObject connectionQuestion;
    public GameObject connectionTable;

    public GameObject eyeInformation;
    
    // other public variables
    [Header("Other variables")]
    public bool german = false;
    public bool english = true;
    
    public bool languageQuestionAnswered = false;

    public float heightThreshold = 1.4f;
    public float cameraHeight;
    public bool triggerHeightQuestion = false;
    public bool inWheelchair = false;
    public bool smallBodySize = false;
    public bool noHeightIssue = false;
    public bool heightQuestionAnswered = false;
    
    
    public bool displayWelcomeText = false;
    public bool fadeInWelcomeNext = false;
    public bool welcomeNextTrigger = false;
    public float welcometextTimer = 0.0f;
    public float welcomeNextThreshold = 5.0f;
    public float welcomeTextExitThreshold = 120.0f;
    
    public bool consentCheckAnswered = false;
    public bool numVisitsAnswered = false;
    public bool notFirstVisit = false;
    public int numberOfVisits;
    public bool ageQuestionAnswered = false;
    public int age;
    public bool genderQuestionAnswered = false;
    public string gender;
    
    public bool startQuestions = false;
    public bool emotionQuestionAnswered = false;
    public int emotionNumAnswered = 0; 
    public List<string> emotionList = new List<string>()
        { "sad", "anxious", "annoyed", "happy", "calm/relaxed", "excited" };
    public float currentEmotionRating = 0.0f;
    public float[] emotionRatingList = new float[]{0.0f,0.0f,0.0f,0.0f,0.0f,0.0f};
    private static Random rng;
    public bool emotionQtransition = false;
    public float targetHeight = 50.0f;

    public bool relationQuestionAnswered = false;
    public string relation;
    
    public bool connectionTransition = false;
    public bool connectionQuestionAnswered = false;
    public float connectionRating = 0.0f;

    public bool eyeInfoFadeOut = false;
    public bool calibratingSystemFinished = false;
    
    public bool inPrepRoom = true;
    private bool _fadingOut = false;
    private bool _fadingIn = false;
    private float _tFading = 0.0f;
    private bool _movingUp = false;
    private bool _movingDown = false;
    private float _tMoving = 0.0f;
    private bool _handleBack = false;
    private float _tHandle = 0.0f;

    private void OnEnable()
    {
        //cameraVR = playerVR.GetComponent<GameObject>().Find("VRCamera");
        //languageSelection.SetActive(true);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // language question
        if (languageQuestionAnswered)
        {
            // save answer
            //languageSelection.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, 0.1f);
            // deactivate question and activate next one

            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingOut(languageSelection);
                }
                else
                {
                    languageSelection.SetActive(false);
                    _fadingOut = false;
                    _tFading = 0.0f;
                    cameraHeight = cameraVR.GetComponent<Transform>().position.y +50.0f;

                    if (cameraHeight < heightThreshold)
                    {
                        heightQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
                        heightQuestion.SetActive(true);
                        _fadingIn = true;

                    }
                    else
                    {
                        // trigger next question
                        welcomeText.GetComponent<CanvasGroup>().alpha = 0.0f;
                        welcomeText.SetActive(true);
                        _fadingIn = true;
                        
                    }
                }
            }

            if (_fadingIn)
                {
                    if (_tFading < 1.0f)
                    {
                        if (cameraHeight < heightThreshold)
                        {
                            CanvasFadingIn(heightQuestion);
                        }
                        else
                        {
                            CanvasFadingIn(welcomeText);
                        }
                    }
                    else
                    {
                        _fadingIn = false;
                        _tFading = 0.0f;
                        languageQuestionAnswered = false;
                        
                        if (!(cameraHeight < heightThreshold))
                        {
                            displayWelcomeText = true;

                        }
                    }
                }
        }

        if (heightQuestionAnswered)
        {
            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingOut(heightQuestion);
                }
                else
                {
                    heightQuestion.SetActive(false);
                    _fadingOut = false;
                    _tFading = 0.0f;

                    if (inWheelchair | smallBodySize)
                    {
                        canvasUIAll.GetComponent<Transform>().position = new Vector3(0.0f,-48.56f,0.0f); // original value is y = -48.46
                    }
                    welcomeText.GetComponent<CanvasGroup>().alpha = 0.0f;
                    welcomeText.SetActive(true);
                    _fadingIn = true;
                }
            }

            if (_fadingIn)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingIn(welcomeText);
                }
                else
                {
                    _fadingIn = false;
                    _tFading = 0.0f;
                     heightQuestionAnswered = false;
                     displayWelcomeText = true;
                }
            }
            
        }
        // when the welcome text is displayed, start timer
        if (displayWelcomeText)
        {
            welcometextTimer += Time.deltaTime;
            // if timer exceeds threshold, fade in next button
            if (!fadeInWelcomeNext && welcometextTimer > welcomeNextThreshold && welcometextTimer < welcomeTextExitThreshold)
            {
                fadeInWelcomeNext = true;
                welcomeNext.GetComponent<CanvasGroup>().alpha = 0.0f;
                welcomeNext.SetActive(true);
            }

            if (fadeInWelcomeNext)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingIn(welcomeNext);
                }
            }

            if (!welcomeNextTrigger && welcometextTimer > welcomeTextExitThreshold)
            {
                welcomeNextTrigger = true;
                _fadingOut = true;
                _tFading = 0.0f;
            }

            if (welcomeNextTrigger)
            {

                if (_fadingOut)
                {
                    if (_tFading < 1.0f)
                    {

                        CanvasFadingOut(welcomeText);

                    }
                    else
                    {
                        welcomeText.SetActive(false);
                        _fadingOut = false;
                        _tFading = 0.0f;

                        // trigger next question

                        consentCheck.GetComponent<CanvasGroup>().alpha = 0.0f;
                        consentCheck.SetActive(true);
                        
                        _fadingIn = true;
                    }
                }

                if (_fadingIn)
                {
                    if (_tFading < 1.0f)
                    {
                        CanvasFadingIn(consentCheck);
                    }
                    else
                    {
                        _fadingIn = false;
                        _tFading = 0.0f;
                        displayWelcomeText = false;
                    }
                }
            }
        }

        // consent check question
        if (consentCheckAnswered)
        {
            
            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {

                    CanvasFadingOut(consentCheck);

                }
                else
                {
                    consentCheck.SetActive(false);
                    _fadingOut = false;
                    _tFading = 0.0f;

                    // trigger next question
                    
                    numVisitsQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
                    numVisitsQuestion.SetActive(true);
                    _fadingIn = true;
                }
            }
            // fade in the next question
            if (_fadingIn)
            {
                if (_tFading < 1.0f)
                {

                    CanvasFadingIn(numVisitsQuestion);

                }
                else
                {
                    _fadingIn = false;
                    _tFading = 0.0f;
                    
                    consentCheckAnswered = false;
                    
                }
            }
        }

        // how often visit exhibition question
        // if not the first visit - trigger part 2
        if (notFirstVisit)
        {
            if (_fadingOut && _tFading < 1.0f)
            {

                CanvasFadingOut(numVisitsQRound1);

            }
            else if(_fadingOut && !(_tFading < 1.0f))
            {
                numVisitsQRound1.SetActive(false);
                _fadingOut = false;
                _tFading = 0.0f;
                _fadingIn = true;

                numVisitsQRound2.GetComponent<CanvasGroup>().alpha = 0.0f;
                numVisitsQRound2.SetActive(true);

            }
            else if (_fadingIn && _tFading < 1.0f)
            {
                CanvasFadingIn(numVisitsQRound2);
                
            }
            else
            {
                _tFading = 0.0f;
                _fadingIn = false;
                notFirstVisit = false;
            }
        
        }

        // if question is answered, save answer and trigger next question
        if (numVisitsAnswered)
        {
            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingOut(numVisitsQuestion);
                }
                else
                {
                    numVisitsQuestion.SetActive(false);
                    _fadingOut = false;
                    _tFading = 0.0f;
                
                    // trigger next question
                    _fadingIn = true;
                    ageQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
                    ageQuestion.SetActive(true);
                }
            }

            if (_fadingIn)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingIn(ageQuestion);
                }
                else
                {

                    _fadingIn = false;
                    _tFading = 0.0f;

                    numVisitsAnswered = false;
                }
                
            }
            
        }
        
        // age question

        if (ageQuestionAnswered)
        {
            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingOut(ageQuestion);
                }
                else
                {
                    ageQuestion.SetActive(false);
                    _fadingOut = false;
                    _tFading = 0.0f;
                
                    // trigger next question
                    _fadingIn = true;
                    genderQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
                    genderQuestion.SetActive(true);
                }
                
            }
            
            if (_fadingIn)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingIn(genderQuestion);
                }
                else
                {
                    _fadingIn = false;
                    _tFading = 0.0f;
                    ageQuestionAnswered= false;
                }
            }
        }
        
        
        // gender question
        if (genderQuestionAnswered)
        {
            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingOut(genderQuestion);
                }
                else
                {
                    genderQuestion.SetActive(false);
                    _fadingOut = false;
                    _tFading = 0.0f;

                    // trigger next question
                    _fadingIn = true;
                    _movingUp = true;
                    emotionQtransition = true;

                    rng = new Random();
                    Shuffle(emotionList);
                    emotionQuestionText.GetComponent<TextMeshProUGUI>().text =
                        "At the moment, how much are you " + emotionList[0] + "?";
                    emotionQuestions.GetComponent<CanvasGroup>().alpha = 0.0f;
                    emotionQuestions.SetActive(true);

                    genderQuestionAnswered = false;
                }

            }
        }

        if (emotionQtransition)
            {

                if (_fadingIn)
                {
                    if (_tFading < 1.0f)
                    {
                        CanvasFadingIn(emotionQuestions);
                    }
                    else
                    {
                        _fadingIn = false;
                        _tFading = 0.0f;
                    }
                }

                if (_movingUp)
                {
                    if (_tMoving < 1.0f)
                    {
                        TablePullUp(emotionTable);
                    }
                    else
                    {
                        _movingUp = false;
                        _tMoving = 0.0f;
                    }
                }

                if (!_fadingIn & !_movingUp)
                {
                    emotionQtransition = false;
                }
            
            }
        
        
        // emotion questions
        if (emotionQuestionAnswered)
        {
            if (emotionNumAnswered < 6)
            {
                if (_fadingOut)
                {
                    if (_tFading < 1.0f)
                    {
                        CanvasFadingOut(emotionQuestions);
                    }
                    else
                    {
                        _fadingOut = false;
                        _tFading = 0.0f;
                    }
                }

                if (!_fadingOut && !_fadingIn)
                {
                    emotionQuestionText.GetComponent<TextMeshProUGUI>().text =
                        "At the moment, how much are you " + emotionList[emotionNumAnswered] + "?";
                    _fadingIn = true;
                    emotionNext.GetComponent<TextMeshProUGUI>().color = Color.black;

                    emotionRatingList[emotionNumAnswered - 1] = currentEmotionRating;

                    _handleBack = true;
                }

                if (_fadingIn)
                {
                    if (_tFading < 1.0f)
                    {
                        CanvasFadingIn(emotionQuestions);
                    }
                    else
                    {
                        _fadingIn = false;
                        _tFading = 0.0f;
                    }
                }

                if (_handleBack)
                {
                    if (_tHandle < 1.0f)
                    {
                        MoveHandleToStart(emotionHandle, emotionHandle.GetComponent<Transform>().position.x);
                    }
                    else
                    {
                        _tHandle = 0.0f;
                        _handleBack = false;
                    }
                }

                if (!_fadingIn & !_handleBack)
                {
                    emotionQuestionAnswered = false;
                }
                
            }
            
            else
            {
                if (_fadingOut)
                {
                    if (_tFading < 1.0f)
                    {
                        CanvasFadingOut(emotionQuestions);
                    }
                    else
                    {
                        _fadingOut = false;
                        _tFading = 0.0f;
                    }
                }

                if (_movingDown)
                {
                    if (_tMoving < 1.0f)
                    {
                        TablePullDown(emotionTable);
                    }
                    else
                    {
                        _movingDown = false;
                        _tMoving = 0.0f;
                    }
                }

                if (!_fadingOut & !_movingDown)
                {
                    genderQuestionAnswered = false;
                    
                    
                    // trigger next question/part
                    _fadingIn = true;
                    relationQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
                    relationQuestion.SetActive(true);

                }
                
                if (_fadingIn)
                {
                    if (_tFading < 1.0f)
                    {
                        CanvasFadingIn(relationQuestion);
                    }
                    else
                    {
                        _fadingIn = false;
                        _tFading = 0.0f;
                        emotionQuestionAnswered = false;
                    }
                }
            }
        }


        if (relationQuestionAnswered)
        {
            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingOut(relationQuestion);
                }
                else
                {
                    relationQuestion.SetActive(false);
                    _fadingOut = false;
                    _tFading = 0.0f;
                
                    // trigger next question

                    _fadingIn = true;
                    _movingUp = true;
                    emotionQtransition = true;

                    connectionQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
                    connectionQuestion.SetActive(true);

                    relationQuestionAnswered = false;
                }
            }
        }
        
        if (connectionTransition)
        {
            if (_fadingIn)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingIn(connectionQuestion);
                }
                else
                {
                    _fadingIn = false;
                    _tFading = 0.0f;
                }
            }

            if (_movingUp)
            {
                if (_tMoving < 1.0f)
                {
                    TablePullUp(connectionTable);
                }
                else
                {
                    _movingUp = false;
                    _tMoving = 0.0f;
                }
            }

            if (!_fadingIn & !_movingUp)
            {
                emotionQtransition = false;
            }

        }

        if (connectionQuestionAnswered)
        {
            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingOut(connectionQuestion);
                }
                else
                {
                    _fadingOut = false;
                    _tFading = 0.0f;
                }
            }

            if (_movingDown)
            {
                if (_tMoving < 1.0f)
                {
                    TablePullDown(connectionTable);
                }
                else
                {
                    _movingDown = false;
                    _tMoving = 0.0f;
                }
            }

            if (!_fadingOut & !_movingDown)
            {
                _fadingIn = true;
                connectionQuestion.SetActive(false);
                relationQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
                relationQuestion.SetActive(true);
            }

            if (_fadingIn)
            {
                if (_tFading < 1.0f)
                {
                    // CanvasFadingIn(relationQuestion);
                }
                else
                {
                    _fadingIn = false;
                    _tFading = 0.0f;
                    connectionQuestionAnswered = false;
                }
            }
        }

        if (eyeInfoFadeOut)
        {
            // trigger calibration
            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {
                    CanvasFadingOut(eyeInformation);
                }
                else
                {
                    eyeInformation.SetActive(false);
                    _fadingOut = false;
                    _tFading = 0.0f;
                }

            }
            
            
        }
    }

    #region Language

    public bool GetEnglish()
    {
        return english;
    }

    public bool GetGerman()
    {
        return german;
    }

    #endregion

    #region QuestionTransitionEfffects

    private void CanvasFadingOut(GameObject canvasGroup)
    {

        canvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, _tFading);
        _tFading += 0.6f*Time.deltaTime;


    }

    private void CanvasFadingIn(GameObject canvasGroup)
    {
        canvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, _tFading);
        _tFading += 1.0f*Time.deltaTime;
        
    }
    
    private void TablePullUp(GameObject table)
    {
        float x = table.GetComponent<Transform>().position.x;
        float z = table.GetComponent<Transform>().position.z;
        table.GetComponent<Transform>().position = new Vector3(x,Mathf.Lerp(-51.7f, targetHeight, _tMoving),z);
        _tMoving += 1.0f*Time.deltaTime;

    }

    private void TablePullDown(GameObject table)
    {
        float x = table.GetComponent<Transform>().position.x;
        float y = table.GetComponent<Transform>().position.y;
        float z = table.GetComponent<Transform>().position.z;
        table.GetComponent<Transform>().position = new Vector3(x,Mathf.Lerp(y, -51.7f, _tMoving),z);
        _tMoving += 1.0f*Time.deltaTime;
        
    }

    private void MoveHandleToStart(GameObject handle, float destinationPosition)
    {
        float currentX =  handle.GetComponent<Transform>().position.x;
        float y = handle.GetComponent<Transform>().position.y;
        float z = handle.GetComponent<Transform>().position.z;
        handle.GetComponent<Transform>().position = new Vector3(Mathf.Lerp(currentX, destinationPosition, _tHandle),y, z);
        _tHandle += 1.0f*Time.deltaTime;
        
    }
    
    #endregion

    #region EmotionRandomization
    
   
    public static void Shuffle(List<string> emoList)  
    {  
        int n = emoList.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            String value = emoList[k];  
            emoList[k] = emoList[n];  
            emoList[n] = value;  
        }
    }

    

    #endregion
    
    #region ButtonResponses

    #region GeneralButtonFunctions
    public void HighlightSelectionText(TextMeshProUGUI text)
    {
        text.color = Color.white;
    }
    
    public void KeyboardDelete(GameObject inputField)
    {
        string inputText = inputField.GetComponentInChildren<TextMeshProUGUI>().text;
        if (inputText.Length > 0)
        {
            inputField.GetComponentInChildren<TextMeshProUGUI>().text = inputText.Remove(inputText.Length - 1);
        }
        
    }
    
    #endregion
    
    #region LanguageSelection
    
    public void DisplaySelectionText(GameObject buttonText)
    {
        buttonText.SetActive(true);
    }
    public void SelectionEnglish()
    {
        Debug.Log("Selection English");
        english = true;
        languageQuestionAnswered = true;
        _fadingOut = true;

    }

    public void SelectionGerman()
    {
        Debug.Log("Selection German");
        german = true;
        languageQuestionAnswered = true;
        _fadingOut = true;

    }

    #endregion

    #region HeightQuestion

    public void HeightSelection(int heightCondition)
    {
        if (heightCondition == 1)
        {
            inWheelchair = true;
            targetHeight = -50.433f;
        }

        else if (heightCondition == 2)
        {
            smallBodySize = true;
            targetHeight = -50.433f;

        }

        else if (heightCondition == 3)
        {
            noHeightIssue = true;
        }
        else
        {
            Debug.Log("Something went wrong in height question");
        }

        heightQuestionAnswered = true;
        _fadingOut = true;
    }

    #endregion

    #region WelcomeText

    public void WelcomeNext()
    {
        welcomeNextTrigger = true;
        _fadingOut = true;
        _tFading = 0.0f;
    }
    

    #endregion

    #region ConsentCheckKeyboard
    
    public void ConsentKeyBoardInput(TextMeshProUGUI keyInput)
    {
        consentInputField.GetComponentInChildren<TextMeshProUGUI>().text += keyInput.text;
    }
    public void ConsentKeyboardNext(GameObject nextButton)
    {
        // save the data
        ColorBlock cb = nextButton.GetComponent<Button>().colors;

        consentInputField.GetComponent<Button>().colors = cb;
        
        consentCheckAnswered = true;
        _fadingOut = true;
        
    }
    
    #endregion
    
    #region NumVisits

    public void FirstTimeVisit()
    {
        numberOfVisits = 1;
        numVisitsAnswered = true;
        _fadingOut = true;

    }

    public void MarkSelectionText(GameObject text)
    {
        text.GetComponent<Text>().color = Color.white;
    }

    public void NotFirstTime()
    {
        notFirstVisit = true;
        _fadingOut = true;
    }

    public void NumVisitKeyBoardInput(TextMeshProUGUI keyInput)
    {
        numVisitInput.GetComponentInChildren<TextMeshProUGUI>().text += keyInput.text;
    }

    public void NumVisitsEnter(GameObject inputNumField)
    {
        // save the data
        ColorBlock cb = inputNumField.GetComponent<Button>().colors;

        inputNumField.GetComponent<Button>().colors = cb;

        numberOfVisits = int.Parse(inputNumField.GetComponentInChildren<TextMeshProUGUI>().text);
        numVisitsAnswered = true;
        _fadingOut = true;
        
    }
    

    #endregion

    #region AgeQuestion

    public void AgeNumPad(TextMeshProUGUI keyInput)
    {
        ageInput.GetComponentInChildren<TextMeshProUGUI>().text += keyInput.text;
    }

    public void AgeEnter(GameObject numAgeInput)
    {
        // save the data
        ColorBlock cb = numAgeInput.GetComponent<Button>().colors;

        numAgeInput.GetComponent<Button>().colors = cb;

        age = int.Parse(numAgeInput.GetComponentInChildren<TextMeshProUGUI>().text);
        ageQuestionAnswered = true;
        _fadingOut = true;
    }

    #endregion

    #region GenderQuestion

    public void GenderInput(TextMeshProUGUI genderInput)
    {
        gender = genderInput.text;
        genderQuestionAnswered = true;
        _fadingOut = true;
    }

    #endregion

    #region EmotionQuestions

    public void EmotionEnter()
    {
        emotionQuestionAnswered = true;
        emotionNumAnswered += 1;
        _fadingOut = true;
        
        if (emotionNumAnswered >= 6)
        {
            _movingDown = true;
        }
    }

    #endregion

    #region RelationQuestion

    public void RelationInput(TextMeshProUGUI relationInput)
    {
        relation = relationInput.text;
        relationQuestionAnswered = true;
        _fadingOut = true;
    }

    #endregion

    #region ConnectionQuestion

    public void ConnectionEnter()
    {
        connectionQuestionAnswered = true;
        _fadingOut = true;
        _movingDown = true;
    }

    #endregion

    #region EyeCallibrationInformation

    public void CalibrationInfoFinished()
    {
        eyeInfoFadeOut = true;
        _fadingOut = true;

    }

    #endregion
    #endregion

}
