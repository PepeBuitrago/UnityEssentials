using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Server Configuration", menuName = "Configuration/Create New Server Configuration")]
public class ServerConfiguration : ScriptableObject
{
    public string playerName;
    public int sceneCmd;
    public string address;
    public ushort port;
}
