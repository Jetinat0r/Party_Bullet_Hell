using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

/// <summary>
/// Network Manager for the CLIENT
/// </summary>
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
    }

    // Start is called before the first frame update
    void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        Client.Connect($"{ip}:{port}");
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
