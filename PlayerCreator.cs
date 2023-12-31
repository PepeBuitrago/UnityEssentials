using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerCreator : NetworkBehaviour
{
    public static PlayerCreator Instance;

    private int healt = 100;
    private int stamine = 100;
    private int hungry = 100;

    private int gender = 1;

    public GameObject[] hairs;
    public GameObject[] facialHairs;
    public GameObject male;
    public GameObject female;

    public Slider hairSlider, FacialHairSlider, colorHairSlider, colorSkinSlider;

    public Button btnSave, btnMale, btnFemale;

    public SavePlayerDataClass characterSave;

    PlayerController player;

    private void Awake()
    {
        Instance = this;
        player = gameObject.GetComponent<PlayerController>();

    }

    private void Start()
    {
    }

    public void SetupGui()
    {
        hairSlider = GameObject.Find("HairSlider").GetComponent<Slider>();
        hairSlider.onValueChanged.AddListener(UpdateHair);
        FacialHairSlider = GameObject.Find("FacialHairSlider").GetComponent<Slider>();
        FacialHairSlider.onValueChanged.AddListener(UpdateFacialHair);
        colorHairSlider = GameObject.Find("ColorHairSlider").GetComponent<Slider>();
        colorHairSlider.onValueChanged.AddListener(UpdateHairColor);
        colorSkinSlider = GameObject.Find("SkinSlider").GetComponent<Slider>();
        colorSkinSlider.onValueChanged.AddListener(UpdateSkinColor);

        btnSave = GameObject.Find("ButtonSaveCharacter").GetComponent<Button>();
        btnSave.onClick.AddListener(() => {
            SaveCharacter();
        });

        btnMale = GameObject.Find("ButtonMale").GetComponent<Button>();
        btnMale.onClick.AddListener(() => {
            UpdateMaleBody();
        });

        btnFemale = GameObject.Find("ButtonFemale").GetComponent<Button>();
        btnFemale.onClick.AddListener(() => {
            UpdateFemaleBody();
        });
    }


    public void SaveCharacter()
    {
        if (isLocalPlayer)
        {
            characterSave = new SavePlayerDataClass();
            characterSave.name = player.namePlayer;
            characterSave.healt = healt;
            characterSave.stamine = stamine;
            characterSave.hungry = hungry;
            characterSave.gender = gender;
            characterSave.hair = (int)hairSlider.value;
            characterSave.facialHair = (int)FacialHairSlider.value;
            characterSave.skin = (int)colorSkinSlider.value;
            characterSave.colorHair = (int)colorHairSlider.value;
            characterSave.nvl = 0;
            characterSave.coins = 0;
            characterSave.ban = false;
            characterSave.admin = -1;
            characterSave.coordX = 0;
            characterSave.coordY = 1;
            characterSave.coordZ = 0;

            CmdSavePlayer(characterSave);

            player.UpdateIsFree(true);
            CmdUpdateGui();
        }
        player.CmdLoadCharacter(characterSave);
    }

    [Command]
    public void CmdSavePlayer(SavePlayerDataClass player)
    {
        SaveSystem.SaveObject(player.name, player, true);
    }


    [Command]
    public void CmdUpdateGui()
    {
        player.RpcCreatorSetActive(false);
    }


    public void UpdateMaleBody()
    {
        female.SetActive(false);
        male.SetActive(true);
        gender = 1;
    }

    public void UpdateFemaleBody()
    {
        female.SetActive(true);
        male.SetActive(false);
        gender = 0;
    }

    public void UpdateHair(float value)
    {
        foreach (GameObject hair in hairs)
        {
            hair.SetActive(false);
        }
        hairs[(int)value].SetActive(true);

    }

    public void UpdateFacialHair(float value)
    {
        foreach (GameObject hair in facialHairs)
        {
            hair.SetActive(false);
        }
        facialHairs[(int)value].SetActive(true);
    }

    public void UpdateSkinColor(float value)
    {
        player.mat.SetColor("_Color_Skin_1", player.colorSkin[(int)value]);
        player.mat.SetColor("_Color_Skin_2", player.colorSkin[(int)value]);
    }
    public void UpdateHairColor(float value)
    {
        player.mat.SetColor("_Color_Hair", player.colorHair[(int)value]);
    }

}
