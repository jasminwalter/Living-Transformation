using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;

public class QuestionsManager : MonoBehaviour
{
    // Question Overview Game Objects
    public GameObject languageSelection;
    public GameObject englishUI;
    public GameObject germanUI;
    public GameObject consentCheck;
    public GameObject consentInputField;
    
    
    
    
    
    public bool german = false;
    public bool english = false;
    
    public bool languageQuestionAnswered = false;
    public bool consentCheckAnswered = false;
    
    public bool inPrepRoom = true;
    private bool _fading = false;
    public float _tFading = 0.0f;

    private void OnEnable()
    {
        languageSelection.SetActive(true);
    }


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
                    
                    languageQuestionAnswered = false;
                    consentCheck.SetActive(true);
                    }
            }
        }

        if (consentCheckAnswered)
        {
            
            if (_fading)
            {
                if (_tFading < 1.0f)
                {

                    CanvasFading(consentCheck);

                    // deactivate question and activate next one
                }
                else
                {
                    consentCheck.SetActive(false);
                    _fading = false;
                    _tFading = 0.0f;
                    
                    consentCheckAnswered = false;
                    
                    // trigger next question
                }
            }
            
            
            
        }
    }

    void CanvasFading(GameObject canvasGroup)
    {

        canvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, _tFading);
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
    
    public void KeyboardInput(GameObject InputText)
    {
        consentInputField.GetComponentInChildren<Text>().text += InputText.GetComponent<Text>().text;

    }

    public void KeyboardDelete()
    {
        string inputText = consentInputField.GetComponentInChildren<Text>().text;
        if (inputText.Length > 0)
        {
            consentInputField.GetComponentInChildren<Text>().text = inputText.Remove(inputText.Length - 1);
        }
    }

    public void KeyboardNext(GameObject nextButton)
    {
        Debug.Log("Next Button pressed");
        // save the data
        ColorBlock cb = nextButton.GetComponent<Button>().colors;
        cb.normalColor = Color.green;
        Debug.Log(cb);

        consentInputField.GetComponent<Button>().colors = cb;
        
        consentCheckAnswered = true;
        _fading = true;
        
    }
    
    

    #endregion
    
}
