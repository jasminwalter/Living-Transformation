using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSphereInteraction : MonoBehaviour
{
    private static GazeSphereInteraction _Instance;
    public static GazeSphereInteraction Instance()
    {
        Debug.Assert(_Instance!=null);
        return _Instance;
    }
    
    public bool gazeSphereCollision = false;
    public bool artObjectCollision = false;

    public bool isObject1 = false;
    public bool isObject2 = false;
    public bool isObject3 = false;
    
    public bool transitionCountdown = false;
    private float interactionTimerDefault = 0.5f;
    public float interactionTimer;

    public bool coolOffPeriod = false;
    private float coolOffTimerDefault = 12.0f;
    public float coolOffTimer;

    public bool isServer;
        

    public GameObject gazeSphereRemoteEffect;

    public GameObject interactionEffect;

    // public GameObject object1;
    // public GameObject object2;
    // public GameObject object3;
    
    // Start is called before the first frame update
    

    private void OnEnable()
    {
        interactionTimer = interactionTimerDefault;
        coolOffTimer = coolOffTimerDefault;
        
        gazeSphereCollision = false;
        artObjectCollision = false;

        isObject1 = false;
        isObject2 = false;
        isObject3 = false;
    
        transitionCountdown = false;
        coolOffPeriod = false;
        
        gazeSphereRemoteEffect.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (artObjectCollision & gazeSphereCollision)
        {
            if (!coolOffPeriod)
            {
                interactionEffect.SetActive(true);
                transitionCountdown = true;
            }
        } 
        if (transitionCountdown)
        {
            if (interactionTimer > 0)
            {
                interactionTimer -= Time.deltaTime;
            }
            else
            {
                
                // gazeSphereRemoteEffect.SetActive(false);
                // interactionEffect.SetActive(false);
                // trigger object transitions depending on which object is viewed
                if (ExperimentManager.Instance().isServer)
                {
                    if (isObject1)
                    {
                    
                        // object1.GetComponent<ObjectTransitions>().MakeTransition();
                        ExperimentManager.Instance().triggerTransitionObj1_server = true;
                    }

                    if (isObject2)
                    {
                        // object2.GetComponent<ObjectTransitions>().MakeTransition();
                        ExperimentManager.Instance().triggerTransitionObj2_server = true;
                    }

                    if (isObject3)
                    {
                        // object3.GetComponent<ObjectTransitions>().MakeTransition();
                        ExperimentManager.Instance().triggerTransitionObj3_server = true;
                    }
                }
                else
                {
                    if (isObject1)
                    {

                        // object1.GetComponent<ObjectTransitions>().MakeTransition();
                        ExperimentManager.Instance().triggerTransitionObj1_other = true;
                    }

                    if (isObject2)
                    {
                        // object2.GetComponent<ObjectTransitions>().MakeTransition();
                        ExperimentManager.Instance().triggerTransitionObj2_other = true;
                    }

                    if (isObject3)
                    {
                        // object3.GetComponent<ObjectTransitions>().MakeTransition();
                        ExperimentManager.Instance().triggerTransitionObj3_other = true;
                    }
                }


                // transitionCountdown = false;
                // interactionTimer = interactionTimerDefault;
                // coolOffPeriod = true;

            }
        }

        if (coolOffPeriod)
        {
            if (coolOffTimer > 0)
            {
                coolOffTimer -= Time.deltaTime;
            }
            else
            {
                coolOffPeriod = false;
                coolOffTimer = coolOffTimerDefault;
                gazeSphereRemoteEffect.SetActive(true);
                if (artObjectCollision & gazeSphereCollision)
                {
                    interactionEffect.SetActive(true);
                    transitionCountdown = true;
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!coolOffPeriod)
        {
            if (other.CompareTag("LocalGazeSphere"))
            {
                gazeSphereCollision = true;
            }

            if (other.CompareTag("Object1"))
            {
                isObject1 = true;
                artObjectCollision = true;
            }

            if (other.CompareTag("Object2"))
            {
                isObject2 = true;
                artObjectCollision = true;
            }

            if (other.CompareTag("Object3"))
            {
                isObject3 = true;
                artObjectCollision = true;
            }

            if (artObjectCollision & gazeSphereCollision)
            {
                interactionEffect.SetActive(true);
                transitionCountdown = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LocalGazeSphere"))
        {
            gazeSphereCollision = false;
            if (transitionCountdown)
            {
                interactionEffect.SetActive(false);
                transitionCountdown = false;
                interactionTimer = interactionTimerDefault;
                
            }
        }

        if (other.CompareTag("Object1"))
        {
            isObject1 = false;
            artObjectCollision = false;
            if (transitionCountdown)
            {
                interactionEffect.SetActive(false);
                transitionCountdown = false;
                interactionTimer = interactionTimerDefault;
            }

        }
    
        if (other.CompareTag("Object2"))
        {
            isObject2 = false;
            artObjectCollision = false;
            if (transitionCountdown)
            {
                interactionEffect.SetActive(false);
                transitionCountdown = false;
                interactionTimer = interactionTimerDefault;
            }

        }
    
        if (other.CompareTag("Object3"))
        {
            isObject3 = false;
            artObjectCollision = false;
            if (transitionCountdown)
            {
                interactionEffect.SetActive(false);
                transitionCountdown = false;
                interactionTimer = interactionTimerDefault;
            }
        }
    }

    public void EndInteractionEffects()
    {
        gazeSphereRemoteEffect.SetActive(false);
        interactionEffect.SetActive(false);
    }

    public void ResetInteractionTimer()
    {
        interactionTimer = interactionTimerDefault;
    }
}
