using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendInteractionInfoeCon : MonoBehaviour
{
    
    public static sendInteractionInfoeCon Instance{ get; private set; }
    
    private float _samplingRate = 1.0f/ 90;

    private bool isCoolOff = false;
    private float coolOffTimerDefault = 4.0f;
    public float coolOffTimer;
    
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    public GameObject gazeSphereRemoteEffect;
    public GameObject interactionEffect;
    
    
    public int transitionO1;
    public int transitionO2;
    public int transitionO3;

    public int transitionReceivedO1;
    public int transitionReceivedO2;
    public int transitionReceivedO3;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Ensure that this instance is the only one and is accessible globally
        if (Instance == null)
        {
            Instance = this;
        }
        
        transitionO1 = 0;
        transitionO2 = 0;
        transitionO3 = 0;
        transitionReceivedO1 = 0;
        transitionReceivedO2 = 0;
        transitionReceivedO3 = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCoolOff)
        {
            if (coolOffTimer > 0)
            {
                coolOffTimer -= Time.deltaTime;
            }
            else
            {
                isCoolOff = false;
                coolOffTimer = coolOffTimerDefault;
                transitionO1 = 0;
                transitionO2 = 0;
                transitionO3 = 0;
                
            }
        }
        
    }



    public void transitionObject1()
    {
        transitionO1 = 1;
        object1.GetComponent<ObjectTransitions>().MakeTransition();
    }
    
    public void transitionObject2()
    {
        transitionO2 = 1;
        object2.GetComponent<ObjectTransitions>().MakeTransition();
    }
    
    public void transitionObject3()
    {
        transitionO3 = 1;
        object3.GetComponent<ObjectTransitions>().MakeTransition();
    }
    
    private IEnumerator sendInteractionData()
    {
        // continuously save data until stopped
        while(true)
        {
            double timeBeginnSample = GetCurrentTimestampInSeconds();

            int[] sample1 = {transitionO1};
            lslStreamsExperimentControl.Instance.eCon_makeTransitionO1_O.push_sample(sample1);
            
            int[] sample2 = {transitionO2};
            lslStreamsExperimentControl.Instance.eCon_makeTransitionO2_O.push_sample(sample2);
            
            int[] sample3 = {transitionO3};
            lslStreamsExperimentControl.Instance.eCon_makeTransitionO3_O.push_sample(sample3);
            
   
            // wait until restarting coroutine to match sampling rate
            double timeEndSample = GetCurrentTimestampInSeconds();
            if ((timeEndSample - timeBeginnSample) < _samplingRate) 
            {
                yield return new WaitForSeconds((float)(_samplingRate - (timeEndSample - timeBeginnSample)));
            }
        }

        yield return null;

    }
    
    
    public void Start_sendInteractionData()
    {
        
        StartCoroutine( sendInteractionData());
        
    }

    public void Stopp_sendInteractionData()
    {
        
        StopCoroutine( sendInteractionData() );

            
    }
    
    private double GetCurrentTimestampInSeconds()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }
    
}
