using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ExperimentManager : MonoBehaviour
{
    private static ExperimentManager _Instance;
    public static ExperimentManager Instance()
    {
        Debug.Assert(_Instance!=null);
        return _Instance;
    }
    
    // Player
    [Header("Player Vars")]

    public Transform localPlayer;
    public Transform remotePlayer;

    public Transform VRCamera_local;
    public Transform VRHandRight_local;
    public Transform VRHandLeft_local;
    
    public Transform VRCamera_remote;
    public Transform VRHandRight_remote;
    public Transform VRHandLeft_remote;
    
    public Transform startPositionExperiment;
    
    // Eye Tracking Networking Vars
    [Header("ET Vars")]
    public Transform LocalGazeSphere;
    public Transform RemoteGazeSphere;
    
    // public List<float> localEyeShapeTable;
    // public List<float> remoteEyeShapeTable;
    // public List<float> localEyeShape2IntTable;
    // public List<float> remoteEyeShape2IntTable;

    public Quaternion eyeShapePart1_local;
    public Quaternion eyeShapePart2_local;
    public Quaternion eyeShapePart3_local;
    
    public Quaternion eyeShapePart1_remote;
    public Quaternion eyeShapePart2_remote;
    public Quaternion eyeShapePart3_remote;

    public Quaternion EyeBlinkVal1_local;
    public Quaternion EyeBlinkVal2_local;
    public Vector3 EyeBlinkVal3_local;
    
    public Quaternion EyeBlinkVal1_remote;
    public Quaternion EyeBlinkVal2_remote;
    public Vector3 EyeBlinkVal3_remote;


    public bool inExhibitionArea = false;
    
    // synchronization variables

    public bool localLanguageSelected = false;
    public bool remoteLanugageSelected = false;

    public bool localEnterExhibition = false;
    public bool remoteEnterExhibition = false;

    public bool localExhibitionExit = false;
    public bool remoteExhibitionExit = false;

    public bool localStartNewVisitor = false;
    public bool remoteStartNewVisitor = false;


    // public bool LocalResponseGiven;
    // public bool RemoteResponseGiven;
    //
    // public bool LocalPlayerReady;
    // public bool RemotePlayerReady;
    //
    // public bool localStartExperiment;
    // public bool remoteStartExperiment;
    //
    // public bool startExperimentPress= false;
    // public bool useHDMI;
    // public bool withEyeTracking = false;
    //
    // public bool randomSeedSet = false;
    // public int randomSeed;

    // private SpawningManager _spawnManager;

    [Header("Experiment Settings"), Range(1.0f, 10.0f)]
    public float WarmUpTime = 3.0f;

    [Range(1.0f, 10.0f)]
    public float InterpolationFactor = 5.0f;

    [Header("Required References")]
    public NetworkManager NetMan;
    
    // public TextMesh infoTextVR;
    //
    // public TextMesh infoTextFallback;
    //
    // public GameObject playerSteam;



    private EExperimentStatus Status;
    private float WarmUpTimer;

    


    public void ReceivedUserStateUpdate(UserState incomingState)
    {

        // RemoteGazeSphere.position = Vector3.Lerp(RemoteGazeSphere.position,incomingState.GazeSpherePosition,Time.deltaTime * InterpolationFactor);

        Debug.Log("Received State " + incomingState.GazeSpherePosition.x);
        RemoteGazeSphere.position = incomingState.GazeSpherePosition;
        remotePlayer.position = Vector3.Lerp(remotePlayer.position,incomingState.playerPosition,Time.deltaTime * InterpolationFactor);

        VRCamera_remote.position = Vector3.Lerp(VRCamera_remote.position,incomingState.VRCameraPosition,Time.deltaTime * InterpolationFactor);;
        VRCamera_remote.rotation = Quaternion.Lerp(VRCamera_remote.rotation,incomingState.VRCameraRotation,Time.deltaTime * InterpolationFactor);;

        VRHandRight_remote.position = Vector3.Lerp(VRHandRight_remote.position,incomingState.VRRightHandPosition,Time.deltaTime * InterpolationFactor);;
        VRHandRight_remote.rotation = Quaternion.Lerp(VRHandRight_remote.rotation,incomingState.VRRightHandRotation,Time.deltaTime * InterpolationFactor);;
    
        VRHandLeft_remote.position = Vector3.Lerp(VRHandLeft_remote.position,incomingState.VRLeftHandPosition,Time.deltaTime * InterpolationFactor);;
        VRHandLeft_remote.rotation = Quaternion.Lerp(VRHandLeft_remote.rotation,incomingState.VRLeftHandRotation,Time.deltaTime * InterpolationFactor);;

        eyeShapePart1_remote = incomingState.eyeShapePart1;
        eyeShapePart2_remote = incomingState.eyeShapePart2;
        eyeShapePart3_remote = incomingState.eyeShapePart3;

        EyeBlinkVal1_remote = incomingState.EyeBlinkVal1;
        EyeBlinkVal2_remote = incomingState.EyeBlinkVal2;
        EyeBlinkVal3_remote = incomingState.EyeBlinkVal3;

        // remoteEyeShapeTable = incomingState.EyeShapeTable;
        // remoteEyeShape2IntTable = incomingState.EyeShape2IntTable;


        //RemoteResponseGiven = incomingState.responseGiven;
        //_spawnManager.trialAnswer = incomingState.trialAnswer;
        // RemotePlayerReady = incomingState.playerReady;
        // remoteStartExperiment = incomingState.startExperiment;

    }

    public void ReceiveSynchroStateUpdate(SynchronizationState incomingState)
    {
        remoteLanugageSelected = incomingState.languageSelection;
        remoteEnterExhibition = incomingState.enterExhibition;
        remoteExhibitionExit = incomingState.exitExhibition;
        remoteStartNewVisitor = incomingState.newStartVisitor;

    }
    // public void ReceivedRandomStateUpdate(RandomState incomingState)
    // {
    //     randomSeed =  (int) incomingState.randomSeed;
    //     // _spawnManager.SetRandomObject(incomingState.randomSeed);
    // }
    //
    // public void ReceivedResponseStateUpdate(ResponseState incomingState)
    // {
    //     RemoteResponseGiven = incomingState.responseGiven;
    //     // _spawnManager.trialAnswer = incomingState.trialAnswer;
    // }

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
        
        // localStartExperiment = false;
        // remoteStartExperiment = false;
        //
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
            NetMan.BroadCastSynchronizationState(localLanguageSelected, localEnterExhibition, localExhibitionExit, localStartNewVisitor);
            
            if(inExhibitionArea)
            {
                NetMan.BroadcastExperimentState(LocalGazeSphere, localPlayer, VRCamera_local.position, 
                    VRCamera_local.rotation, VRHandRight_local.position, VRHandRight_local.rotation,
                    VRHandLeft_local.position, VRHandLeft_local.rotation, eyeShapePart1_local,
                    eyeShapePart2_local,eyeShapePart3_local, EyeBlinkVal1_local, EyeBlinkVal2_local,
                    EyeBlinkVal3_local);
            }
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
