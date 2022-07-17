using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingMovement : MonoBehaviour
{
    
    private float minimum = 1.0f;

    private float maximum = 1.2f;
    
    private float minimum2 = 1.2f;

    private float maximum2 = 1.3f;
    
    
    private float t1max = 0.95f;
    private float t2max = 1.0f;

    public float t = 0.0f;

    public bool rest = false;

    // private float delay = 2.0f;

    public float count= 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {

    
    
    }

    // Update is called once per frame
    void Update()
    {
        if (t <= maximum)
        {
            transform.localScale = new Vector3(Mathf.Lerp(minimum, maximum, t), Mathf.Lerp(minimum, maximum, t), Mathf.Lerp(minimum, maximum, t));

            t += 0.4f * Time.deltaTime;
        }

        
        else if (t > maximum & t< maximum2)
        {
            transform.localScale = new Vector3(Mathf.Lerp(minimum2, maximum2, t), Mathf.Lerp(minimum2, maximum2, t), Mathf.Lerp(minimum2, maximum2, t));
            t += 0.25f * Time.deltaTime;
        }
        else if (rest == true && count <= t2max)
        {
            transform.localScale = new Vector3(Mathf.Lerp(minimum2, maximum2, count), Mathf.Lerp(minimum2, maximum2, count), Mathf.Lerp(minimum2, maximum2, count));

            count += 0.2f * Time.deltaTime;
        }
        else if(rest == true && count > t2max)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
            temp = maximum2;
            maximum2 = minimum2;
            minimum2 = temp;
            rest = false;
            count = 0.95f;
            Debug.Log("back");
        }
        else
        {

        }

    }
}
