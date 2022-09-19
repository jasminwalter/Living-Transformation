using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsManager : MonoBehaviour
{
    public GameObject languageSelection;
    public GameObject flagGerman;
    public GameObject flagEnglish;
    public GameObject textGerman;
    public GameObject textEnglish;
    public bool german = false;
    public bool english = false;
    public bool languageQuestionAnswered = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (languageQuestionAnswered)
        {
            // save answer
            
            // deactivate question and activate next one
            languageSelection.SetActive(false);
        }
    }

    public void LanguageSelection()
    {
        // save answer
        
        languageSelection.SetActive(false);
        german = true;
    }
}
