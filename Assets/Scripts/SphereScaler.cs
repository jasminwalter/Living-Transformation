using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScaler : MonoBehaviour
{
    public Transform local_playerLocation;
    private int scaleFactor = 20;
    private float distance;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // scale sphere depending on distance to players
        // since both players will be at the same location (rotation does not matter for distance)
        // we can use the local player location for calculating the distance
        distance = Vector3.Distance(this.transform.position, local_playerLocation.position);
        this.transform.localScale =  new Vector3(
            distance/scaleFactor,
            distance/scaleFactor,
            distance/scaleFactor);

    }
    
}
