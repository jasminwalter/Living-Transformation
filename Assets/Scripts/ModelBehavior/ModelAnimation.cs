using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnimation : MonoBehaviour
{
    
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // anim.setTrigger("FadeOut1", true);
            anim.SetTrigger("FadeOut1");

            
        }


    }
}
