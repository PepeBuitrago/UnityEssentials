using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : NetworkBehaviour
{
    public int maxMessages = 25;
    public GameObject chatPanel;
    public GameObject textPrefab;
    public TMP_InputField chatInput;

    private readonly List<GameObject> messages = new List<GameObject>();
    public PlayerController player;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        //if (isLocalPlayer)  SetupGUI();//Si se ejecuta dentro de la comprobacion isLocalPlayer crashea
        
    }
    private void Start()
    {
        if (isLocalPlayer)
        {
            //CmdSendChatMessage($"<color=red>Bienvenido {player.namePlayer}.</color>", player.namePlayer);
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrWhiteSpace(chatInput.text))
        {
            ProcessChatCommand(chatInput.text);
            chatInput.text = string.Empty;  //Reinicia el campo de entrada
            chatInput.ActivateInputField();  // Activa el campo de entrada
            chatInput.Select();  // Selecciona el campo de entrada
        }
    }


    public void SetupGUI()
    {
        chatPanel = GameObject.Find("ContentChat");
        chatInput = GameObject.Find("InputFieldChat").GetComponent<TMP_InputField>();
    }

    [Command]
    private void CmdSendChatMessage(string message, string name)
    {
        RpcReceiveChatMessage(message);
        LogSystem.LogMessage($"{name}", message, "LogChat.txt");
    }

    [ClientRpc]
    private void RpcReceiveChatMessage(string message)
    {
        AddMessageToChat(message);
    }

    private void AddMessageToChat(string message)
    {
        if (messages.Count >= maxMessages)
        {
            Destroy(messages[0]);
            messages.RemoveAt(0);
        }

        GameObject newText = Instantiate(textPrefab, chatPanel.transform);
        newText.GetComponent<TMP_Text>().text = message;
        messages.Add(newText);
    }

    // Función para verificar si un mensaje contiene palabras censurables y obtener las palabras censurables encontradas
    static List<string> ContainsForbiddenWords(string mensaje, string[] palabrasCensurables)
    {
        List<string> palabrasEncontradas = new List<string>();

        foreach (string palabra in palabrasCensurables)
        {
            if (mensaje.Contains(palabra))
            {
                palabrasEncontradas.Add(palabra);
            }
        }

        return palabrasEncontradas;
    }

    private void ProcessChatCommand(string message)
    {
        List<string> palabrasEncontradas = ContainsForbiddenWords(message, WordsClass.forbiddenWords);
        string words = "";

        if (palabrasEncontradas.Count == 0)
        {
            if (message.StartsWith("cmd/ "))
            {
                // Eliminamos el prefijo "cmd/" para obtener el comando y los argumentos
                string command = message.Substring(5);

                // Dividimos el mensaje en partes separadas por espacios
                string[] parts = command.Split(' ');

                // El primer elemento después de "cmd/" es el comando
                string cmd = parts[0];

                // Dependiendo del comando, ejecutar la lógica correspondiente
                switch (cmd)
                {
                    // cmd/ tp (id) (vector3)
                    case "tp":
                        if (parts.Length > 2)
                        {
                            // El segundo elemento es el argumento
                            string coords = parts[2];

                            string[] vectorParts = coords.Split(',');

                            if (vectorParts.Length == 3)
                            {
                                float x, y, z;
                                if (float.TryParse(vectorParts[0], out x) && float.TryParse(vectorParts[1], out y) && float.TryParse(vectorParts[2], out z))
                                {
                                    // Ahora 'vector' es un Vector3 con los valores de la cadena
                                    Vector3 vector = new Vector3(x, y, z);

                                    Debug.Log($"Jugador {parts[1]} a las coordenadas {parts[2]}");
                                    CmdSendChatMessage($"<color=red>El admin {netId} ha teleportado al jugador {parts[1]} a las coordenadas {vector}</color>", player.namePlayer);
                                }
                                else
                                {
                                    Debug.LogError("Error al convertir la cadena a Vector3.");
                                }
                            }
                        }
                        break;
                    // cmd/ ban (id) (reason)
                    case "ban":
                        if (parts.Length > 2)
                        {
                            // El segundo elemento es el argumento
                            string argument = parts[1];
                            // Lógica basada en el argumento
                            // Ejemplo: Ejecutar una función con el argumento
                            Debug.Log($"Jugador baneado: {parts[1]}  Motivo: {parts[2]}");
                            LogSystem.Log($"{netId}", $"Baneado: {parts[1]}  Motivo: {parts[2]}", "Log.txt");
                            CmdSendChatMessage($"<color=red>El admin {netId} ha baneado al jugador {parts[1]} por {parts[2]}</color>", player.namePlayer);
                        }
                        break;
                    // cmd/ 
                    case "test":
                        if (parts.Length > 1)
                        {

                        }
                        break;
                    default:
                        // Comando desconocido
                        AddMessageToChat("No has ingresado un comando valido.");
                        Debug.Log("No has ingresado un comando valido.");
                        break;
                }
            }
            else
            {
                // Si no es un comando, simplemente muestra el mensaje en el chat
                CmdSendChatMessage("<b>" + player.namePlayer + ":</b> " + message, player.namePlayer);
            }
        }
        else
        {
            AddMessageToChat("<color=red>No puedes usar ese vocabulario.</color>");

            foreach (string word in palabrasEncontradas)
            {
                words = words + " " + word;
            }
            Debug.Log($"El jugador {netId} ha usado:  {words} | Mensaje: {message}");
        }
    }
}
