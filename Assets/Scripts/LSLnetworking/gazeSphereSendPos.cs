using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gazeSphereSendPos : MonoBehaviour
{

    public lslStreams lslStreams;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float[] gSpos =
        {
            this.transform.position.x,
            this.transform.position.y,
            this.transform.position.z
        };
        
        lslStreams.gazeSpherePos_O.push_sample(gSpos);
        
    }
}
