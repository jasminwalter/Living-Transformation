using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    public NetworkManager NetMan;
    public ExperimentManager Exp;
    public Text ExperimentState;
    public Text NetworkState;
    public InputField AddressField;
    public Button BtnStartServer;
    public Button BtnStartClient;
    public Button BtnAbortClose;
    public Text ClientList;

    // set non/default value as "last" value, so the update kicks in at least once
    ENetworkState PreviousNetState = ENetworkState.Startup;
    EExperimentStatus PreviousExpStatus = EExperimentStatus.WarmUp;
    bool bPreviousServerState = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(NetMan != null);
        Debug.Assert(Exp!= null);
        Debug.Assert(ExperimentState != null);
        Debug.Assert(NetworkState != null);
        Debug.Assert(AddressField != null);
        Debug.Assert(BtnStartServer != null);
        Debug.Assert(BtnStartClient != null);
        Debug.Assert(BtnAbortClose != null);
        Debug.Assert(ClientList != null);
        // Debug.Assert(BtnStartExp != null);
        // Debug.Assert(BtnAbortExp != null);

        BtnStartServer.onClick.AddListener(BtnStartServerClicked);
        BtnStartClient.onClick.AddListener(BtnStartClientClicked);
        BtnAbortClose.onClick.AddListener(BtnAbortCloseClicked);
        // BtnStartExp.onClick.AddListener(BtnStartExperiment);
        // BtnAbortExp.onClick.AddListener(BtnAbortExperiment);

        NetMan.NetComp.OnClientConnected += (object sender, ConnectionEventArgs e) => 
        {
            RefreshUI();
        };

        NetMan.NetComp.OnClientDisconnected += (object sender, ConnectionEventArgs e) =>
        {
            RefreshUI();
        };
    }

    void BtnStartServerClicked()
    {
        ENetworkState netState = NetMan.GetState();
        Debug.Assert(netState == ENetworkState.Closed);

        PreviousNetState = netState;
        NetMan.StartServer();
    }

    void BtnStartClientClicked()
    {
        ENetworkState netState = NetMan.GetState();
        Debug.Assert(netState == ENetworkState.Closed);
    
        PreviousNetState = netState;
        string address = AddressField.text;
        address = address.Length == 0 ? "127.0.0.1" : address;
        NetMan.StartClient(address);
    }

    void BtnAbortCloseClicked()
    {
        ENetworkState netState = NetMan.GetState();
        Debug.Assert(netState == ENetworkState.Running || netState == ENetworkState.Startup);
        PreviousNetState = NetMan.GetState();
    
        if (NetMan.IsServer())
        {
            Exp.AbortExperiment();
        }
    
        NetMan.Close();
    }

    // void BtnStartExperiment()
    // {
    //     EExperimentStatus expStatus = Exp.GetExperimentStatus();
    //     Debug.Assert(expStatus == EExperimentStatus.Waiting);
    //     // Exp.StartExperiment();
    // }

    // void BtnAbortExperiment()
    // {
    //     EExperimentStatus expStatus = Exp.GetExperimentStatus();
    //     Debug.Assert(expStatus != EExperimentStatus.Waiting);
    //     Exp.AbortExperiment();
    // }

    void RefreshUI()
    {
        ENetworkState netState = NetMan.GetState();
        EExperimentStatus expStatus = Exp.GetExperimentStatus();

        BtnStartServer.gameObject.SetActive(netState == ENetworkState.Closed);
        BtnStartClient.gameObject.SetActive(netState == ENetworkState.Closed);
        BtnAbortClose.gameObject.SetActive(netState == ENetworkState.Running || netState == ENetworkState.Startup);
        PreviousNetState = netState;

        // if (NetMan.IsServer())
        // {
        //     BtnStartExp.gameObject.SetActive(expStatus == EExperimentStatus.Waiting && NetMan.GetNumConnections() == 1);
        //     BtnAbortExp.gameObject.SetActive(expStatus != EExperimentStatus.Waiting);
        // }
        // else
        // {
        //     // BtnStartExp.gameObject.SetActive(false);
        //     // BtnAbortExp.gameObject.SetActive(false);
        // }
        PreviousExpStatus = expStatus;
        bPreviousServerState = NetMan.IsServer();

        NetworkState.text = (netState != ENetworkState.Closed ? (NetMan.IsServer() ? "Server - " : "Client - ") : "") + netState.ToString();
        ExperimentState.text = expStatus.ToString();

        ClientList.text = "";
        string[] conns = NetMan.NetComp.GetConnectionNames();
        foreach (string name in conns)
        {
            ClientList.text += name + "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        ENetworkState netState = NetMan.GetState();
        EExperimentStatus expStatus = Exp.GetExperimentStatus();
        if (netState != PreviousNetState || expStatus != PreviousExpStatus || bPreviousServerState != NetMan.IsServer())
        {
            RefreshUI();
        }
    }
}
