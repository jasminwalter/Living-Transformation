using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimatorController : MonoBehaviour
{
    public Vector3 headsetSpeed;
    public Vector3 headsetLocalSpeed;

    public float speedThreshold = 50f;
    [Range(0,1)]
    public float smoothing = 1;
    private Animator animator;

    private Vector3 previousPos;

    private VRRig vrRig;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        vrRig = GetComponent<VRRig>();
        previousPos = vrRig.head.vrTarget.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        // compute the speed
        headsetSpeed = (vrRig.head.vrTarget.position - previousPos) / Time.deltaTime;
        headsetSpeed.y = 0;
        
        // local speed
        headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previousPos = vrRig.head.vrTarget.position;
        
        //set animator values

        float previousDirectionX = animator.GetFloat("DirectionX");
        float previousDirectionY = animator.GetFloat("DirectionY");
        
        
        animator.SetBool("IsMoving", headsetLocalSpeed.magnitude > speedThreshold);
        animator.SetFloat("DirectionX",  Mathf.Lerp(previousDirectionX, Mathf.Clamp(headsetLocalSpeed.x,-1,1),smoothing));
        animator.SetFloat("DirectionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headsetLocalSpeed.z,-1,1),smoothing));

    }
}
