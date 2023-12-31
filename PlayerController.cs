using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using Mirror;
using System.IO;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : NetworkBehaviour
{
    #region StatsPlayer
    [SerializeField]
    private SavePlayerDataClass characterSave = new SavePlayerDataClass();

    [SyncVar] [SerializeField] public string namePlayer;
    [SyncVar] [SerializeField] private float healt;
    [SyncVar] [SerializeField] private float stamine;
    [SyncVar] [SerializeField] private float hungry;
    [SyncVar] [SerializeField] private float nvl;

    [SyncVar] [SerializeField] private float coins;

    [SyncVar] [SerializeField] private int gender;
    [SyncVar] [SerializeField] private int hair;
    [SyncVar] [SerializeField] private int facialHair;
    [SyncVar] [SerializeField] private int colorHairPlayer;
    [SyncVar] [SerializeField] private int skin;

    [SyncVar] [SerializeField] private bool ban;
    [SyncVar] [SerializeField] private int admin;

    [SyncVar] [SerializeField] public string inventory;
    #endregion

    [SerializeField]
    private GameObject cameraPivot, mainCamera, focusObject;

    [SerializeField]
    private Slider healtSlider, stamineSlider, hungerSlider;

    [SerializeField]
    private TMP_Text nvlTxt, nameTxt;

    [SerializeField]
    private GameObject playerCreator, mainGui;


    [SerializeField]
    private float speedWalk = 3f;
    [SerializeField]
    private float speedRun = 5f;
    [SerializeField]
    private float speedHorseRun = 8f;

    [SerializeField]
    private bool runing, free = true, twoHands, combat, dead, horseRider, creator;

    [SerializeField]
    private Image runingIcon;

    [SerializeField]
    private Sprite runIcon, walkIcon;


    [SerializeField]
    private GameObject[] tools;

    [SerializeField]
    private GameObject[] hairs;
    [SerializeField]
    private GameObject[] facialHairs;

    public GameObject male;
    public GameObject female;

    private Animator animator;

    [Header("Material")]
    public Material mat;

    [Header("Skin Colors")]
    public Color[] colorSkin;

    [Header("Hair Colors")]
    public Color[] colorHair;

    [SerializeField]
    private TwoBoneIKConstraint RightHandBone, LeftHandBone;

    [SerializeField]
    private float velocidadMovimientoZoom = 2.0f, limiteZoomSuperior = -2.5f, limiteZoomInferior = -10.0f;

    [SerializeField]
    private float sensibilidadVertical = 2.0f, sensibilidadHorizontal = 2.0f, limiteSuperiorX = 70.0f, limiteInferiorX = 10.0f, limiteSuperiorY = 360.0f, limiteInferiorY = -360.0f;


    [SerializeField]
    private GameObject horseProp, horsePrefab;
    private HorseDataClass horseData = new HorseDataClass();
    private Animator animatorHorse;

    public Item Bread, potionHealth, potionStamine, goldCoin;

    [SerializeField]
    private AudioSource AxeWood, PickaxeStone, TreeFall, FindingResources;

    [SerializeField]
    private AudioSource GrassRightStep, GrassLeftStep;

    [SerializeField]
    private GameObject MiningFx;

    [SerializeField]
    private List<GameObject> horseSkins = new List<GameObject>();


    [Header("Character Controller")]
    public float moveSpeed = 3.0f;
    public float rotationSpeed = 5.0f;
    public float jumpForce = 8.0f;
    public float gravity = -9.81f;


    private float _verticalVelocity = 0.0f;

    private CharacterController _controller;
    private PlayerCreator _playerCreator;
    private InventoryManager _inventoryManager;
    private ChatManager _chatManager;

    [SerializeField]
    private GameObject WoodPrefab, StonePrefab, StrawPrefab;
    private List<GameObject> resourcesList = new List<GameObject>();

    [SerializeField]
    private GameObject UpRaycast;

    [SerializeField]
    Material[] materialPlayer;

    [SerializeField]
    private List<SkinnedMeshRenderer> materialRenders = new List<SkinnedMeshRenderer>();

    /// <summary>
    /// ----------------------------------------------------------------------------------
    /// </summary>
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _controller.center = new Vector3(0, 1, 0);

        _playerCreator = GetComponent<PlayerCreator>();
        _inventoryManager = GetComponent<InventoryManager>();
        _chatManager = GetComponent<ChatManager>();

        GameObject script = GameObject.Find("GeneralScript");
        script.GetComponent<PanelDataManager>().player = this;
        
        animator = GetComponent<Animator>();

    }

    private void Start()
    {
        AssignMaterialToPlayer((int)netId);
        
        if (isLocalPlayer)
        {
            SetupVars();
            CmdSetupNamePlayer(PlayerPrefs.GetString("PlayerName"));
            GameObject.Find("MapGenerator").GetComponent<TerrainGenerator>().viewer = transform;

            
            CmdFindPlayer();

            //SetupInitialKit();

            //PanelDataManager.Instance.ClosePanelGround();
            //PanelDataManager.Instance.ClosePanelStone();
            //PanelDataManager.Instance.ClosePanelTree();

            //LoadPosition();
            //CmdLoadCharacterData();
        }
        else
        {
            LoadCharacterData(facialHair, hair, colorHairPlayer, skin, gender, namePlayer);
        }
        
    }

    [TargetRpc]
    void RpcSetupInitialKit()
    {
        if (nvl < 1)
        {
            for (int x = 0; x < 5; x++)
            {
                ItemClass itm = Bread.CreateInstance();
                itm.quality = 50;
                GetComponent<InventoryManager>().Add(itm);
            }
            NotificationManager.Instance.Notification("Has recibido 5 de pan.", 5f);
            for (int x = 0; x < 1; x++)
            {
                ItemClass itm = potionStamine.CreateInstance();
                itm.quality = 50;
                GetComponent<InventoryManager>().Add(itm);
            }
            NotificationManager.Instance.Notification("Has recibido 1 pociones de energia.", 5f);
            for (int x = 0; x < 1; x++)
            {
                ItemClass itm = potionHealth.CreateInstance();
                itm.quality = 50;
                GetComponent<InventoryManager>().Add(itm);
            }
            NotificationManager.Instance.Notification("Has recibido 1 pociones de salud.", 5f);
            GetComponent<InventoryManager>().ListItems();
            UpdateNivel(1);
            
            inventory = GetComponent<InventoryManager>().GetInventoryString();
            CmdSaveCharacter();
        }
        UpdateNvlPanel();
        LoadInventory();
    }

    void SetupVars()
    {
        playerCreator = GameObject.Find("CharacterCreator");
        mainGui = GameObject.Find("GeneralGui");

        cameraPivot = GameObject.Find("CameraPivot");
        mainCamera = GameObject.Find("MainCamera");

        healtSlider = GameObject.Find("HealthBar").GetComponent<Slider>();
        stamineSlider = GameObject.Find("StamineBar").GetComponent<Slider>();
        hungerSlider = GameObject.Find("HungerBar").GetComponent<Slider>();

        runingIcon = GameObject.Find("RuningIcon").GetComponent<Image>();
        nvlTxt = GameObject.Find("NvlText").GetComponent<TMP_Text>();

        AxeWood = GameObject.Find("AudioAxe").GetComponent<AudioSource>();
        PickaxeStone = GameObject.Find("AudioPickaxe").GetComponent<AudioSource>();
        TreeFall = GameObject.Find("AudioTreeFall").GetComponent<AudioSource>();
        FindingResources = GameObject.Find("AudioPlantGrow").GetComponent<AudioSource>();
        GrassRightStep = GameObject.Find("GrassRightStep").GetComponent<AudioSource>();
        GrassLeftStep = GameObject.Find("GrassLeftStep").GetComponent<AudioSource>();

        _playerCreator.SetupGui();
        _inventoryManager.SetupGUI();
        //_chatManager.SetupGUI();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        //Actualizar movimiento yanimaciones 
        if (free && !dead)
        {
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");


            //Mover jugador
            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 desiredMoveDirection = forward * verticalInput + right * horizontalInput;

            _verticalVelocity += gravity * Time.deltaTime;

            Vector3 gravityVector = Vector3.down * _verticalVelocity;

            Vector3 finalMove = desiredMoveDirection * moveSpeed + gravityVector;

            _controller.Move(finalMove * Time.deltaTime);



            if (desiredMoveDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(desiredMoveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            }

            // Envía la información de movimiento al servidor
            CmdMovePlayer(transform.position, transform.rotation);

            CmdUpdateAnimation(verticalInput, horizontalInput, runing);

        }



        if (Input.GetKey(KeyCode.F))
        {
            DownHorse();
        }

        if (Input.GetButtonDown("Fire3"))
        {
            if (!dead && !creator)
            {
                if (stamine > 10)
                {
                    runingIcon.sprite = runIcon;
                    runing = true;
                    if (horseRider)
                    {
                        moveSpeed = speedHorseRun;
                    }
                    else
                    {
                        moveSpeed = speedRun;
                    }
                }
                else
                {
                    runingIcon.sprite = walkIcon;
                    runing = false;

                    moveSpeed = speedWalk;
                }
            }
            
        }

        if (Input.GetButton("Fire3"))
        {
            if (!dead && !creator)
            {
                if (stamine < 10)
                {
                    runingIcon.sprite = walkIcon;
                    runing = false;
                    moveSpeed = speedWalk;
                }
                if (stamine > 99)
                {
                    runingIcon.sprite = runIcon;
                    runing = true;
                    if (horseRider)
                    {
                        moveSpeed = speedHorseRun;
                    }
                    else
                    {
                        moveSpeed = speedRun;
                    }
                }
            }
        }

        if (Input.GetButtonUp("Fire3"))
        {
            if (!dead && !creator)
            {
                runingIcon.sprite = walkIcon;
                runing = false;
                moveSpeed = speedWalk;
            }
        }

        //Mover la camara
        if (Input.GetButton("Fire2"))
        {
            Cursor.visible = false;
            
            float movimientoVertical = Input.GetAxis("Mouse Y");
            float movimientoHorizontal = Input.GetAxis("Mouse X");

            // Calcular el cambio de rotación en el eje X basado en el movimiento vertical del mouse
            float cambioRotacionX = -movimientoVertical * sensibilidadVertical;
            float nuevaRotacionX = cameraPivot.transform.eulerAngles.x + cambioRotacionX;
            nuevaRotacionX = Mathf.Clamp(nuevaRotacionX, limiteInferiorX, limiteSuperiorX);

            // Calcular el cambio de rotación en el eje Y basado en el movimiento horizontal del mouse
            float cambioRotacionY = movimientoHorizontal * sensibilidadHorizontal;
            float nuevaRotacionY = cameraPivot.transform.eulerAngles.y + cambioRotacionY;
            nuevaRotacionY = Mathf.Clamp(nuevaRotacionY, limiteInferiorY, limiteSuperiorY);

            // Aplicar la rotación al GameObject
            cameraPivot.transform.rotation = Quaternion.Euler(nuevaRotacionX, nuevaRotacionY, transform.eulerAngles.z);
        }

        //Ocultar cursor al mover la camara
        if (Input.GetButtonUp("Fire2"))
        {
            Cursor.visible = true;
        }
        

        //Ataque
        if (Input.GetButtonDown("Fire1") && combat)
        {
            animator.SetTrigger("Attack");
            //UpdateIsFree(false);
            StartCoroutine(AttackDelay());
        }

        //Interaccion con el entorno
        if (Input.GetButtonDown("Fire1") && !Input.GetButton("Fire2") && !combat)
        {
            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            if (!dead && !creator)
            { 
                if (!combat) 
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.tag == "Tree")
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 2f)
                            {
                                if (!horseRider)
                                    PanelDataManager.Instance.OpenPanelTree(hit.transform);
                            }
                        }

                        if (hit.transform.tag == "Rock")
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 2f)
                            {
                                if (!horseRider)
                                    PanelDataManager.Instance.OpenPanelStone(hit.transform);
                            }
                        }

                        if (hit.transform.tag == "Resource")
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 2f)
                            {
                                if (!horseRider)
                                {
                                    animator.SetBool("Finding", true);
                                    UpdateIsFree(false);
                                    StartCoroutine(AnimationDelayPickupResource(1f, hit));
                                }
                                    
                            }
                            else
                            {
                                NotificationManager.Instance.Notification("Debes estar mas cerca para recoger esto.", 5f);
                            }
                        }

                        if (hit.transform.tag == "Item")
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 2f)
                            {
                                CmdChangeAutority(connectionToClient, hit.transform.GetComponent<NetworkIdentity>());

                                List<ItemClass> itemsDropped;
                                //if(hit.transform.GetComponent<NetworkIdentity>().)
                                hit.transform.GetComponent<DropedItemController>().Pickup(out itemsDropped);

                                foreach(ItemClass item in itemsDropped)
                                {
                                    GetComponent<InventoryManager>().Add(item);
                                    NotificationManager.Instance.Notification("Has recogido " + item.itemName + ".", 5f);
                                }
                                CmdSaveCharacter();
                            }
                            else
                            {
                                NotificationManager.Instance.Notification("Debes estar mas cerca para recoger esto.", 5f);
                            }
                        }

                        if (hit.transform.tag == "Horse")
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 2f) 
                            { 
                                horseData.name = hit.transform.GetComponent<HorseController>().horseName;
                                horseData.id = hit.transform.GetComponent<HorseController>().id;
                                ClimbHorse(horseData.id);
                                hit.transform.GetComponent<HorseController>().Destroy();
                            }
                        }

                        if (hit.transform.tag == "Forge")
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 2f)
                            {
                                GeneralGui.Instance.UpdatePanelForge();
                            }
                        }

                        if (hit.transform.tag == "Foundry")
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 2f)
                            {
                                GeneralGui.Instance.UpdatePanelFoundry();
                            }
                        }

                        if (hit.transform.tag == "Kilm")
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 2f)
                            {
                                GeneralGui.Instance.UpdatePanelKilm();
                            }
                        }

                        if (hit.transform.tag == "Ground")
                        {
                            if (!isOverUI && !free)
                            {
                                PanelDataManager.Instance.ClosePanelStone();
                                PanelDataManager.Instance.ClosePanelTree();
                            }
                            if (!isOverUI && !horseRider)
                            {
                                PanelDataManager.Instance.OpenPanelGround();
                            }
                        }
                    }
                }
            }
        }

        //Actualizar GUI
        UpdateSliders();


        //Movimiento de camara
        cameraPivot.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        //Zoom de la camara
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Mover el GameObject de manera relativa en el eje Z
        mainCamera.transform.Translate(0f, 0f, scroll * velocidadMovimientoZoom, Space.Self);

        // Limitar la posición en el eje Z dentro del rango especificado
        Vector3 posicionLocal = mainCamera.transform.localPosition;
        posicionLocal.z = Mathf.Clamp(posicionLocal.z, limiteZoomInferior, limiteZoomSuperior);
        mainCamera.transform.localPosition = posicionLocal;
    }

    [Command]
    private void CmdMovePlayer(Vector3 position, Quaternion rotation)
    {
        RpcUpdatePlayerMovement(position, rotation);
    }

    [ClientRpc]
    private void RpcUpdatePlayerMovement(Vector3 position, Quaternion rotation)
    {
        if (isLocalPlayer)
            return;

        // Actualiza la posición y rotación del jugador en el cliente remoto
        transform.position = position;
        transform.rotation = rotation;
    }

    [Command]
    private void CmdUpdateAnimation(float verticalInput, float horizontalInput, bool isRunning)
    {
        RpcUpdateAnimation(verticalInput, horizontalInput, isRunning);
    }

    [ClientRpc]
    private void RpcUpdateAnimation(float verticalInput, float horizontalInput, bool isRunning)
    {
        // Actualizar la animación en todos los clientes
        float speed = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f ? 1f : 0f;
        if (!isRunning) speed /= 2f;

        animator.SetFloat("Speed", speed);

        if (animatorHorse != null)
        {
            animatorHorse.SetFloat("Speed_f", speed);
        }
    }

    void OnApplicationQuit()
    {
        inventory = GetComponent<InventoryManager>().GetInventoryString();
        if (isLocalPlayer && characterSave.nvl >= 1)
        {
            //CmdSaveCharacter();
        }
    }

    [Command]
    public void CmdSaveCharacter()
    {
        inventory = GetComponent<InventoryManager>().GetInventoryString();
        SetupSaveData();
        
        SaveSystem.SaveObject(namePlayer, characterSave, true);
        Debug.Log("PlayerData Saved!");
    }

    [Command]
    public void CmdLoadCharacter(SavePlayerDataClass data)
    {
        Debug.Log("Se ha ejecutado LoadCharacter Data.name: " + data.name);
        characterSave = data;
        SetupPlayerData();
        CmdSaveCharacter();
        RpcLoadCharacterData(facialHair, hair, colorHairPlayer, skin, gender, namePlayer);
        RpcCreatorSetActive(false);
        RpcSetupInitialKit();
    }

    
    public void CheckDataPlayer()
    {
        Debug.Log("nvl: " + nvl);
        if (nvl != -1)
        {
            RpcCreatorSetActive(false);
            RpcLoadCharacterData(facialHair, hair, colorHairPlayer, skin, gender, namePlayer);
            RpcSetupInitialKit();
        }
        else
        {
            //Crear player
            RpcCreatorSetActive(true);
            UpdateIsFree(false);
        }
        //SetupInitialKit();
    }


    [Command]
    void CmdFindPlayer()
    {
        nvl = -1;
        if (PlayerExist(namePlayer))
        {
            characterSave = SaveSystem.LoadObject<SavePlayerDataClass>(namePlayer);
            SetupPlayerData();
            Debug.Log("El jugador esta registrado.");
            Debug.Log("nvl: " + nvl);
        }
        else
        {
            Debug.Log("El jugador no esta registrado.");
            Debug.Log("nvl: " + nvl);
        }
        CheckDataPlayer();
    }


    public void SetupPlayerData()
    {
        healt = characterSave.healt;
        stamine = characterSave.stamine;
        hungry = characterSave.hungry;
        nvl = characterSave.nvl;
        coins = characterSave.coins;

        hair = characterSave.hair;
        facialHair = characterSave.facialHair;
        colorHairPlayer = characterSave.colorHair;
        skin = characterSave.skin;
        gender = characterSave.gender;

        ban = characterSave.ban;
        admin = characterSave.admin;

        inventory = characterSave.inventoryData;
    }

    public void SetupSaveData()
    {
        characterSave.name = namePlayer;
        characterSave.healt = healt;
        characterSave.stamine = stamine;
        characterSave.hungry = hungry;
        characterSave.nvl = nvl;
        characterSave.coins = coins;

        characterSave.hair = hair;
        characterSave.facialHair = facialHair;
        characterSave.colorHair = colorHairPlayer;
        characterSave.skin = skin;
        characterSave.gender = gender;

        characterSave.ban = ban;
        characterSave.admin = admin;

        characterSave.inventoryData = inventory; 

        characterSave.coordX = transform.position.x;
        characterSave.coordY = transform.position.y;
        characterSave.coordZ = transform.position.z;
    }

    public void LoadPosition()
    {
        gameObject.transform.position = new Vector3(characterSave.coordX, characterSave.coordY + 10, characterSave.coordZ);
    }

    public void LoadInventory()
    {
        GetComponent<InventoryManager>().SetupInventory(SaveSystem.StringToInventory(inventory).inventory);
    }

    public void UpdateNvlPanel()
    {
        nvlTxt.text = nvl.ToString();
    }

    public void UpdateSliders()
    {
        UpdateStamine();
        CmdUpdateHungry();
        healtSlider.value = healt;
        stamineSlider.value = stamine;
        hungerSlider.value = hungry;
    }

    public void UpdateHair(int id)
    {
        foreach (GameObject hair in hairs)
        {
            hair.SetActive(false);
        }
        hairs[id].SetActive(true);
    }

    public void UpdateFacialHair(int id)
    {
        foreach (GameObject hair in facialHairs)
        {
            hair.SetActive(false);
        }
        facialHairs[id].SetActive(true);
    }

    public void UpdateColorHair(int id)
    {
        mat.SetColor("_Color_Hair", colorHair[id]);
    }

    public void UpdateColorSkin(int id)
    {
        mat.SetColor("_Color_Skin_1", colorSkin[id]);
        mat.SetColor("_Color_Skin_2", colorSkin[id]);
    }

    public void UpdateGender(int id)
    {
        if(id == 0)
        {
            female.SetActive(true);
            male.SetActive(false);
        }
        if (id == 1)
        {
            female.SetActive(false);
            male.SetActive(true);
        }
    }

    #region Activities

    public void ChopTree()
    {
        if (animator.GetBool("TwoHands"))
        {
            PanelDataManager.Instance.ClosePanelTree();
            transform.LookAt(focusObject.transform.position);
            UpdateIsFree(false);
            animator.SetBool("Axe_Chop", true);
            StartCoroutine(AnimationDelayChopTree());
        }
        else
        {
            NotificationManager.Instance.Notification("Necesitas una hacha para realizar esto.", 5f);
        }
    }

    public void Mining()
    {
        if (twoHands)
        {
            PanelDataManager.Instance.ClosePanelStone();
            transform.LookAt(focusObject.transform.position);
            UpdateIsFree(false);
            animator.SetBool("Pickaxe_Mining", true);
            StartCoroutine(AnimationDelayMining());
        }
        else
        {
            NotificationManager.Instance.Notification("Necesitas un pico para realizar esto.", 5f);
        }
    }
    #endregion

    void UpdateStamine()
    {
        if (runing)
        {
            if (stamine > 0)
                stamine -= 0.1f;
        }
        else
        {
            if (stamine < 100)
                stamine += 0.05f;
        }
    }

    [Command]
    void CmdUpdateHungry()
    {
        if (!dead && !creator)
        {
            if (hungry > 0)
            {
                if (runing)
                {
                    if (stamine > 10) hungry -= 0.001f;//Si esta corriendo gasta mas
                }
                else
                {
                    hungry -= 0.0001f;
                }
            }
            else
            {
                CmdDamage(0.05f);
            }
        }
    }

    [Command]
    public void CmdDamage(float damage)
    {
        Damage(damage);
    }

    [Server]
    public void Damage(float damage)
    {
        if (healt > 0)
        {
            healt -= damage;
        }
    }

    public void UpdateAnimationDamage( bool isHit)
    {
        if (isHit)
        {
            animator.SetTrigger("Damage");
            StartCoroutine(DamageDelay());
        }
        else
        {
            animator.SetTrigger("Killed");
            StartCoroutine(DeadDelay());
        }
    }

    [Server]
    public void UpdateHealth(float health)
    {
        healt += health;
        if (healt > 100) healt = 100;
    }

    [TargetRpc]
    public void RpcCreatorSetActive(bool i)
    {
        if (isLocalPlayer)
        {
            playerCreator.SetActive(i);
            mainGui.SetActive(!i);
            creator = i;
            _playerCreator.UpdateHair(0);
            _playerCreator.UpdateFacialHair(0);
            Debug.Log("Character creator: " + i);
        }
    }

    public void UpdateFocusObject(GameObject obj)
    {
        focusObject = obj;
    }

    public void UpdateIsFree(bool i)
    {
        //Debug.Log("UpdateIsFree: " + i);
        free = i;
    }

    public int GetAdminNvl()
    {
        return characterSave.admin;
    }

    public bool IsRuning()
    {
        return runing;
    }

    public bool IsTwoHand()
    {
        return twoHands;
    }

    public int GetCoins()
    {
        return (int)coins;
    }
    #region UpdateTools
    public void UpdatePrimitiveTool()
    {
        if (twoHands)
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 0;
            RightHandBone.data.target = null;
            twoHands = false;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
        }
        else
        {
            foreach(var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[0].SetActive(true);
            twoHands = true;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
            GeneralGui.Instance.ClosePanels();
        }
    }

    public void UpdateFishingRod()
    {
        if (twoHands)
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 0;
            RightHandBone.data.target = null;
            twoHands = false;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
        }
        else
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[1].SetActive(true);
            twoHands = true;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
            GeneralGui.Instance.ClosePanels();
        }
    }

    public void UpdatePrimitiveKnife()
    {
        if (combat)
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 0;
            RightHandBone.data.target = null;
            combat = false;
            runing = false;
            animator.SetBool("TwoHands", false);
            animator.SetBool("Combat", combat);
        }
        else
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[2].SetActive(true);
            combat = true;
            runing = true;
            animator.SetBool("TwoHands", false);
            animator.SetBool("Combat", combat);
            GeneralGui.Instance.ClosePanels();
        }
    }

    public void UpdateLantern()
    {
        if (twoHands)
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 0;
            RightHandBone.data.target = null;
            twoHands = false;
        }
        else
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 1;
            RightHandBone.data.target = tools[3].transform;
            twoHands = true;
            tools[3].SetActive(true);
            GeneralGui.Instance.ClosePanels();
        }
    }

    public void UpdateHoe()
    {
        if (twoHands)
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 0;
            RightHandBone.data.target = null;
            twoHands = false;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
        }
        else
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[4].SetActive(true);
            twoHands = true;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
            GeneralGui.Instance.ClosePanels();
        }
    }

    public void UpdateIronAxe()
    {
        if (twoHands)
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 0;
            RightHandBone.data.target = null;
            twoHands = false;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
        }
        else
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[5].SetActive(true);
            twoHands = true;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
            GeneralGui.Instance.ClosePanels();
        }
    }

    public void UpdateMeltingPot()
    {
        if (twoHands)
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 0;
            RightHandBone.data.target = null;
            twoHands = false;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
        }
        else
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[6].SetActive(true);
            twoHands = true;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
            GeneralGui.Instance.ClosePanels();
        }
    }

    public void UpdateShovel()
    {
        if (twoHands)
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 0;
            RightHandBone.data.target = null;
            twoHands = false;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
        }
        else
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[7].SetActive(true);
            twoHands = true;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
            GeneralGui.Instance.ClosePanels();
        }
    }

    public void UpdateLittlePickaxe()
    {
        if (twoHands)
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            RightHandBone.weight = 0;
            RightHandBone.data.target = null;
            twoHands = false;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", twoHands);
        }
        else
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[8].SetActive(true);
            twoHands = true;
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", false);
            GeneralGui.Instance.ClosePanels();
        }
    }
    #endregion

    #region Update Stats
    public void Eat(float food)
    {
        hungry += food;
        if(hungry > 100) hungry = 100;
    }

    public void UpdateStamine(float sta)
    {
        stamine += sta;
        if (stamine > 100) stamine = 100;
    }


    public void UpdateNivel(float f)
    {
        nvl += f;
        UpdateNvlPanel();
    }
    #endregion

    #region Horse
    public void ClimbHorse(int id)
    {
        if (!horseRider)
        {
            horseRider = true;
            twoHands = false;
            _controller.center = new Vector3(0, 0.3f, 0);
            moveSpeed = 8f;
            foreach (GameObject skin in horseSkins)
            {
                skin.SetActive(false);
            }
            horseSkins[id].SetActive(true);
            SetupHorseProp(true);
            if (animatorHorse == null) animatorHorse = horseProp.GetComponent<Animator>();
            animator.SetBool("Horse", true);
            animator.SetBool("Combat", false);
            animator.SetBool("TwoHands", false);
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
        }
    }

    public void DownHorse()
    {
        if (horseRider)
        {
            horseRider = false;
            SetupHorseProp(false);
            _controller.center = new Vector3(0, 1, 0);
            GameObject horse = Instantiate(horsePrefab, gameObject.transform.position, Quaternion.identity);
            horse.GetComponent<HorseController>().id = horseData.id;
            horse.GetComponent<HorseController>().horseName = horseData.name;
            //horse.GetComponent<HorseController>().Tame();
            //horse.GetComponent<HorseController>().SetupRider(gameObject);
            animator.SetBool("Horse", false);
            moveSpeed = 5f;
        }
    }

    void SetupHorseProp(bool t)
    {
        horseProp.SetActive(t);
    }
    #endregion

    #region FindResources
    public void FindInGround()
    {
        animator.SetBool("TwoHands", false);
        animator.SetBool("Finding", true);
        FindingResources.Play();
        PanelDataManager.Instance.ClosePanelGround();
        PanelDataManager.Instance.ClosePanelStone();
        PanelDataManager.Instance.ClosePanelTree();
        UpdateIsFree(false);
        StartCoroutine(AnimationDelayFinding(2f));
    }

    public void FindGroundResources()
    {

        foreach(GameObject obj in resourcesList)
        {
            Destroy(obj);
        }

        resourcesList.Clear();

        int resources = Random.Range(1, 10);
        //int resources = 10;
        RaycastHit hit;

        for (int x = 0; x < resources; x++)
        {
            int random = Random.Range(1, 4);

            if ( random == 1)
            {
                // Lanzar un rayo desde la posición del GameObject en la dirección de targetPosition
                if (Physics.Raycast(UpRaycast.transform.position, GenerarPosicionAleatoria(transform.position, 8f) - UpRaycast.transform.position, out hit))
                {
                    // Instanciar el prefab en la posición del golpe del rayo
                    if (hit.transform.tag == "Ground") { GameObject newObject = Instantiate(WoodPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); resourcesList.Add(newObject); }
                }
            }
            if (random == 2)
            {
                // Lanzar un rayo desde la posición del GameObject en la dirección de targetPosition
                if (Physics.Raycast(UpRaycast.transform.position, GenerarPosicionAleatoria(transform.position, 8f) - UpRaycast.transform.position, out hit))
                {
                    // Instanciar el prefab en la posición del golpe del rayo
                    if (hit.transform.tag == "Ground") { GameObject newObject = Instantiate(StonePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); resourcesList.Add(newObject); }
                }
            }
            else
            {
                // Lanzar un rayo desde la posición del GameObject en la dirección de targetPosition
                if (Physics.Raycast(UpRaycast.transform.position, GenerarPosicionAleatoria(transform.position, 8f) - UpRaycast.transform.position, out hit))
                {
                    // Instanciar el prefab en la posición del golpe del rayo
                    if (hit.transform.tag == "Ground") { GameObject newObject = Instantiate(StrawPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); resourcesList.Add(newObject); }
                }
            }
        }

        PanelDataManager.Instance.ClosePanelGround();
    }

    public Vector3 GenerarPosicionAleatoria(Vector3 posicionPersonaje, float radio)
    {
        // Generar coordenadas aleatorias dentro del rango especificado
        float offsetX = Random.Range(-radio, radio);
        float offsetZ = Random.Range(-radio, radio);

        // Calcular la nueva posición aleatoria
        Vector3 nuevaPosicion = new Vector3(posicionPersonaje.x + offsetX, posicionPersonaje.y, posicionPersonaje.z + offsetZ);

        return nuevaPosicion;
    }

    public void PickupResource(RaycastHit hit)
    {
        hit.transform.GetComponent<ResourcesController>().Pickup();
        CmdSaveCharacter();
    }
    #endregion

    #region Sound
    public void PlayAxeSound()
    {
        AxeWood.Play();
    }

    public void PlayPickaxeSound()
    {
        PickaxeStone.Play();
        MiningFx.SetActive(true);
    }

    public void PlayLeftStepSound()
    {
        GrassLeftStep.Play();
    }

    public void PlayRightStepSound()
    {
        GrassRightStep.Play();
    }
    #endregion



    #region Coroutines
    IEnumerator AnimationDelayChopTree()
    {
        yield return new WaitForSeconds(6f);
        animator.SetBool("Axe_Chop", false);
        UpdateIsFree(true);
        TreeFall.Play();
        focusObject.transform.gameObject.GetComponent<TreeController>().Cut();
    }

    IEnumerator AnimationDelayMining()
    {
        yield return new WaitForSeconds(8f);
        animator.SetBool("Pickaxe_Mining", false);
        MiningFx.SetActive(false);
        UpdateIsFree(true);
        focusObject.transform.gameObject.GetComponent<StoneController>().Mining();
    }

    IEnumerator DeadDelay()
    {
        yield return new WaitForSeconds(.1f);
        dead = true;
        UpdateIsFree(false);
        animator.SetBool("Dead", true);
        animator.ResetTrigger("Killed");
    }

    IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(.1f);
        animator.ResetTrigger("Damage");
    }

    IEnumerator AnimationDelayFinding(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("Finding", false);
        UpdateIsFree(true);
        FindGroundResources();
    }

    IEnumerator AnimationDelayPickupResource(float time, RaycastHit hit)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("Finding", false);
        UpdateIsFree(true);
        PickupResource(hit);
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(.1f);
        animator.ResetTrigger("Attack");
    }

    #endregion


    [Command]
    public void CmdLoadCharacterData(int facialHair, int hair, int colorHair, int skin, int gender, string name) 
    {
        RpcLoadCharacterData(facialHair, hair, colorHair, skin, gender, name);
    }


    [ClientRpc]
    public void RpcLoadCharacterData(int facialHair, int hair, int colorHair, int skin, int gender, string name)
    {
        UpdateFacialHair(facialHair);
        UpdateHair(hair);
        UpdateColorHair(colorHair);
        UpdateColorSkin(skin);
        UpdateGender(gender);
        nameTxt.text = name;
        Debug.Log($"Nombre: {name} facial hair: {facialHair} hair: {hair} color hair: {colorHair} skin: {skin} gender: {gender}");
    }

    public void LoadCharacterData(int facialHair, int hair, int colorHair, int skin, int gender, string name)
    {
        UpdateFacialHair(facialHair);
        UpdateHair(hair);
        UpdateColorHair(colorHair);
        UpdateColorSkin(skin);
        UpdateGender(gender);
        nameTxt.text = name;
        Debug.Log($"Nombre: {name} facial hair: {facialHair} hair: {hair} color hair: {colorHair} skin: {skin} gender: {gender}");
    }

    public bool PlayerExist(string name)
    {
        return SaveSystem.FileExists(name);
    }

    void AssignMaterialToPlayer(int i)
    {
        // Asigna un material al Renderer del jugador
        mat = materialPlayer[i];
        foreach(SkinnedMeshRenderer mesh in materialRenders)
        {
            mesh.material = mat;
        }
        Debug.Log("Material " + i + " asignado al player " + netId);
    }

    [Command]
    public void CmdSetupNamePlayer(string newName)
    {
        namePlayer = newName;
    }

    [Command]
    public void CmdChangeAutority(NetworkConnectionToClient client, NetworkIdentity identity)
    {
        identity.AssignClientAuthority(client);
    }
}

public class HorseDataClass
{
    public int id;
    public string name;
}