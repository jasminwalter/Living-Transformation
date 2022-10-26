using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

[System.Serializable]
public class MapTransforms
{
    public Transform vrTargets;
    public Transform ikTarget;
    
    public Vector3 trackingPositionsOffset;
    public Vector3 trackingRotationOffset;

    public void VRMapping()
    {
        ikTarget.position = vrTargets.TransformPoint(trackingPositionsOffset);
        ikTarget.rotation = vrTargets.rotation * Quaternion.Euler(trackingRotationOffset);
    }


}

public class VRRig_VersionRPM : MonoBehaviour
{

    [SerializeField] private MapTransforms head;

    [SerializeField] private MapTransforms leftHand;
    [SerializeField] private MapTransforms rightHand;

    [SerializeField] private float turnSmoothness;

    [SerializeField] private Transform ikhead;

    [SerializeField] private Vector3 headBodyOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = ikhead.position + headBodyOffset;

        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikhead.forward, Vector3.up).normalized, Time.deltaTime *turnSmoothness);
        
        head.VRMapping();
        leftHand.VRMapping();
        rightHand.VRMapping();


    }
}
