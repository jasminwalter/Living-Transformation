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
    
    // Update is called once per frame
    void Update()
    {
        
        if (languageQuestionAnswered)
        {
            // save answer
            languageSelection.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, 1.1f*Time.deltaTime);
            // deactivate question and activate next one

            //languageSelection.SetActive(false);
        }
    }
    #region ButtonResponses
    #region LanguageSelection
    public void SelectionEnglish()
    {
        // save answer
        Debug.Log("Selection English");
        english = true;
        languageQuestionAnswered = true;
        // fade in or fade out of 

    }

    public void SelectionGerman()
    {
        Debug.Log("Selection German");
        german = true;
        languageQuestionAnswered = true;
        
    }
    #endregion
    
    
    

    #endregion
    
}
