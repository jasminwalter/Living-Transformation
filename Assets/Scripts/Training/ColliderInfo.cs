using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public class ColliderInfo : MonoBehaviour
{
    //References
    public TrainingManager trainingManager;
    public GameObject localCamera;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the local player that enters the trigger
        if (other.gameObject == localCamera)
        {
            Debug.Log("Teleportation Hit Point reached by local player");
            StartCoroutine(trainingManager.HitpointTrigger());
            
        }

    }

}
