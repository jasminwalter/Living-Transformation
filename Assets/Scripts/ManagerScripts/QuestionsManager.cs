using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;

public class QuestionsManager : MonoBehaviour
{
    public GameObject languageSelection;
    public bool german = false;
    public bool english = false;
    public bool languageQuestionAnswered = false;
    public bool inPrepRoom = true;
    private bool _fading = false;
    public float _tFading = 0.0f;
    
    // Update is called once per frame
    void Update()
    {
        
        if (languageQuestionAnswered)
        {
            // save answer
            //languageSelection.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, 0.1f);
            // deactivate question and activate next one

            //languageSelection.SetActive(false);
            if (_fading)
            {
                if (_tFading < 1.0f)
                {

                    CanvasFading(languageSelection);

                    // deactivate question and activate next one
                }
                else
                {
                    languageSelection.SetActive(false);
                    _fading = false;
                    _tFading = 0.0f;
                    
                }
            }
        }
    }

    void CanvasFading(GameObject canvasGroup)
    {

        canvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, _tFading);
        Debug.Log(canvasGroup.GetComponent<CanvasGroup>().alpha );
        _tFading += 0.6f*Time.deltaTime;


    }
    
    #region ButtonResponses
    #region LanguageSelection
    public void SelectionEnglish()
    {
        Debug.Log("Selection English");
        english = true;
        languageQuestionAnswered = true;
        _fading = true;

    }

    public void SelectionGerman()
    {
        Debug.Log("Selection German");
        german = true;
        languageQuestionAnswered = true;
        _fading = true;

    }

    public void SelectionText(GameObject buttonText)
    {
        buttonText.SetActive(true);
    }
    #endregion
    
    
    

    #endregion
    
}
