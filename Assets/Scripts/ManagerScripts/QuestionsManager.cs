﻿using System;
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
    
    public GameObject numVisitsQuestion;
    public GameObject numVisitsQRound1;
    public GameObject numVisitsQRound2;
    public GameObject numVisitInput;

    public GameObject ageQuestion;
    public GameObject ageInput;






    public bool german = false;
    public bool english = false;
    
    public bool languageQuestionAnswered = false;
    public bool consentCheckAnswered = false;
    public bool numVisitsAnswered = false;
    public bool notFirstVisit = false;
    public int numberOfVisits;
    public bool ageQuestionAnswered = false;
    public int age;
    
    public bool inPrepRoom = true;
    private bool _fadingOut = false;
    private bool _fadingIn = false;
    private float _tFading = 0.0f;

    private void OnEnable()
    {
        languageSelection.SetActive(true);
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

            //languageSelection.SetActive(false);
            if (_fadingOut)
            {
                if (_tFading < 1.0f)
                {

                    CanvasFadingOut(languageSelection);

                    // deactivate question and activate next one
                }
                else
                {
                    languageSelection.SetActive(false);
                    _fadingOut = false;
                    _tFading = 0.0f;
                    
                    languageQuestionAnswered = false;
                    consentCheck.SetActive(true);
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
                
                    _fadingIn = true;
                }
                
            }
        }
    }

    #region FadingEffects

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
    
    #endregion
    
    #region ButtonResponses
    #region LanguageSelection
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

    public void SelectionText(GameObject buttonText)
    {
        buttonText.SetActive(true);
    }
    #endregion

    #region ConsentCheckKeyboard
    public void ConsentKeyboardInput(GameObject buttonText)
    {
        consentInputField.GetComponentInChildren<Text>().text += buttonText.GetComponent<Text>().text;

    }
    
    public void KeyboardDelete(GameObject inputField)
    {
        string inputText = inputField.GetComponentInChildren<Text>().text;
        if (inputText.Length > 0)
        {
            inputField.GetComponentInChildren<Text>().text = inputText.Remove(inputText.Length - 1);
        }
    }

    public void ConsentKeyboardNext(GameObject nextButton)
    {
        // save the data
        ColorBlock cb = nextButton.GetComponent<Button>().colors;
        cb.normalColor = Color.green;

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

    public void NumVisitButton(GameObject numButtons)
    {
        numVisitInput.GetComponentInChildren<Text>().text += numButtons.GetComponent<Text>().text;
        
    }

    public void EnterNumVisits(GameObject inputNumField)
    {
        // save the data
        ColorBlock cb = inputNumField.GetComponent<Button>().colors;
        cb.normalColor = Color.green;

        inputNumField.GetComponent<Button>().colors = cb;

        numberOfVisits = int.Parse(inputNumField.GetComponentInChildren<Text>().text);
        numVisitsAnswered = true;
        _fadingOut = true;
        
    }
    

    #endregion

    #region AgeQuestion

    public void AgeNumPad(GameObject numButtons)
    {
        ageInput.GetComponentInChildren<Text>().text += numButtons.GetComponent<Text>().text;
    }

    public void AgeEnter(GameObject numAgeInput)
    {
        // save the data
        ColorBlock cb = numAgeInput.GetComponent<Button>().colors;
        cb.normalColor = Color.green;

        numAgeInput.GetComponent<Button>().colors = cb;

        age = int.Parse(numAgeInput.GetComponentInChildren<Text>().text);
        ageQuestionAnswered = true;
        _fadingOut = true;
    }

    #endregion
    
    
    #endregion
    
}
