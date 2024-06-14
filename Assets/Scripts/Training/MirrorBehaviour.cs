using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehaviour : MonoBehaviour
{
    //References
    public Transform head;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (head != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = head.position.y;
            newPosition.x = head.position.x;
            transform.position = newPosition;

            Debug.Log($"(A: {transform.position.y}; B: {head.position.y})");
        }
    }
}
