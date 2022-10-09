using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionManager : MonoBehaviour
{

    public static ExhibitionManager Instance { get; private set; }
    
    [Header("UI Elements")]
    public GameObject exhibitionStart;

    public GameObject exhibitionInfo1;

    public GameObject exhibitionInfo2;

    public GameObject exitExhibition;

    public float displayTime = 5.0f;
    
    
    [Header("visual transition variables")]

    public FadingCamera fadingCamera;
    public GameObject preparationRoom;
    public float fadeDuration = 2.0f;

    [Header("Exhibition elements")]
    public GameObject exhibition;

    public GameObject playerLocal;
    public GameObject playerRemote;

    public GameObject localGazeSphere;
    public GameObject remoteGazeSphere;
    
    public GameObject startServer;
    public GameObject start2ndPlayer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator fadeOutUI(GameObject uiElement)
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

    IEnumerator fadeInUI(GameObject uiElement)
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

    IEnumerator EnterExhibitionRoutine()
    {
        // display the other instructions
        StartCoroutine(fadeOutUI(exhibitionStart));
        yield return new WaitForSeconds(fadeDuration);
        
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
        
        // position player in exhibition
        playerLocal.GetComponent<Transform>().position = startServer.GetComponent<Transform>().position;


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

    IEnumerator ExitExhibitionRoutine()
    {
        
        
        yield return null;
    }
    

    #endregion

    

    



}
