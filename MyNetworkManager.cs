using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;


public class MyNetworkManager : NetworkManager
{
    //Se llama en el cliente cuando se conecta a un servidor
    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }

    //Se llama en el servidor cada que se conecta un cliente
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    
}
