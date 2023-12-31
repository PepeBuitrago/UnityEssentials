using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class SetupNetwork : MonoBehaviour
{
    [SerializeField]
    NetworkManager manager;

    [SerializeField] 
    ServerConfiguration serverConfig;

    public static SetupNetwork Instance;

    void Start()
    {
        Instance = this;
        manager = GetComponent<MyNetworkManager>();

        if (PlayerPrefs.HasKey("MultiplayerTipe"))
        {
            int multiplayerType = PlayerPrefs.GetInt("MultiplayerTipe");
            if (multiplayerType == 0)
            {
                manager.StartHost();
                NotificationManager.Instance.Notification("Se ha iniciado como Host.", 5f);
            }
            else if (multiplayerType == 1)
            {
                manager.StartClient();
                NotificationManager.Instance.Notification("Se ha iniciado como Cliente.", 5f);
            }
            else if (multiplayerType == 2)
            {
                manager.StartServer();
                NotificationManager.Instance.Notification("Se ha iniciado como Servidor.", 5f);
            }
        }
        
    }
}
