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
    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the local player that enters the trigger
        //if (other.CompareTag("Player"))
        {
            // Check if the player has entered either of the teleportation hit points
            if (other.gameObject == hitpoint1)
            {
                hitpoint1Reached = true;
                hitpoint1.SetActive(false);
                hitpoint2.SetActive(true);
                Debug.Log("Teleportation Hit Point 1 reached by local player");
            }
            else if (other.gameObject == hitpoint2)
            {
                hitpoint2Reached = true;
                hitpoint2.SetActive(false);
                Debug.Log("Teleportation Hit Point 2 reached by local player");
            }
        }
    }

    private IEnumerator CheckUpdate()
    {
        if (hitpoint1Reached && hitpoint2Reached)
        {
            StartCoroutine (questionsManager.CheckPointsDone());
            //TriggerNextButton();
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