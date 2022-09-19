using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagEnglish : MonoBehaviour
{

    public QuestionsManager questionManager;
    // Start is called before the first frame update
    void Start()
    {
        
        questionManager = GameObject.Find("QuestionManager").GetComponent<QuestionsManager>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            questionManager.english = true;
            questionManager.languageQuestionAnswered = true;
        }
        
    }
}
