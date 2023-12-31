using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{

    public TMP_InputField namePlayer;

    [SerializeField] ServerConfiguration serverConfig;

    public void Load(string nameScene)
    {
        if(namePlayer.text != "")
        {
            serverConfig.playerName = namePlayer.text;
            PlayerPrefs.SetString("PlayerName", namePlayer.text);

            if (nameScene == "Multiplayer0")
            {
                PlayerPrefs.SetInt("MultiplayerTipe", 0);
                SceneManager.LoadScene("Multiplayer", LoadSceneMode.Single);
            }
            if (nameScene == "Multiplayer1")
            {
                PlayerPrefs.SetInt("MultiplayerTipe", 1);
                SceneManager.LoadScene("Multiplayer", LoadSceneMode.Single);
            }
            if (nameScene == "Multiplayer2")
            {
                PlayerPrefs.SetInt("MultiplayerTipe", 2);
                SceneManager.LoadScene("Multiplayer", LoadSceneMode.Single);
            }
        }
        else
        {
            NotificationManager.Instance.Notification("Debes agregar un nombre primero.", 5f);
            Debug.Log("Debes agregar un nombre primero.");
        }
    }

    public void Quit()
    {
        Application.Quit();
        //EditorApplication.isPlaying = false;
    }


}
