using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Random = System.Random;

public class QuestionsManager : MonoBehaviour
{
    public static QuestionsManager Instance { get; private set; }
    public TrainingManager trainingManager;
    public Valve.VR.InteractionSystem.Teleport teleport;

    
    // Question Overview Game Objects
    [Header("Game Objects")]

    public GameObject playerVR;
    public GameObject cameraVR;
    
    #region UI Elements
    public GameObject languageSelection;
    public GameObject canvasUIAll;
    
    #region English
    [Header("English")]
    public GameObject heightQuestionE;
    
    public GameObject welcomeTextE;
    public GameObject welcomeNextE;
    
    public GameObject consentCheckE;
    public GameObject consentInputFieldE;

    public GameObject numVisitsQuestionE;
    public GameObject numVisitsQRound1E;
    public GameObject numVisitsQRound2E;
    public GameObject numVisitInputE;

    public GameObject ageQuestionE;
    public GameObject ageInputE;

    public GameObject genderQuestionE;

    public GameObject emotionQuestionsE;
    public GameObject emotionQuestionTextE;
    public GameObject emotionTableE;
    public GameObject emotionHandleE;
    public GameObject emotionHandleOriginE;
    public GameObject emotionNextE;

    public GameObject relationQuestionE;

    public GameObject connectionQuestionE;
    public GameObject connectionTableE;
    public GameObject connectionHandleE;
    public GameObject connectionHandleOriginE;

    public GameObject eyeInformationE;

    public GameObject avatarSelectionE;

    public GameObject trainingInformationE1;
    public GameObject trainingInformationE2;
    public GameObject trainingInformationE3;
    public GameObject trainingInformationE4;
    
    public GameObject startExhibitionE;

    public GameObject WrongDirSignE1;
    public GameObject WrongDirSignE2;

    #endregion
    #region German
    [Header("German")]
    
    public GameObject heightQuestionG;

    public GameObject welcomeTextG;
    public GameObject welcomeNextG;
    
    public GameObject consentCheckG;
    public GameObject consentInputFieldG;

    public GameObject numVisitsQuestionG;
    public GameObject numVisitsQRound1G;
    public GameObject numVisitsQRound2G;
    public GameObject numVisitInputG;

    public GameObject ageQuestionG;
    public GameObject ageInputG;

    public GameObject genderQuestionG;

    public GameObject emotionQuestionsG;
    public GameObject emotionQuestionTextG;
    public GameObject emotionTableG;
    public GameObject emotionHandleG;
    public GameObject emotionHandleOriginG;
    public GameObject emotionNextG;

    public GameObject relationQuestionG;

    public GameObject connectionQuestionG;
    public GameObject connectionTableG;
    public GameObject connectionHandleG;
    public GameObject connectionHandleOriginG;

    public GameObject eyeInformationG;

    public GameObject avatarSelectionG;

    public GameObject trainingInformationG1;
    public GameObject trainingInformationG2;
    public GameObject trainingInformationG3;
    
    public GameObject startExhibitionG;
    
    #endregion

    #region generalUIVariables

    // private UI variables
    
    private GameObject _heightQuestion;

    private GameObject _welcomeText;
    private GameObject _welcomeNext;
    
    private GameObject _consentCheck;
    private GameObject _consentInputField;

    private GameObject _numVisitsQuestion;
    private GameObject _numVisitsQRound1;
    private GameObject _numVisitsQRound2;
    private GameObject _numVisitInput;

    private GameObject _ageQuestion;
    private GameObject _ageInput;

    private GameObject _genderQuestion;

    private GameObject _emotionQuestions;
    private GameObject _emotionQuestionText;
    private GameObject _emotionTable;
    private GameObject _emotionHandle;
    private GameObject _emotionHandleOrigin;
    private GameObject _emotionNext;

    private GameObject _relationQuestion;

    private GameObject _connectionQuestion;
    private GameObject _connectionTable;
    private GameObject _connectionHandle;
    private GameObject _connectionHandleOrigin;

    private GameObject _eyeInformation;

    private GameObject _avatarSelection;

    private GameObject _trainingInformation1;
    private GameObject _trainingInformation2;
    private GameObject _trainingInformation3;
    private GameObject _trainingInformation4;

    private GameObject _startExhibition;

    private GameObject _wrongDirSign1;
    public GameObject _wrongDirSign2;

    #endregion
    
    #endregion

    public GameObject mirrorCamera; 
    
    // other public variables
    [Header("Saving variables")]
    
    public bool german = false;
    public bool english = false;
    
    
    public float cameraHeight;
    public bool inWheelchair = false;
    public bool smallBodySize = false;
    public bool noHeightIssue = false;

    private bool skipConsentCheck = false;
    public int numberOfVisits;
    public int age;
    public string gender;
    
    public int emotionNumAnswered = 0; 
    public List<string> emotionListE = new List<string>()
        { "sad", "anxious", "annoyed", "happy", "calm/relaxed", "excited" };

    public List<string> emotionListG = new List<string>()
        {"traurig", "besorgt", "genervt", "glücklich", "entspannt", "begeistert"};
    
    private List<string> _emotionList;
    public float currentEmotionRating = 0.0f;
    public float[] emotionRatingList = new float[]{0.0f,0.0f,0.0f,0.0f,0.0f,0.0f};
    
    public string relation;
    public float connectionRating = 0.0f;

    
    [Header("Other Variables")]
    
    private float _heightThreshold = 1.4f;
    
    private bool _welcomeNextTrigger = false;
    private float _welcomeNextThreshold = 5.0f;
    private float _welcomeTextExitThreshold = 120.0f;
    
    private static Random rng;
    private float _targetHeight = -50.0f;

    
    public float _fadingOutDuration = 2.0f;
    private float _fadingInDuration = 2.0f;
    private float _movingTableDuration = 2.0f;
    private float _handleResetDuration = 0.5f;


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
    
    public IEnumerator CanvasGroupFadingOut(GameObject canvasGroup)
    {
        float timer = 0.0f;
        while (timer <= _fadingOutDuration)
        {
            canvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, timer / _fadingOutDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.GetComponent<CanvasGroup>().alpha = 0;
        yield return null;
    }
    
    public IEnumerator CanvasGroupFadingIn(GameObject canvasGroup)
    {
        float timer = 0.0f;
        while (timer <= _fadingInDuration)
        {
            canvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, timer / _fadingInDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.GetComponent<CanvasGroup>().alpha = 1;
        yield return null;
    }

    private IEnumerator MoveTableUp(GameObject table)
    {
        float x = table.GetComponent<Transform>().position.x;
        float z = table.GetComponent<Transform>().position.z;

        float timer = 0.0f;
        while (timer <= _movingTableDuration)
        {
            table.GetComponent<Transform>().position = new Vector3(x,Mathf.Lerp(-51.7f, _targetHeight, timer/_movingTableDuration),z);
            
            timer += Time.deltaTime;
            yield return null;
        }
        table.GetComponent<Transform>().position = new Vector3(x, _targetHeight, z);
        yield return null;
    }

    private IEnumerator MoveTableDown(GameObject table)
    {
        float x = table.GetComponent<Transform>().position.x;
        float y = table.GetComponent<Transform>().position.y;
        float z = table.GetComponent<Transform>().position.z;
        
        float timer = 0.0f;
        while (timer <= _movingTableDuration)
        {
            table.GetComponent<Transform>().position = new Vector3(x, Mathf.Lerp(y, -51.7f, timer/_movingTableDuration), z);
                        
            timer += Time.deltaTime;
            yield return null;
        }

        table.GetComponent<Transform>().position = new Vector3(x, -51.7f, z);
        yield return null;
    }
    

    private IEnumerator ResetTableHandle(GameObject handle, float destinationPosition)
    {
        float currentX =  handle.GetComponent<Transform>().position.x;
        float y = handle.GetComponent<Transform>().position.y;
        float z = handle.GetComponent<Transform>().position.z;
        
        float timer = 0.0f;
        while (timer <= _handleResetDuration)
        {
            handle.GetComponent<Transform>().position =
                new Vector3(Mathf.Lerp(currentX, destinationPosition, timer/_handleResetDuration), y, z);
            timer += Time.deltaTime;
            yield return null;
        }

        handle.GetComponent<Transform>().position =
            new Vector3(destinationPosition, y, z);

        yield return null;
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
        StartCoroutine(HighlightRoutine(text));
    }

    private IEnumerator HighlightRoutine(TextMeshProUGUI text)
    {
        text.color = Color.white;
        yield return new WaitForSeconds(3.0f);

        text.color = Color.black;

        yield return null;
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

        StartCoroutine(LanguageSelectionTextRoutine(buttonText));
    }

    private IEnumerator LanguageSelectionTextRoutine(GameObject buttonText)
    {
        buttonText.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        buttonText.SetActive(false);
        
        yield return null;
    }
    public void SelectionEnglish()
    {
        Debug.Log("Selection English");
        english = true;
        StartCoroutine(LanguageSelectionAnswered());
    }

    public void SelectionGerman()
    {
        Debug.Log("Selection German");
        german = true;
        StartCoroutine(LanguageSelectionAnswered());
    }

    IEnumerator LanguageSelectionAnswered()
    {
        AssignLanguageVariables();

        StartCoroutine(CanvasGroupFadingOut(languageSelection));
        yield return new WaitForSeconds(_fadingOutDuration);
        
        languageSelection.SetActive(false);
        
        cameraHeight = cameraVR.GetComponent<Transform>().position.y +50.0f;
        
        
        if (cameraHeight < _heightThreshold)
        {
            _heightQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
            _heightQuestion.SetActive(true);
            StartCoroutine(CanvasGroupFadingIn(_heightQuestion));
        }
        else
        {
            // trigger next question
            StartCoroutine(FadeInWelcomeText());
        }

        yield return null;
    }

    private void AssignLanguageVariables()
    {
        if (english)
        {
            _heightQuestion = heightQuestionE;

            _welcomeText = welcomeTextE;
            _welcomeNext = welcomeNextE;
                
            _consentCheck = consentCheckE;
            _consentInputField = consentInputFieldE;

            _numVisitsQuestion = numVisitsQuestionE;
            _numVisitsQRound1 = numVisitsQRound1E;
             _numVisitsQRound2 = numVisitsQRound2E;
            _numVisitInput = numVisitInputE;

            _ageQuestion = ageQuestionE;
            _ageInput = ageInputE;

            _genderQuestion = genderQuestionE;

            _emotionList = emotionListE;
            _emotionQuestions = emotionQuestionsE;
            _emotionQuestionText = emotionQuestionTextE;
            _emotionTable = emotionTableE;
            _emotionHandle = emotionHandleE;
            _emotionHandleOrigin = emotionHandleOriginE;
            _emotionNext = emotionNextE;

            _relationQuestion = relationQuestionE;

            _connectionQuestion = connectionQuestionE;
            _connectionTable = connectionTableE;
            _connectionHandle = connectionHandleE;
            _connectionHandleOrigin =  connectionHandleOriginE;

            _eyeInformation = eyeInformationE;

            _avatarSelection = avatarSelectionE;

            _trainingInformation1 = trainingInformationE1;
            _trainingInformation2 = trainingInformationE2;
            _trainingInformation3 = trainingInformationE3;
            _trainingInformation4 = trainingInformationE4;
    
            _startExhibition = startExhibitionE;

            _wrongDirSign1 = WrongDirSignE1;
            _wrongDirSign2 = WrongDirSignE2;

        }

        if (german)
        {
            _heightQuestion = heightQuestionG;

            _welcomeText = welcomeTextG;
            _welcomeNext = welcomeNextG;
                
            _consentCheck = consentCheckG;
            _consentInputField = consentInputFieldG;

            _numVisitsQuestion = numVisitsQuestionG;
            _numVisitsQRound1 = numVisitsQRound1G;
            _numVisitsQRound2 = numVisitsQRound2G;
            _numVisitInput = numVisitInputG;

            _ageQuestion = ageQuestionG;
            _ageInput = ageInputG;

            _genderQuestion = genderQuestionG;

            _emotionList = emotionListG;
            _emotionQuestions = emotionQuestionsG;
            _emotionQuestionText = emotionQuestionTextG;
            _emotionTable = emotionTableG;
            _emotionHandle = emotionHandleG;
            _emotionHandleOrigin = emotionHandleOriginG;
            _emotionNext = emotionNextG;

            _relationQuestion = relationQuestionG;

            _connectionQuestion = connectionQuestionG;
            _connectionTable = connectionTableG;
            _connectionHandle = connectionHandleG;
            _connectionHandleOrigin =  connectionHandleOriginG;

            _eyeInformation = eyeInformationG;

            _avatarSelection = avatarSelectionG;

            _trainingInformation1 = trainingInformationG1;
            _trainingInformation2 = trainingInformationG2;
            _trainingInformation3 = trainingInformationG3;
    
            _startExhibition = startExhibitionG;
        }
        
    }

    #endregion
    
    #region HeightQuestion

    public void HeightSelection(int heightCondition)
    {
        if (heightCondition == 1)
        {
            inWheelchair = true;
            _targetHeight = -50.433f;
        }

        else if (heightCondition == 2)
        {
            smallBodySize = true;
            _targetHeight = -50.433f;

        }

        else if (heightCondition == 3)
        {
            noHeightIssue = true;
        }
        else
        {
            Debug.Log("Something went wrong in height question");
        }
        StartCoroutine(HeightQuestionAnswered());
    }

    private IEnumerator HeightQuestionAnswered()
    {

        StartCoroutine(CanvasGroupFadingOut(_heightQuestion));
        yield return new WaitForSeconds(_fadingOutDuration);
        _heightQuestion.SetActive(false);
        
        if (inWheelchair | smallBodySize)
        {
            canvasUIAll.GetComponent<Transform>().position = new Vector3(0.0f,-48.56f,0.0f); // original value is y = -48.46
        }

        StartCoroutine(FadeInWelcomeText());
        yield return null;
    }

    #endregion

    #region WelcomeText

    private IEnumerator FadeInWelcomeText()
    {
        _welcomeText.GetComponent<CanvasGroup>().alpha = 0.0f;
        _welcomeText.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_welcomeText));
        yield return new WaitForSeconds(_fadingInDuration);
        
        // start audio read out

        yield return new WaitForSeconds(_welcomeNextThreshold);
        
        // fade in next button
        _welcomeNext.GetComponent<CanvasGroup>().alpha = 0.0f;
        _welcomeNext.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_welcomeNext));
        yield return new WaitForSeconds(_fadingInDuration);
        
        // wait for the rest of the threshold time before transitioning to next question, if not already done
        yield return new WaitForSeconds(_welcomeTextExitThreshold);
        if (!_welcomeNextTrigger)
        {
            StartCoroutine(WelcomeTextFadeOut());
        }
        yield return null;
    }
    public void WelcomeNext()
    {
        _welcomeNextTrigger = true;
        StartCoroutine(WelcomeTextFadeOut());

    }
    

    private IEnumerator WelcomeTextFadeOut()
    {
        StartCoroutine(CanvasGroupFadingOut(_welcomeText));
        yield return new WaitForSeconds(_fadingOutDuration);
        _welcomeText.SetActive(false);
        _welcomeNext.SetActive(false);

        if (skipConsentCheck)
        {
            StartCoroutine(FadeInNumVisits());
        }
        else
        {
            StartCoroutine(FadeInConsentCheck());
        }
        yield return null;
    }

    #endregion

    #region ConsentCheckKeyboard

    public void EnableSkipConsentCheck()
    {
        skipConsentCheck = true;
    }

    private IEnumerator FadeInConsentCheck()
    {
        _consentCheck.GetComponent<CanvasGroup>().alpha = 0.0f;
        _consentCheck.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_consentCheck));
        yield return null;
    }
    public void ConsentKeyBoardInput(TextMeshProUGUI keyInput)
    {
        _consentInputField.GetComponentInChildren<TextMeshProUGUI>().text += keyInput.text;
    }
    public void ConsentKeyboardNext(GameObject nextButton)
    {
        // save the data
        ColorBlock cb = nextButton.GetComponent<Button>().colors;

        _consentInputField.GetComponent<Button>().colors = cb;

        StartCoroutine(ConsentCheckAnswered());
    }

    private IEnumerator ConsentCheckAnswered()
    {

        StartCoroutine(CanvasGroupFadingOut(_consentCheck));
        yield return new WaitForSeconds(_fadingOutDuration);
        _consentCheck.SetActive(false);

        StartCoroutine(FadeInNumVisits());
        
        yield return null;
    }

    #endregion
    
    #region NumVisits
    
    private IEnumerator FadeInNumVisits()
    {
        _numVisitsQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
        _numVisitsQuestion.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_numVisitsQuestion));
        yield return null;
        
    }
    
    public void FirstTimeVisit()
    {
        numberOfVisits = 1;
        StartCoroutine(NumVisitQuestionAnswered());
    }

    public void MarkSelectionText(GameObject text)
    {
        text.GetComponent<Text>().color = Color.white;
    }

    public void NotFirstTime()
    {
        StartCoroutine(FadeInNumVisitsKeyboard());

    }

    public void NumVisitKeyBoardInput(TextMeshProUGUI keyInput)
    {
        _numVisitInput.GetComponentInChildren<TextMeshProUGUI>().text += keyInput.text;
    }

    public void NumVisitsEnter(GameObject inputNumField)
    {

        StartCoroutine(CheckTextInputNumVisits(inputNumField));
    }

    private IEnumerator CheckTextInputNumVisits(GameObject inputNumField)
    {
        if (inputNumField.GetComponentInChildren<TextMeshProUGUI>().text != "")
        {
            ColorBlock cb = inputNumField.GetComponent<Button>().colors;
            
            inputNumField.GetComponent<Button>().colors = cb;

            numberOfVisits = int.Parse(inputNumField.GetComponentInChildren<TextMeshProUGUI>().text);
            StartCoroutine(NumVisitQuestionAnswered());
            
            yield return new WaitForSeconds(20.0f);

            inputNumField.GetComponentInChildren<TextMeshProUGUI>().text = "";
            
        }

        yield return null;
    }

    private IEnumerator FadeInNumVisitsKeyboard()
    {
        StartCoroutine(CanvasGroupFadingOut(_numVisitsQRound1));
        yield return new WaitForSeconds(_fadingOutDuration);
        _numVisitsQRound1.SetActive(false);

        _numVisitsQRound2.GetComponent<CanvasGroup>().alpha = 0.0f;
        _numVisitsQRound2.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_numVisitsQRound2));
        yield return null;
    }

    private IEnumerator NumVisitQuestionAnswered()
    {
        StartCoroutine(CanvasGroupFadingOut(_numVisitsQuestion));
        yield return new WaitForSeconds(_fadingOutDuration);
        _numVisitsQuestion.SetActive(false);

        _relationQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
        _relationQuestion.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_relationQuestion));
        yield return null;

        /*
        _ageQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
        _ageQuestion.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_ageQuestion));
        yield return null; */
    }

    #endregion

    #region AgeQuestion

    public void AgeNumPad(TextMeshProUGUI keyInput)
    {
        _ageInput.GetComponentInChildren<TextMeshProUGUI>().text += keyInput.text;
    }

    public void AgeEnter(GameObject numAgeInput)
    {
        // save the data !!! - needs to be checked - used to be uncommented - should it be reverted back/is it useful?!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // ColorBlock cb = numAgeInput.GetComponent<Button>().colors;
        //
        // numAgeInput.GetComponent<Button>().colors = cb;

        StartCoroutine(CheckTextInputAgeQ(numAgeInput));
    }
    
    private IEnumerator CheckTextInputAgeQ(GameObject inputNumField)
    {
        if (inputNumField.GetComponentInChildren<TextMeshProUGUI>().text != "")
        {
            ColorBlock cb = inputNumField.GetComponent<Button>().colors;
            
            inputNumField.GetComponent<Button>().colors = cb;

            numberOfVisits = int.Parse(inputNumField.GetComponentInChildren<TextMeshProUGUI>().text);
            StartCoroutine(AgeQuestionAnswered());

            yield return new WaitForSeconds(20.0f);

            inputNumField.GetComponentInChildren<TextMeshProUGUI>().text = "";


        }

        yield return null;
    }

    private IEnumerator AgeQuestionAnswered()
    {

        StartCoroutine(CanvasGroupFadingOut(_ageQuestion));
        yield return new WaitForSeconds(_fadingOutDuration);
        _ageQuestion.SetActive(false);
        
        // fade in next question
        _genderQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
        _genderQuestion.SetActive(true);

        StartCoroutine(CanvasGroupFadingIn(_genderQuestion));
        yield return null;

    }

    #endregion

    #region GenderQuestion

    public void GenderInput(TextMeshProUGUI genderInput)
    {
        gender = genderInput.text;
        StartCoroutine(GenderQuestionAnswered());
        // trigger next part
        //StartCoroutine(SkipStuff());
    }

    // private IEnumerator SkipStuff()
    // {
    //     // StartCoroutine(CanvasGroupFadingOut(_genderQuestion));
    //     // yield return new WaitForSeconds(_fadingOutDuration);
    //     // _genderQuestion.SetActive(false);
    //     //
    //     // _eyeInformation.GetComponent<CanvasGroup>().alpha = 0.0f;
    //     // _eyeInformation.SetActive(true);
    //     // StartCoroutine(CanvasGroupFadingIn(_eyeInformation));
    // }

    private IEnumerator GenderQuestionAnswered()
    {
        StartCoroutine(CanvasGroupFadingOut(_genderQuestion));

        yield return new WaitForSeconds(_fadingOutDuration);

        _genderQuestion.SetActive(false);
        // start with emotion question
        rng = new Random();
        Shuffle(_emotionList);
        if (english)
        {
            _emotionQuestionText.GetComponent<TextMeshProUGUI>().text =
                "At the moment, how much are you " + _emotionList[0] + "?";
            
        }
        
        if (german)
        {
            _emotionQuestionText.GetComponent<TextMeshProUGUI>().text =
                "In diesem Moment, wie sehr sind Sie " + _emotionList[0] + "?";
        }
        
        _emotionQuestions.GetComponent<CanvasGroup>().alpha = 0.0f;
        _emotionQuestions.SetActive(true);
        
        // fade in text and move up table
        StartCoroutine(CanvasGroupFadingIn(_emotionQuestions));
        StartCoroutine(MoveTableUp(_emotionTable));
        yield return null;
    }

    #endregion

    #region EmotionQuestions

    public void EmotionEnter()
    {
        emotionNumAnswered += 1;
        Debug.Log("Emotion enter");
        Debug.Log(emotionNumAnswered);

        if (emotionNumAnswered <= 6)
        {
            Debug.Log("Start Fade in next emotion");
            StartCoroutine(FadeInNextEmotion());
        }
        else
        {
            Debug.Log("emotion questions finished");
            StartCoroutine(EmotionQuestionFinished());
        }
    }

    private IEnumerator FadeInNextEmotion()
    {
        // fade out answered question and select text for next question
        Debug.Log("fade out current emotion question");
        StartCoroutine(CanvasGroupFadingOut(_emotionQuestions));
        yield return new WaitForSeconds(_fadingOutDuration);
    
        if (english)
        {
            _emotionQuestionText.GetComponent<TextMeshProUGUI>().text =
                "At the moment, how much are you " + _emotionList[emotionNumAnswered] + "?";
            
        }

        if (german)
        {
            _emotionQuestionText.GetComponent<TextMeshProUGUI>().text =
                "In diesem Moment, wie sehr sind Sie " + _emotionList[emotionNumAnswered] + "?";
        }

        Debug.Log(_emotionNext.gameObject);

        _emotionNext.GetComponent<TextMeshProUGUI>().color = Color.black;

        Debug.Log("color assigned");

        emotionRatingList[emotionNumAnswered - 1] = currentEmotionRating;
        
        // fade back in and reset handle
        Debug.Log("Fade in next emotion question");
        StartCoroutine(CanvasGroupFadingIn(_emotionQuestions));
        
        Debug.Log("Reset handle");
        StartCoroutine(ResetTableHandle(_emotionHandle, _emotionHandleOrigin.GetComponent<Transform>().position.x));
        
        yield return null;
    }

    private IEnumerator EmotionQuestionFinished()
    {

        StartCoroutine(CanvasGroupFadingOut(_emotionQuestions));

        StartCoroutine(MoveTableDown(_emotionTable));

        if (_fadingOutDuration < _movingTableDuration)
        {
            yield return new WaitForSeconds(_movingTableDuration);
        }
        else
        {
            yield return new WaitForSeconds(_fadingOutDuration);
        }
        
        // reset handle position
        _emotionHandle.GetComponent<Transform>().position = _emotionHandleOrigin.GetComponent<Transform>().position;

        StartCoroutine(CanvasGroupFadingIn(_relationQuestion));
        
        yield return null;
    }

    #endregion

    #region RelationQuestion

    public void RelationInput(TextMeshProUGUI relationInput)
    {
        relation = relationInput.text;
        StartCoroutine(RelationQuestionAnswered());
        Debug.Log("Relation input - after start coroutine");
    }

    private IEnumerator RelationQuestionAnswered()
    {
        Debug.Log("Face out relation question");
        StartCoroutine(CanvasGroupFadingOut(_relationQuestion));
        yield return new WaitForSeconds(_fadingOutDuration);
        _relationQuestion.SetActive(false);
        
        //trigger eye calibration
        _eyeInformation.GetComponent<CanvasGroup>().alpha = 0.0f;
        _eyeInformation.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_eyeInformation));


        yield return null;

        // trigger connection question
        /*
        _connectionQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
        _connectionQuestion.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_connectionQuestion));
        StartCoroutine(MoveTableUp(_connectionTable)); */


        yield return null;
    }
    

    #endregion

    #region ConnectionQuestion

    public void ConnectionEnter()
    {
        // save answer....
        
        StartCoroutine(ConnectionQuestionAnswered());
    }

    private IEnumerator ConnectionQuestionAnswered()
    {
        StartCoroutine(CanvasGroupFadingOut(_connectionQuestion));
        StartCoroutine(MoveTableDown(_connectionTable));

        if (_fadingOutDuration < _movingTableDuration)
        {
            yield return new WaitForSeconds(_movingTableDuration);
        }
        else
        {
            yield return new WaitForSeconds(_fadingOutDuration);
        }
        
        _connectionQuestion.SetActive(false);
        
        // trigger next part
        _eyeInformation.GetComponent<CanvasGroup>().alpha = 0.0f;
        _eyeInformation.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_eyeInformation));
        
        
        yield return null;
    }

    #endregion
 
    #region EyeCallibrationInformation

    public void CalibrationInfoFinished()
    {
        StartCoroutine(FadeOutEyeInformation());
    }


    private IEnumerator FadeOutEyeInformation()
    {
        StartCoroutine(CanvasGroupFadingOut(_eyeInformation));
        yield return new WaitForSeconds(_fadingOutDuration);
        _eyeInformation.SetActive(false);

        EyeTrackingManager.Instance.StartCalibration();

        // trigger training
        _trainingInformation1.GetComponent<CanvasGroup>().alpha = 0.0f;
        _trainingInformation1.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_trainingInformation1));
        
        yield return null;



    }

    #endregion

    #region StartTrainingAfterAvatarSelection

    public void AvatarSelectionNext()
    {
        StartCoroutine(FadeOutAvatarSelection());

    }

    private IEnumerator FadeOutAvatarSelection()
    {
        StartCoroutine(CanvasGroupFadingOut(_avatarSelection));
        yield return new WaitForSeconds(_fadingOutDuration);
        _avatarSelection.SetActive(false);
        mirrorCamera.SetActive(false);
        
        // trigger training
        _trainingInformation1.GetComponent<CanvasGroup>().alpha = 0.0f;
        _trainingInformation1.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_trainingInformation1));
        
        yield return null;
    }
    #endregion
   
    #region Training

    public void TrainingPart1Selection()
    {
        StartCoroutine(FadeOutPart1Training());
    }

    private IEnumerator FadeOutPart1Training()
    {
        StartCoroutine(CanvasGroupFadingOut(_trainingInformation1));
        yield return new WaitForSeconds(_fadingOutDuration);
        _trainingInformation1.SetActive(false);

        // trigger training
        _trainingInformation2.GetComponent<CanvasGroup>().alpha = 0.0f;
        _trainingInformation2.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_trainingInformation2));
        
        //prevent teleportation
        teleport.isTeleportAllowed = false;
        Debug.Log("isTeleportAllowed status:" + teleport.isTeleportAllowed);
        
        yield return null;
    }

    public void TrainingPart2Selection()
    {
        StartCoroutine(FadeOutPart2Training());
    }

    private IEnumerator FadeOutPart2Training()
    {
        StartCoroutine(CanvasGroupFadingOut(_trainingInformation2));
        yield return new WaitForSeconds(_fadingOutDuration);
        _trainingInformation2.SetActive(false);
        
        //turn the teleportation back on
        teleport.isTeleportAllowed = true;

        // trigger training next part
        _trainingInformation3.GetComponent<CanvasGroup>().alpha = 0.0f;
        _trainingInformation3.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_trainingInformation3));
        
        //trigger wrong direction signs
        _wrongDirSign1.GetComponent<CanvasGroup>().alpha = 0.0f;
        _wrongDirSign1.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_wrongDirSign1));
        _wrongDirSign2.GetComponent<CanvasGroup>().alpha = 0.0f;
        _wrongDirSign2.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_wrongDirSign2));
        
        StartCoroutine(ActivateHitPoints());
        yield return null;
    }

    private IEnumerator FadeOutPart3Training()
    {
        StartCoroutine(CanvasGroupFadingOut(_trainingInformation3));
        StartCoroutine(CanvasGroupFadingOut(_wrongDirSign1));
        yield return new WaitForSeconds(_fadingOutDuration);
        _trainingInformation3.SetActive(false);
        _wrongDirSign1.SetActive(false);

        // trigger training next part
        _trainingInformation4.GetComponent<CanvasGroup>().alpha = 0.0f;
        _trainingInformation4.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_trainingInformation4));
        
        yield return null;
    }

    private IEnumerator ActivateHitPoints()
    {
        //instantiate the hit point teleport area
        trainingManager.hitpoint1.SetActive(true);
        Debug.Log("Checkpoint1 activated in QustionManager");
        yield return null;
    }

    public IEnumerator CheckPointsDone()
    {
        StartCoroutine(FadeOutPart3Training());
        yield return null;
    }
    
    private IEnumerator FadeOutPart4Training()
    {
        StartCoroutine(CanvasGroupFadingOut(_trainingInformation4));
        yield return new WaitForSeconds(_fadingOutDuration);
        _trainingInformation4.SetActive(false);
        
        yield return null;
    }

    public IEnumerator EnterExhibition()
    {
        StartCoroutine(FadeOutPart4Training());
        StartCoroutine(TriggerExhibition());
        yield return null;
    }

    public void TrainingPart4Selection()
    {
        StartCoroutine(EnterExhibition());
    }

    private IEnumerator TriggerExhibition()
    {
        // trigger exhibition
        _startExhibition.GetComponent<CanvasGroup>().alpha = 0.0f;
        _startExhibition.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_startExhibition));
        yield return null;
    }


    #endregion
    #endregion

    public void Return2DefaultNexPlayer()
    { 
        german = false; 
        english = false;
        
        inWheelchair = false; 
        smallBodySize = false; 
        noHeightIssue = false; 
        
        skipConsentCheck = false;

        emotionNumAnswered = 0;
        
        currentEmotionRating = 0.0f; 
        emotionRatingList = new float[]{0.0f,0.0f,0.0f,0.0f,0.0f,0.0f};
        
        connectionRating = 0.0f;

    }
}
