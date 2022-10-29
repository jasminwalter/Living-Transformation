using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionManager : MonoBehaviour
{

    public static ExhibitionManager Instance { get; private set; }
    public NetworkManager NetMan;
    
    [Header("UI Elements")]
    private GameObject exhibitionStart;

    private GameObject exhibitionInfo1;

    private GameObject exhibitionInfo2;
    
    // english
    public GameObject exhibitionStartE;

    public GameObject exhibitionInfo1E;

    public GameObject exhibitionInfo2E;
    
    // german
    public GameObject exhibitionStartG;

    public GameObject exhibitionInfo1G;

    public GameObject exhibitionInfo2G;

    public float displayTime = 5.0f;
    
    // end UI
    [Header("language start")] public GameObject languageSelection;

    public GameObject languageSelectUIStartExhibition;
    
    [Header("visual transition variables")]

    public FadingCamera fadingCamera;
    public GameObject preparationRoom;
    public float fadeDuration = 2.0f;

    [Header("Exhibition elements")]
    public GameObject exhibition;

    public GameObject exitUIEnglish;
    public GameObject exitUIGerman;

    [Header("Player Information")]
    public GameObject playerLocal;
    public GameObject localRightHand;
    public GameObject localLeftHand;
    public GameObject avatarLocal;
    public Vector3 avatarLocalFootOffset = new Vector3(0,0,0);
    
    public GameObject playerRemote;
    public GameObject avatarRemote;

    public GameObject localGazeSphere;
    public GameObject remoteGazeSphere;
    
    public GameObject startServer;
    public GameObject start2ndPlayer;

    public GameObject locationInPrepRoom;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AssignLangugageUI()
    {
        if (QuestionsManager.Instance.english)
        {
            exhibitionStart = exhibitionStartE;
            exhibitionInfo1 = exhibitionInfo1E;
            exhibitionInfo2 = exhibitionInfo2E;
        }

        if (QuestionsManager.Instance.german)
        {
            exhibitionStart = exhibitionStartG;
            exhibitionInfo1 = exhibitionInfo1G;
            exhibitionInfo2 = exhibitionInfo2G;
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
        StartCoroutine(fadeOutUI(exhibitionStart));
        yield return new WaitForSeconds(fadeDuration);
        
        // select UI based on language selection
        AssignLangugageUI();
        
        exhibitionStart.SetActive(false);
        exhibitionInfo1.GetComponent<CanvasGroup>().alpha = 0;
        exhibitionInfo1.SetActive(true);
        
        StartCoroutine(fadeInUI(exhibitionInfo1));
        yield return new WaitForSeconds(fadeDuration);
        
        // now wait for instructions to be displayed

        yield return new WaitForSeconds(displayTime);
        
        // fade out info and fade in next
        StartCoroutine(fadeOutUI(exhibitionInfo1));
        yield return new WaitForSeconds(fadeDuration);
        exhibitionInfo1.SetActive(false);
        
        exhibitionInfo2.GetComponent<CanvasGroup>().alpha = 0;
        exhibitionInfo2.SetActive(true);
        StartCoroutine(fadeInUI(exhibitionInfo2));
        yield return new WaitForSeconds(fadeDuration);
        
        // wait for display of instructions
        yield return new WaitForSeconds(displayTime);

        // fade out
        fadingCamera.FadeOut();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);
        
        // switch rooms
        exhibitionInfo2.SetActive(false);
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
        if (NetMan.IsServer())
        {
            playerLocal.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
            avatarLocal.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
            
            playerRemote.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
            avatarRemote.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;

        }
        else
        {
            playerLocal.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
            avatarLocal.GetComponent<Transform>().position = start2ndPlayer.GetComponent<Transform>().position;
            
            playerRemote.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
            avatarRemote.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;
        }

        avatarLocal.GetComponent<VRFootIK>().footOffset = avatarLocalFootOffset;
        
        ExperimentManager.Instance().inExhibitionArea = true;
        localGazeSphere.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        remoteGazeSphere.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        
        // fade in
        fadingCamera.FadeIn();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);
        
        yield return null;
    }

    #endregion

    #region ExitingExhibition

    public void ExitExhibition()
    {

        StartCoroutine(ExitExhibitionRoutine());
    }

    private IEnumerator ExitExhibitionRoutine()
    {
        
        fadingCamera.FadeOut();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);
        
        // set all exhibition areas inactive
        exhibition.SetActive(false);
        ExperimentManager.Instance().inExhibitionArea = false;
        EyeTrackingManager.Instance.showGazeSphere = false;

        // set avatar and player to correct location
        avatarLocal.GetComponent<Transform>().position = locationInPrepRoom.GetComponent<Transform>().position;
        playerLocal.GetComponent<Transform>().position = locationInPrepRoom.GetComponent<Transform>().position;

        // disable the local avatar and gaze sphere
        avatarLocal.SetActive(false);
        localGazeSphere.SetActive(false);
        
        // set all aspects in Experiment Room to true and correct values
        preparationRoom.SetActive(true);
        
        // set active the normal VR Hands again------------------------------------
        
        
        // set active - final question part 1

        languageSelectUIStartExhibition.SetAcive(true);
        
        
        // fade in
        fadingCamera.FadeIn();
        yield return new WaitForSeconds(fadingCamera.fadeDuration);

        
        yield return null;
    }
    

    #endregion

    

    



}
