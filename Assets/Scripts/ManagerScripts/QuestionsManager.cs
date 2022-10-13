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

    public GameObject trainingInformationE;

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

    public GameObject trainingInformationG;
    
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

    private GameObject _trainingInformation;
    #endregion
    
    #endregion
    
    // other public variables
    [Header("Saving variables")]
    
    public bool german = false;
    public bool english = false;
    
    
    public float cameraHeight;
    public bool inWheelchair = false;
    public bool smallBodySize = false;
    public bool noHeightIssue = false;

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
    
    public float heightThreshold = 1.4f;
    
    public bool welcomeNextTrigger = false;
    public float welcomeNextThreshold = 5.0f;
    public float welcomeTextExitThreshold = 120.0f;
    
    private static Random rng;
    public float targetHeight = 50.0f;

    
    public float fadingOutDuration = 2.0f;
    public float fadingInDuration = 2.0f;
    public float movingTableDuration = 2.0f;
    public float handleResetDuration = 0.5f;


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
    
    private IEnumerator CanvasGroupFadingOut(GameObject canvasGroup)
    {
        float timer = 0.0f;
        while (timer <= fadingOutDuration)
        {
            canvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, timer / fadingOutDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.GetComponent<CanvasGroup>().alpha = 0;
        yield return null;
    }
    
    private IEnumerator CanvasGroupFadingIn(GameObject canvasGroup)
    {
        float timer = 0.0f;
        while (timer <= fadingInDuration)
        {
            canvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, timer / fadingInDuration);

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
        while (timer <= movingTableDuration)
        {
            table.GetComponent<Transform>().position = new Vector3(x,Mathf.Lerp(-51.7f, targetHeight, timer/movingTableDuration),z);
        }
        
        table.GetComponent<Transform>().position = new Vector3(x, targetHeight, z);
        yield return null;
    }

    private IEnumerator MoveTableDown(GameObject table)
    {
        float x = table.GetComponent<Transform>().position.x;
        float y = table.GetComponent<Transform>().position.y;
        float z = table.GetComponent<Transform>().position.z;
        
        float timer = 0.0f;
        while (timer <= movingTableDuration)
        {
            table.GetComponent<Transform>().position = new Vector3(x, Mathf.Lerp(y, -51.7f, timer/movingTableDuration), z);
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
        while (timer <= handleResetDuration)
        {
            handle.GetComponent<Transform>().position =
                new Vector3(Mathf.Lerp(currentX, destinationPosition, timer/handleResetDuration), y, z);
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
        yield return new WaitForSeconds(fadingOutDuration);
        
        languageSelection.SetActive(false);
        
        cameraHeight = cameraVR.GetComponent<Transform>().position.y +50.0f;
        
        
        if (cameraHeight < heightThreshold)
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

            _trainingInformation = trainingInformationE;
                        
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

            _trainingInformation = trainingInformationG;
        }
        
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
        StartCoroutine(HeightQuestionAnswered());
    }

    private IEnumerator HeightQuestionAnswered()
    {

        StartCoroutine(CanvasGroupFadingOut(_heightQuestion));
        yield return new WaitForSeconds(fadingOutDuration);
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
        yield return new WaitForSeconds(fadingInDuration);
        
        // start audio read out

        yield return new WaitForSeconds(welcomeNextThreshold);
        
        // fade in next button
        _welcomeNext.GetComponent<CanvasGroup>().alpha = 0.0f;
        _welcomeNext.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_welcomeNext));
        yield return new WaitForSeconds(fadingInDuration);
        
        // wait for the rest of the threshold time before transitioning to next question, if not already done
        yield return new WaitForSeconds(welcomeTextExitThreshold);
        if (!welcomeNextTrigger)
        {
            StartCoroutine(WelcomeTextFadeOut());
        }
        yield return null;
    }
    public void WelcomeNext()
    {
        welcomeNextTrigger = true;
        StartCoroutine(WelcomeTextFadeOut());

    }

    private IEnumerator WelcomeTextFadeOut()
    {
        StartCoroutine(CanvasGroupFadingOut(_welcomeText));
        yield return new WaitForSeconds(fadingOutDuration);
        _welcomeText.SetActive(false);

        _consentCheck.GetComponent<CanvasGroup>().alpha = 0.0f;
        _consentCheck.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_consentCheck));
        yield return null;
    }

    #endregion

    #region ConsentCheckKeyboard
    
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
        yield return new WaitForSeconds(fadingOutDuration);
        _consentCheck.SetActive(false);

        _numVisitsQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
        _numVisitsQuestion.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_numVisitsQuestion));
        yield return null;
    }
    
    #endregion
    
    #region NumVisits

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
        // save the data
        ColorBlock cb = inputNumField.GetComponent<Button>().colors;

        inputNumField.GetComponent<Button>().colors = cb;

        numberOfVisits = int.Parse(inputNumField.GetComponentInChildren<TextMeshProUGUI>().text);
        StartCoroutine(NumVisitQuestionAnswered());
    }

    private IEnumerator FadeInNumVisitsKeyboard()
    {
        StartCoroutine(CanvasGroupFadingOut(_numVisitsQRound1));
        yield return new WaitForSeconds(fadingOutDuration);
        _numVisitsQRound1.SetActive(false);

        _numVisitsQRound2.GetComponent<CanvasGroup>().alpha = 0.0f;
        _numVisitsQRound2.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_numVisitsQRound2));
        yield return null;
    }

    private IEnumerator NumVisitQuestionAnswered()
    {
        StartCoroutine(CanvasGroupFadingOut(_numVisitsQuestion));
        yield return new WaitForSeconds(fadingOutDuration);
        _numVisitsQuestion.SetActive(false);
        
        _ageQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
        _ageQuestion.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_ageQuestion));
        yield return null;
    }

    #endregion

    #region AgeQuestion

    public void AgeNumPad(TextMeshProUGUI keyInput)
    {
        _ageInput.GetComponentInChildren<TextMeshProUGUI>().text += keyInput.text;
    }

    public void AgeEnter(GameObject numAgeInput)
    {
        // save the data
        ColorBlock cb = numAgeInput.GetComponent<Button>().colors;

        numAgeInput.GetComponent<Button>().colors = cb;

        age = int.Parse(numAgeInput.GetComponentInChildren<TextMeshProUGUI>().text);
        StartCoroutine(AgeQuestionAnswered());
    }

    private IEnumerator AgeQuestionAnswered()
    {

        StartCoroutine(CanvasGroupFadingOut(_ageQuestion));
        yield return new WaitForSeconds(fadingOutDuration);
        _ageQuestion.SetActive(false);
        
        // fade in next question
        _genderQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
        _genderQuestion.SetActive(true);

        StartCoroutine(CanvasGroupFadingIn(_genderQuestion));
    }

    #endregion

    #region GenderQuestion

    public void GenderInput(TextMeshProUGUI genderInput)
    {
        gender = genderInput.text;
    }

    private IEnumerator GenderQuestionAnswered()
    {
        StartCoroutine(CanvasGroupFadingOut(_genderQuestion));
        yield return new WaitForSeconds(fadingOutDuration);
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

        if (emotionNumAnswered >= 6)
        {
            StartCoroutine(FadeInNextEmotion());
        }
        else
        {
            EmotionQuestionFinished();
        }
    }

    private IEnumerator FadeInNextEmotion()
    {
        // fade out answered question and select text for next question
        StartCoroutine(CanvasGroupFadingOut(_emotionQuestions));
        yield return new WaitForSeconds(fadingOutDuration);

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

        _emotionNext.GetComponent<TextMeshProUGUI>().color = Color.black;

        emotionRatingList[emotionNumAnswered - 1] = currentEmotionRating;

        // fade back in and reset handle
        StartCoroutine(CanvasGroupFadingIn(_emotionQuestions));
        StartCoroutine(ResetTableHandle(_emotionHandle, _emotionHandleOrigin.GetComponent<Transform>().position.x));
        
        yield return null;
    }

    private IEnumerator EmotionQuestionFinished()
    {

        StartCoroutine(CanvasGroupFadingOut(_emotionQuestions));

        StartCoroutine(MoveTableDown(_emotionTable));

        if (fadingOutDuration < movingTableDuration)
        {
            yield return new WaitForSeconds(movingTableDuration);
        }
        else
        {
            yield return new WaitForSeconds(fadingOutDuration);
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
    }

    private IEnumerator RelationQuestionAnswered()
    {
        StartCoroutine(CanvasGroupFadingOut(_relationQuestion));
        yield return new WaitForSeconds(fadingOutDuration);
        _relationQuestion.SetActive(false);
        
        // trigger connection question
        _connectionQuestion.GetComponent<CanvasGroup>().alpha = 0.0f;
        _connectionQuestion.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_connectionQuestion));
        StartCoroutine(MoveTableUp(_connectionTable));
        

        yield return null;
    }
    

    #endregion

    #region ConnectionQuestion

    public void ConnectionEnter()
    {
        StartCoroutine(ConnectionQuestionAnswered());
    }

    private IEnumerator ConnectionQuestionAnswered()
    {
        StartCoroutine(CanvasGroupFadingOut(_connectionQuestion));
        StartCoroutine(MoveTableDown(_connectionTable));

        if (fadingOutDuration < movingTableDuration)
        {
            yield return new WaitForSeconds(movingTableDuration);
        }
        else
        {
            yield return new WaitForSeconds(fadingOutDuration);
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
        yield return new WaitForSeconds(fadingOutDuration);
        _eyeInformation.SetActive(false);
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
        yield return new WaitForSeconds(fadingOutDuration);
        _avatarSelection.SetActive(false);
        
        // trigger training
        _trainingInformation.GetComponent<CanvasGroup>().alpha = 0.0f;
        _trainingInformation.SetActive(true);
        StartCoroutine(CanvasGroupFadingIn(_trainingInformation));
        
    }

    public void StartTrainingButton()
    {

    }

    #endregion
    #endregion

}
