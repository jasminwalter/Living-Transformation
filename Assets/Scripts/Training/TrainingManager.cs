using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    //References
    public QuestionsManager questionsManager;

    public GameObject hitpoint1;
    public GameObject hitpoint2;
    
    // Booleans to check if the hitpoints are reached
    public bool hitpoint1Reached;
    public bool hitpoint2Reached;
    
    //Arguments
    public int index = 0;

    private void Start()
    {
        hitpoint1.SetActive(false);
        hitpoint2.SetActive(false);

        hitpoint1Reached = false;
        hitpoint2Reached = false;
        Debug.Log("Script in process");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if both hitpoints are reached and trigger the 'next' button
        StartCoroutine(CheckUpdate());
    }

    // Triggered when something enters the trigger collider

    public IEnumerator HitpointTrigger()
    {
        index += 1;

        if (index == 1)
        {
            hitpoint1.SetActive(false);
            hitpoint1Reached = true;
            hitpoint2.SetActive(true);
            Debug.Log("Teleportation Hit Point 1 reached by local player");
        }
        else if (index == 2)
        {
            hitpoint2Reached = true;
            hitpoint2.SetActive(false);
            Debug.Log("Teleportation Hit Point 2 reached by local player");
        }

        yield return null;
    }

    private IEnumerator CheckUpdate()
    {
        if (hitpoint1Reached && hitpoint2Reached)
        {
            StartCoroutine(questionsManager.CheckPointsDone());
        }
        yield return null;
    }

    // Call this method to proceed to the next part of the training
    void TriggerNextButton()
    {
        Debug.Log("Both Teleportation Hit Points reached by local player. Proceed to the next stage.");
        // Here you can add the code to enable the next button or trigger the next stage
    }
}