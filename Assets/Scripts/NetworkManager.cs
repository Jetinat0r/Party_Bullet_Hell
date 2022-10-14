using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

/// <summary>
/// Network Manager for the CLIENT
/// </summary>

public enum ClientToServerId : ushort
{
    joinInfo = 1,
    playerPosRot = 2,
    shove = 3,
}

public enum ServerToClientId : ushort
{
    playerSpawnInfo = 1,
    playerPosRot = 2,
    playerShove = 3,
}

public class NetworkManager : MonoBehaviour
{
    //Singleton
    public static NetworkManager instance;

    public Client Client { get; private set; }

    [SerializeField]
    private string ip = "127.0.0.1";
    [SerializeField]
    private ushort port = 7777;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }


        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
    }

    public void JoinServer(string _userName, string _ip, string _port)
    {
        if(_ip == "" || _port == "")
        {
            Client.Connect($"{ip}:{port}");
        }
        else
        {
            Client.Connect($"{_ip}:{_port}");
        }
    }

    private void FixedUpdate()
    {
        Client.Tick();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }
}
