using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionManager : MonoBehaviour
{

    public static ExhibitionManager Instance { get; private set; }
    
    [Header("UI Elements")]
    private GameObject _exhibitionStart;

    private GameObject _exhibitionInfo1;

    private GameObject _exhibitionInfo2;
    
    // english
    public GameObject exhibitionStartE;

    public GameObject exhibitionInfo1E;

    public GameObject exhibitionInfo2E;
    
    // german
    public GameObject exhibitionStartG;

    public GameObject exhibitionInfo1G;

    public GameObject exhibitionInfo2G;

    public float displayTime = 7.0f;
    
    // end UI
    [Header("language start")]
    public GameObject languageSelectUIStartExhibition;
    
    [Header("visual transition variables")]

    public FadingCamera fadingCamera;
    public GameObject preparationRoom;
    public float fadeDuration = 2.0f;

    [Header("Exhibition elements")]
    public GameObject exhibition;

    public GameObject exitUIEnglish;
    public GameObject exitUIGerman;

    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    [Header("Player Information")]
    public GameObject playerLocal;
    public GameObject localRightHand;
    public GameObject localLeftHand;
    public GameObject avatarLocal;
    // private Vector3 avatarLocalFootOffsetEx = new Vector3(0,0,0.11);
    // private Vector3 avatarLocalFootOffsetPrep = new Vector3(0,0,0.11);
    
    public GameObject playerRemote;
    public GameObject avatarRemote;

    public GameObject localGazeSphere;
    public GameObject remoteGazeSphere;
    
    public GameObject startServer;
    public GameObject start2ndPlayer;

    public GameObject locationInPrepRoom;
    public GameObject WaitingForPartner;

    private int transfer2Exhibition = 0;
    private int transfer2PrepRoom = 0;

    // Start is called before the first frame update
    private void Start()
    {
        
        // Ensure that this instance is the only one and is accessible globally
        if (Instance == null)
        {
            Instance = this;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (ExperimentManager.Instance().remoteEnterExhibition & ExperimentManager.Instance().localEnterExhibition)
        // {
        //     ExperimentManager.Instance().localLanguageSelected = true;
        // }
        //
        // if (ExperimentManager.Instance().remoteExhibitionExit & ExperimentManager.Instance().localExhibitionExit)
        // {
        //     ExperimentManager.Instance().localStartNewVisitor = true;
        // }
        //
        // if (ExperimentManager.Instance().remoteLanugageSelected)
        // {
        //     if (transfer2Exhibition < 1)
        //     {
        //         StartCoroutine(EnterExhibitionPart2());
        //         transfer2Exhibition++;
        //     }
        //     
        // }
        //
        // if (ExperimentManager.Instance().remoteStartNewVisitor = true)
        // {
        //     if (transfer2PrepRoom < 1)
        //     {
        //         StartCoroutine(ExitExhibitionPart2());
        //         transfer2PrepRoom++;
        //     }
        //     
        // }
    }

    private void AssignLangugageUI()
    {
        if (QuestionsManager.Instance.english)
        {
            _exhibitionStart = exhibitionStartE;
            _exhibitionInfo1 = exhibitionInfo1E;
            _exhibitionInfo2 = exhibitionInfo2E;
        }

        if (QuestionsManager.Instance.german)
        {
            _exhibitionStart = exhibitionStartG;
            _exhibitionInfo1 = exhibitionInfo1G;
            _exhibitionInfo2 = exhibitionInfo2G;
        }
    }

    private IEnumerator fadeOutUI(GameObject uiElement)
    {
     
        float timer = 0.0f;
        while (timer <= fadeDuration)
        {
            uiElement.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, timer / fadeDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        uiElement.GetComponent<CanvasGroup>().alpha = 0;
        yield return null;
    }

    private IEnumerator fadeInUI(GameObject uiElement)
    {

        float timer = 0.0f;
        while (timer <= fadeDuration)
        {
            uiElement.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, timer / fadeDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        uiElement.GetComponent<CanvasGroup>().alpha = 1;
        yield return null;
        
    }

    #region EnteringExhibition

    public void EnterExhibition()
    {
        StartCoroutine(EnterExhibitionRoutine());
    }

    private IEnumerator EnterExhibitionRoutine()
    {
        // display the other instructions
        
        StartCoroutine(fadeOutUI(_exhibitionStart));
        yield return new WaitForSeconds(fadeDuration);

        // select UI based on language selection
        AssignLangugageUI();

        _exhibitionStart.SetActive(false);
        _exhibitionInfo1.GetComponent<CanvasGroup>().alpha = 0;
        _exhibitionInfo1.SetActive(true);

        StartCoroutine(fadeInUI(_exhibitionInfo1));
        yield return new WaitForSeconds(fadeDuration);

        // now wait for instructions to be displayed

        yield return new WaitForSeconds(displayTime);

        // fade out info and fade in next
        StartCoroutine(fadeOutUI(_exhibitionInfo1));
        yield return new WaitForSeconds(fadeDuration);
        _exhibitionInfo1.SetActive(false);

        _exhibitionInfo2.GetComponent<CanvasGroup>().alpha = 0;
        _exhibitionInfo2.SetActive(true);
        StartCoroutine(fadeInUI(_exhibitionInfo2));
        yield return new WaitForSeconds(fadeDuration);

        // wait for display of instructions
        yield return new WaitForSeconds(displayTime);
        
        // StartCoroutine(fadeOutUI(_exhibitionInfo2));
        // yield return new WaitForSeconds(fadeDuration);

        // ExperimentManager.Instance().localEnterExhibition = true;
        // WaitingForPartner.SetActive(true);
        
        //-------------------------------------------------------------------------------
        // fade out
        fadingCamera.FadeOut();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);
        
        // switch rooms
        _exhibitionInfo2.SetActive(false);
        preparationRoom.SetActive(false);
        exhibition.SetActive(true);
        
        // activate language specific elements
        if (QuestionsManager.Instance.english)
        {
            exitUIEnglish.SetActive(true);
        }

        if (QuestionsManager.Instance.german)
        {
            exitUIGerman.SetActive(true);
        }
        
        // position player in exhibition
        
        if (ExperimentManager.Instance.isExperimentControl())
        {
            playerLocal.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
            //avatarLocal.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
            
            playerRemote.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
            //avatarRemote.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;

        }
        else
        {
            playerLocal.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
            //avatarLocal.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
            
            playerRemote.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
            //avatarRemote.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
        }
        
        

        // avatarLocal.GetComponent<VRFootIK>().footOffset = avatarLocalFootOffset;
        
        //ExperimentManager.Instance().inExhibitionArea = true;
        localGazeSphere.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        remoteGazeSphere.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        
        // fade in
        fadingCamera.FadeIn();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);
        
        yield return null;
    }
    
    // private IEnumerator EnterExhibitionPart2()
    // {
    //     WaitingForPartner.SetActive(false);
    //
    //     // fade out
    //     fadingCamera.FadeOut();
    //     yield return new WaitForSeconds(fadingCamera.fadeDuration);
    //     
    //     // switch rooms
    //     _exhibitionInfo2.SetActive(false);
    //     preparationRoom.SetActive(false);
    //     exhibition.SetActive(true);
    //     
    //     // activate language specific elements
    //     if (QuestionsManager.Instance.english)
    //     {
    //         exitUIEnglish.SetActive(true);
    //     }
    //
    //     if (QuestionsManager.Instance.german)
    //     {
    //         exitUIGerman.SetActive(true);
    //     }
    //     
    //     // position player in exhibition
    //     if (NetMan.IsServer())
    //     {
    //         playerLocal.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
    //         avatarLocal.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
    //         
    //         playerRemote.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
    //         avatarRemote.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
    //
    //     }
    //     else
    //     {
    //         playerLocal.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
    //         avatarLocal.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
    //         
    //         playerRemote.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
    //         avatarRemote.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
    //     }
    //
    //     // avatarLocal.GetComponent<VRFootIK>().footOffset = avatarLocalFootOffset;
    //     
    //     ExperimentManager.Instance().inExhibitionArea = true;
    //     localGazeSphere.GetComponent<Transform>().position = new Vector3(0, 0, 0);
    //     remoteGazeSphere.GetComponent<Transform>().position = new Vector3(0, 0, 0);
    //     
    //     // fade in
    //     fadingCamera.FadeIn();
    //     yield return new WaitForSeconds(fadingCamera.fadeDuration);
    //
    //     yield return new WaitForSeconds(60.0f);
    //     
    //     
    //     // create defaults for leaving exhibition
    //     ExperimentManager.Instance().localExhibitionExit = false;
    //     ExperimentManager.Instance().localStartNewVisitor = false;
    //     transfer2PrepRoom = 0;
    //     
    //     
    //     yield return null;
    //     
    // }

    #endregion

    #region ExitingExhibition

    // public void ExitExhibition()
    // {
    //
    //     StartCoroutine(ExitExhibitionRoutine());
    // }
    //
    // private IEnumerator ExitExhibitionRoutine()
    // {
    //     ExperimentManager.Instance().localExhibitionExit = true;
    //
    //     fadingCamera.FadeOut();
    //     yield return new WaitForSeconds(fadingCamera.fadeDuration);
    //     object1.GetComponent<ObjectTransitions>().Return2DefaultState4NewParticipant();
    //     object2.GetComponent<ObjectTransitions>().Return2DefaultState4NewParticipant();
    //     object3.GetComponent<ObjectTransitions>().Return2DefaultState4NewParticipant();
    //     QuestionsManager.Instance.Return2DefaultNexPlayer();
    //
    //     // set all exhibition areas inactive
    //     exhibition.SetActive(false);
    //     ExperimentManager.Instance().inExhibitionArea = false;
    //     // EyeTrackingManager.Instance.showGazeSphere = false;
    //
    //     // set avatar and player to correct location
    //     avatarLocal.GetComponent<Transform>().position = locationInPrepRoom.GetComponent<Transform>().position;
    //     playerLocal.GetComponent<Transform>().position = locationInPrepRoom.GetComponent<Transform>().position;
    //
    //     // disable the local avatar and gaze sphere
    //     // avatarLocal.SetActive(false);
    //     // localGazeSphere.SetActive(false);
    //
    //     // set all aspects in Experiment Room to true and correct values
    //     preparationRoom.SetActive(true);
    //
    //     // set active the normal VR Hands again------------------------------------
    //
    //
    //     
    //     WaitingForPartner.SetActive(true);
    //     
    //     fadingCamera.FadeIn();
    //     yield return new WaitForSeconds(fadingCamera.fadeDuration);
    // }
    //
    // private IEnumerator ExitExhibitionPart2()
    // {
    //     WaitingForPartner.SetActive(false);
    //
    // // set active - final question part 1
    //
    //     languageSelectUIStartExhibition.GetComponent<CanvasGroup>().alpha = 1;
    //     languageSelectUIStartExhibition.SetActive(true);
    //     
    //     
    //     // fade in
    //
    //     // create defaults for entering exhibition
    //     ExperimentManager.Instance().localEnterExhibition = false;
    //     ExperimentManager.Instance().localLanguageSelected = false;
    //     transfer2Exhibition = 0;
    //     
    //     yield return null;
    // }
    //
    public void ForceExhibitionExit()
    {
        
        // ExperimentManager.Instance().localExhibitionExit = true;
        
        object1.GetComponent<ObjectTransitions>().Return2DefaultState4NewParticipant();
        object2.GetComponent<ObjectTransitions>().Return2DefaultState4NewParticipant();
        object3.GetComponent<ObjectTransitions>().Return2DefaultState4NewParticipant();
        QuestionsManager.Instance.Return2DefaultNexPlayer();
    
        // set all exhibition areas inactive
        exhibition.SetActive(false);
        //ExperimentManager.Instance().inExhibitionArea = false;
        // EyeTrackingManager.Instance.showGazeSphere = false;
    
        // set avatar and player to correct location
        avatarLocal.GetComponent<Transform>().position = locationInPrepRoom.GetComponent<Transform>().position;
        playerLocal.GetComponent<Transform>().position = locationInPrepRoom.GetComponent<Transform>().position;
    
        // disable the local avatar and gaze sphere
        // avatarLocal.SetActive(false);
        // localGazeSphere.SetActive(false);
    
        // set all aspects in Experiment Room to true and correct values
        preparationRoom.SetActive(true);
        
        
        // set active - final question part 1
    
        languageSelectUIStartExhibition.GetComponent<CanvasGroup>().alpha = 1;
        languageSelectUIStartExhibition.SetActive(true);
        
        
        // fade in
    
        // create defaults for entering exhibition
        // ExperimentManager.Instance().localEnterExhibition = false;
        // ExperimentManager.Instance().localLanguageSelected = false;
        // transfer2Exhibition = 0;
        
    }
    

    #endregion
    

}
