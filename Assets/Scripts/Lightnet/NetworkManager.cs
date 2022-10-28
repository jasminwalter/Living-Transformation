using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NetworkManager : MonoBehaviour
{
    [Header("Network Settings")]
    public int PortReliable = 42069;
    public int PortUnreliable = 42169;
    public int MaxClients = 1;

    [Header("Required References")]
    public NetworkComponent NetComp;

    private UserState SendingUserState = new UserState();

    private SynchronizationState SendingSynchroState = new SynchronizationState();
    // private RandomState SendingRandomState = new RandomState();
    // private ResponseState SendingResponseState = new ResponseState();
    
    


    public bool StartServer()
    {
        return NetComp.StartServer(PortReliable, PortUnreliable, MaxClients);
    }

    public bool StartClient(string address)
    {
        return NetComp.StartClient(address, PortReliable, PortUnreliable);
    }

    public bool Close()
    {
        return NetComp.Close();
    }

    public ENetworkState GetState()
    {
        return NetComp.GetState();
    }

    public int GetNumConnections()
    {
        return NetComp.GetConnections().Length;
    }

    public bool IsServer()
    {
        return NetComp.IsServer();
    }

    public void BroadCastExperimentStatusUpdate(EExperimentStatus status)
    {
        //Debug.LogFormat("Broadcasting experiment status update: {0}", status.ToString());
        NetComp.BroadcastNetworkData(ENetChannel.Reliable, new ExperimentState { Status = status });
    }


    

    // public void BroadcastExperimentState(Transform gazeSphereTransform, Transform playerPosTransform, 
    //     Vector3 VRCameraPosition, Quaternion VRCameraRotation, Vector3 VRRightHandPosition, Quaternion VRRightHandRotation,
    //     Vector3 VRLeftHandPosition, Quaternion VRLeftHandRotation, List<float> EyeShapeTable, List<float> EyeShape2IntTable,
    //     bool playerReady, bool startExperiment) //, bool responseGiven,bool trialAnswer)
    public void BroadcastExperimentState(Transform gazeSphereTransform, Transform playerPosTransform, 
        Vector3 VRCameraPosition, Quaternion VRCameraRotation, Vector3 VRRightHandPosition, Quaternion VRRightHandRotation,
        Vector3 VRLeftHandPosition, Quaternion VRLeftHandRotation, Quaternion eyeShapePart1, Quaternion eyeShapePart2, Quaternion eyeShapePart3,
        Quaternion eyeBlinkVal1, Quaternion eyeBlinkVal2, Vector3 eyeBlinkVal3) 
    {
        SendingUserState.GazeSpherePosition = gazeSphereTransform.position;
        SendingUserState.playerPosition = playerPosTransform.position;
        
        SendingUserState.VRCameraPosition = VRCameraPosition;
        SendingUserState.VRCameraRotation = VRCameraRotation;
        SendingUserState.VRRightHandPosition = VRRightHandPosition;
        SendingUserState.VRRightHandRotation = VRRightHandRotation;
        SendingUserState.VRLeftHandPosition = VRLeftHandPosition;
        SendingUserState.VRLeftHandRotation = VRLeftHandRotation;

        SendingUserState.eyeShapePart1 = eyeShapePart1;
        SendingUserState.eyeShapePart2 = eyeShapePart2;
        SendingUserState.eyeShapePart3 = eyeShapePart3;

        SendingUserState.EyeBlinkVal1 = eyeBlinkVal1;
        SendingUserState.EyeBlinkVal2 = eyeBlinkVal2;
        SendingUserState.EyeBlinkVal3 = eyeBlinkVal3;
        

        NetComp.BroadcastNetworkData(ENetChannel.Unreliable, SendingUserState);
        
    }

    public void BroadCastSynchronizationState(bool languageSelection, bool enterExhibition, bool exitExhibition, bool newStartVisitor)
    {
        SendingSynchroState.languageSelection = languageSelection;
        SendingSynchroState.enterExhibition = enterExhibition;
        SendingSynchroState.exitExhibition = exitExhibition;
        SendingSynchroState.newStartVisitor = newStartVisitor;
        
        NetComp.BroadcastNetworkData(ENetChannel.Reliable, SendingSynchroState);

    }

    // public void BroadCastRandomState(float randomSeed)
    // {
    //     SendingRandomState.randomSeed = randomSeed;
    //     NetComp.BroadcastNetworkData(ENetChannel.Unreliable, SendingRandomState);
    // }
    //
    // public void BroadCastResponseState(bool responseGiven, bool trialAnswer)
    // {
    //     SendingResponseState.responseGiven = responseGiven;
    //     SendingResponseState.trialAnswer = trialAnswer;
    //     NetComp.BroadcastNetworkData(ENetChannel.Reliable, SendingResponseState);
    // }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(NetComp != null, "Please set a Network Component");
        NetComp.OnNetworkDataReceived += OnNetworkDataReceived;
        NetComp.OnClientDisconnected  += OnClientDisconected;
        NetComp.OnClientConnected     += OnClientConnected;
    }

    // Update is called once per frame
    void Update()
    {
        ENetworkState state = GetState();

        if (state==ENetworkState.Running)
        {
            if (NetComp.IsServer())
            {
                UpdateServer();
            }
            else
            {
                UpdateClient();
            }
        }
    }

    void UpdateClient()
    {
        Debug.Assert(GetState()==ENetworkState.Running);
    }

    void UpdateServer()
    {
        Debug.Assert(GetState()==ENetworkState.Running);
    }
   
    
    void OnNetworkDataReceived(object sender, ReceivedNetworkDataEventArgs e)
    {
        NetworkData data = e.Data;
        switch (e.Type)
        {
            case ENetDataType.ExperimentState:
            {
                ExperimentState state =(ExperimentState) data;
                ExperimentManager.Instance().SetExperimentStatus(state.Status);
                break;
            }
            case ENetDataType.UserState:
                ExperimentManager.Instance().ReceivedUserStateUpdate((UserState)data);
                break;

            case ENetDataType.PartnerSynchronization:
                ExperimentManager.Instance().ReceiveSynchroStateUpdate((SynchronizationState) data);
                break;
                // case ENetDataType.RandomState:
            //     ExperimentManager.Instance().ReceivedRandomStateUpdate((RandomState)data);
            //     break;
            // case ENetDataType.ResponseState:
            //     ExperimentManager.Instance().ReceivedResponseStateUpdate((ResponseState)data);
            //     break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    void OnClientConnected(object sender, ConnectionEventArgs e)         //if you are a server than, you handle clients
    {
        if (NetComp.IsServer())
        {
            BroadCastExperimentStatusUpdate(ExperimentManager.Instance().GetExperimentStatus());
        }
    }
    
    
    void OnClientDisconected(object sender, ConnectionEventArgs e)
    {
        ExperimentManager.Instance().ClientDisconected();
    }
}
