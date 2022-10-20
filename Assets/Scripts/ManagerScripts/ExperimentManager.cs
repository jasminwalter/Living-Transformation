using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ExperimentManager : MonoBehaviour
{
    
    public Transform LocalGazeSphere;
    public Transform RemoteGazeSphere;
    public Transform RemoteGazeSphe2;
    public Transform localPlayer;
    public Transform remotePlayer;
    public Transform startPositionExperiment;
    
    public bool LocalResponseGiven;
    public bool RemoteResponseGiven;

    public bool LocalPlayerReady;
    public bool RemotePlayerReady;

    public bool localStartExperiment;
    public bool remoteStartExperiment;
    
    public bool startExperimentPress= false;
    public bool useHDMI;
    public bool withEyeTracking = false;
    
    public bool randomSeedSet = false;
    public int randomSeed;

    // private SpawningManager _spawnManager;
    
    
    
    
    //---------------------------------------------

    private static ExperimentManager _Instance;
    public static ExperimentManager Instance()
    {
        Debug.Assert(_Instance!=null);
        return _Instance;
    }
    
    [Header("Experiment Settings"), Range(1.0f, 10.0f)]
    public float WarmUpTime = 3.0f;

    [Range(1.0f, 10.0f)]
    public float InterpolationFactor = 5.0f;

    [Header("Required References")]
    public NetworkManager NetMan;
    
    public TextMesh infoTextVR;
    
    public TextMesh infoTextFallback;

    public GameObject playerSteam;
   
    

    private EExperimentStatus Status;
    private float WarmUpTimer;

    


    public void ReceivedUserStateUpdate(UserState incomingState)
    {

        RemoteGazeSphere.position = Vector3.Lerp(RemoteGazeSphere.position,incomingState.GazeSpherePosition,Time.deltaTime * InterpolationFactor);

        RemoteGazeSphe2.position = incomingState.GazeSpherePosition;
        remotePlayer.position = Vector3.Lerp(remotePlayer.position,incomingState.playerPosition,Time.deltaTime * InterpolationFactor);

        //RemoteResponseGiven = incomingState.responseGiven;
        //_spawnManager.trialAnswer = incomingState.trialAnswer;
        RemotePlayerReady = incomingState.playerReady;
        remoteStartExperiment = incomingState.startExperiment;

    }

    public void ReceivedRandomStateUpdate(RandomState incomingState)
    {
        randomSeed =  (int) incomingState.randomSeed;
        // _spawnManager.SetRandomObject(incomingState.randomSeed);
    }
    
    public void ReceivedResponseStateUpdate(ResponseState incomingState)
    {
        RemoteResponseGiven = incomingState.responseGiven;
        // _spawnManager.trialAnswer = incomingState.trialAnswer;
    }

    public void SetExperimentStatus(EExperimentStatus status)
    {
        Status = status;
        if (NetMan.IsServer())
        {
            NetMan.BroadCastExperimentStatusUpdate(Status);
            
        }

        if (status == EExperimentStatus.Waiting)
        {
            // CountdownDisplay.text = "Waiting...";
            // CountdownDisplay.gameObject.SetActive(true);
        }
        else if (status == EExperimentStatus.WarmUp)
        {
            WarmUpTimer = WarmUpTime;
            // CountdownDisplay.gameObject.SetActive(true);
        }
        else // Running
        {
            
            // CountdownDisplay.gameObject.SetActive(false);
            
        }
    }

    public EExperimentStatus GetExperimentStatus()
    {
        return Status;
    }

    public void ClientDisconected()
    {
        if (NetMan.IsServer())
        {
            switch (Status)
            {
                case EExperimentStatus.Waiting:
                    break;

                case EExperimentStatus.WarmUp:
                case EExperimentStatus.Running:
                    AbortExperiment();
                    break;
            }
        }
        else
        {
            Status = EExperimentStatus.Waiting;
        }
    }

    public void AbortExperiment()
    {
        if (!NetMan.IsServer())
        {
            Debug.LogWarning("Experiment abortion is only possible as server!");
            return;
        }
        if (Status == EExperimentStatus.Waiting)
        {
            return;
        }
        
        
        //Save data 
        
        SetExperimentStatus(EExperimentStatus.Waiting);
        Debug.Log("Experiment aborted!");
    }

  
    private void Awake()
    {
        _Instance = this;
    }
    
    private void Start()
    {
        Debug.Assert(NetMan != null, "Sample Network Manager is not set");
        
        Debug.Assert(RemoteGazeSphere != null, "Remote GazeSphere is not set");
        Debug.Assert(LocalGazeSphere != null, "Local GazeSphere is not set");

        
        Status = EExperimentStatus.Waiting;

        // _spawnManager = GetComponentInChildren<SpawningManager>();
        
        localStartExperiment = false;
        remoteStartExperiment = false;
    }

    private void Update()
    {
        // if (startExperimentPress)
        // {
        //     localStartExperiment = true;
        //     
        //     startExperimentPress = false;
        // }
        //
        // // if only local start experiment true
        // if (localStartExperiment & !remoteStartExperiment)
        // {
        //     // activate both text meshs
        //     infoTextVR.text = "waiting for partner";
        //     infoTextVR.gameObject.SetActive(true);
        //     infoTextFallback.text = "waiting for partner";
        //     infoTextFallback.gameObject.SetActive(true);
        // }
        //
        // // if only remote start experiment true
        // if (!localStartExperiment & remoteStartExperiment)
        // {
        //     // activate both text meshs
        //     infoTextVR.text = "partner is ready";
        //     infoTextVR.gameObject.SetActive(true);
        //     infoTextFallback.text = "partner is ready";
        //     infoTextFallback.gameObject.SetActive(true);
        // }
        //
        // // if both participants have pressed start experiment
        // if (localStartExperiment & remoteStartExperiment)
        // {
        //     // deactivate both info texts
        //     infoTextVR.gameObject.SetActive(false);
        //     infoTextFallback.gameObject.SetActive(false);
        //
        //     
        //     // then teleport players
        //     localPlayer.position = startPositionExperiment.position;
        //     localPlayer.localEulerAngles = new Vector3(0, 0, 0);
        //     // _spawnManager.inExperimentRoom = true;
        //
        //
        //     if (NetMan.IsServer() && !randomSeedSet)
        //     {
        //         Random rnd = new Random();
        //         randomSeed = rnd.Next(0, 10000000);
        //         // _spawnManager.SetRandomObject((float) randomSeed);
        //         NetMan.BroadCastRandomState((float) randomSeed);
        //         randomSeedSet = true;
        //
        //     }
        //
        //     // localStartExperiment = false;
        //     // remoteStartExperiment = false; // should be set by network, but to make sure
        //
        // }
        
        if (NetMan.GetState() == ENetworkState.Running)
        {
            NetMan.BroadcastExperimentState(LocalGazeSphere, localPlayer, LocalPlayerReady, localStartExperiment);
        }

        if (Status == EExperimentStatus.WarmUp)
        {
            WarmUpTimer -= Time.deltaTime;
            if (WarmUpTimer <= 0.0f)
            {
                WarmUpTimer = 0.0f;
                if (NetMan.IsServer())
                {
                    SetExperimentStatus(EExperimentStatus.Running);
                }
            }
            //CountdownDisplay.text = "Get Ready\n" + Mathf.Ceil(WarmUpTimer).ToString();
        }
    }
}
