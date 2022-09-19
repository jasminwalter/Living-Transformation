using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagGerman : MonoBehaviour
{
    
    public QuestionsManager questionManager;
    // Start is called before the first frame update
    void Start()
    {
        
        questionManager = GameObject.Find("QuestionManager").GetComponent<QuestionsManager>();
        
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Hand"))
    //     {
    //         questionManager.german = true;
    //         questionManager.languageQuestionAnswered = true;
    //     }
    //     
    // }
    

}
