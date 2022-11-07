using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRemoteGazeSphere : MonoBehaviour
{

    public GameObject remoteGazeSphere;
    private bool simulateGazeSphereRemote = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (simulateGazeSphereRemote)
        {
            // StartRecording();
            // testGazeSphere = false;
            int layerGazeSphereLocal = 1 << LayerMask.NameToLayer("RemoteGazeSphere");

            RaycastHit firstHit;
            if (Physics.Raycast(transform.position, transform.forward, out firstHit, Mathf.Infinity))
            {
                remoteGazeSphere.transform.position = firstHit.point;
            
            }
        }
        
    }
}
