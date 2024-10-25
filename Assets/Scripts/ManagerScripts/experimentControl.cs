using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class experimentControl : MonoBehaviour
{
    
    public static experimentControl Instance { get; private set; } // used to allow easy access of this script in other scripts

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that this instance is the only one and is accessible globally
        if (Instance == null)
        {
            Instance = this;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
