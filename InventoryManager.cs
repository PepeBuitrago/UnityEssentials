using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class InventoryManager : NetworkBehaviour
{
    public static InventoryManager instance;
    [SerializeField] private Transform content;

    // SyncList para manejar la sincronización automática del inventario
    [SerializeField] private List<ItemClass> poket = new List<ItemClass>();
    private List<ItemClass> itemList = new List<ItemClass>();
    private List<int> itemListCount = new List<int>();


    [SerializeField] private GameObject itemSlot, panelInfo;
    [SerializeField] private TMP_Text ItemName, ItemQuality, ItemDurability, ItemWeight, WeightText, CoinsText;
    [SerializeField] private Button btnUseItem, btnDropItem, btnPanelClose;
    [SerializeField] private TMP_Text btnUseText, btnDropText;
    [SerializeField] private List<Sprite> itemsIcon = new List<Sprite>();

    private float weight = 0;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject DroppedItemPrefab;


    void Awake()
    {
        instance = this;
        player = gameObject;
    }

    public void SetupGUI()
    {
        content = GameObject.Find("InventoryContent").GetComponent<Transform>();
        panelInfo = GameObject.Find("ItemDataPanel");
        ItemName = GameObject.Find("ItemName").GetComponent<TMP_Text>();
        ItemQuality = GameObject.Find("ItemQuality").GetComponent<TMP_Text>();
        ItemWeight = GameObject.Find("ItemWeight").GetComponent<TMP_Text>();
        ItemDurability = GameObject.Find("ItemDurability").GetComponent<TMP_Text>();

        WeightText = GameObject.Find("weightText").GetComponent<TMP_Text>();
        CoinsText = GameObject.Find("coinsText").GetComponent<TMP_Text>();

        btnDropItem = GameObject.Find("ButtonDrop").GetComponent<Button>();
        btnUseItem = GameObject.Find("ButtonUse").GetComponent<Button>();
        btnPanelClose = GameObject.Find("ButtonPanelClose").GetComponent<Button>();

        btnPanelClose.onClick.AddListener(() => {
            PanelInfoExit();
        });

        btnUseText = GameObject.Find("UseItemText").GetComponent<TMP_Text>();
        btnDropText = GameObject.Find("DropItemText").GetComponent<TMP_Text>();

        if(content != null)
        {
            GeneralGui.Instance.ClosePanels();
        }
    }

    public void Add(ItemClass item)
    {
        poket.Add(item);
        ListItems();
    }
    public void RemoveItemByID(int idToRemove)
    {
        ItemClass itemToRemove = poket.FirstOrDefault(item => item.id == idToRemove);

        if (itemToRemove != null)
        {
            poket.Remove(itemToRemove);
        }
    }

    public void RemoveItemByIDAndQuality(int idToRemove, int qualityToRemove)
    {
        ItemClass itemToRemove = poket.FirstOrDefault(item => item.id == idToRemove && item.quality == qualityToRemove);

        if (itemToRemove != null)
        {
            poket.Remove(itemToRemove);
        }
    }

    public List<ItemClass> GetInventory()
    {
        return poket;
    }

    public string GetInventoryString()
    {
        SaveDataInventory inventory = new SaveDataInventory();
        inventory.inventory = poket;
        string json = SaveSystem.ObjectToString(inventory);
        return json;
    }

    public void SetupInventoryString(string list)
    {
        SaveDataInventory inventory = SaveSystem.StringToInventory(list);
        poket = inventory.inventory;
        ListItems();
    }

    public void SetupInventory(List<ItemClass> list)
    {
        poket = list;
        ListItems();
    }

    public void ListItems()
    {
        foreach(Transform item in content)
        {
            Destroy(item.gameObject);
        }
        itemList.Clear();
        itemListCount.Clear();
        foreach(ItemClass item in poket)
        {
            bool exist = false;
            for (int x = 0; x < itemList.Count; x++){
                if (item.stackable)
                {
                    if (item.id == itemList[x].id)
                    {
                        if (item.quality == itemList[x].quality)
                        {
                            exist = true;
                            itemListCount[x]++;
                        }
                    }
                }
            }
            if (!exist)
            {
                itemList.Add(item);
                itemListCount.Add(1);
            }
        }

        int y = 0;
        foreach(ItemClass item in itemList)
        {
            GameObject obj = Instantiate(itemSlot, content);
            obj.GetComponent<Button>().onClick.AddListener(() => { UpdateBtnUseItem(item); PanelInfoEnter(item); });
            var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
            // Carga el ícono desde el archivo usando la ruta almacenada en iconPath
            itemIcon.sprite = itemsIcon[item.id];
            var itemQuality = obj.transform.Find("Quality").GetComponent<TMP_Text>();
            itemQuality.text = item.quality.ToString();
            var itemCount = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            itemCount.text = itemListCount[y].ToString();
            y++;
        }
        CalculateWeight();
        CoinsText.text = GetComponent<PlayerController>().GetCoins().ToString();
    }

    public void UpdateBtnUseItem(ItemClass item)
    {
        btnUseItem.onClick.RemoveAllListeners();

        btnUseItem.onClick.AddListener(() => { 
            InteractItem(item); 
            PanelInfoExit(); 
        });

        btnDropItem.onClick.RemoveAllListeners();

        btnDropItem.onClick.AddListener(() => {
            CmdDropItem(item);
            RemoveItemByIDAndQuality(item.id, item.quality);
            ListItems();
            PanelInfoExit();
        });

        if (item.id == 1 || item.id == 17 || item.id == 27)
        {
            btnUseText.fontSize = 7.5f;
            btnUseText.text = "No puedes usar esto";
        }
        if (item.id == 6 || item.id == 8 || item.id == 9 || item.id == 10 || item.id == 11 || item.id == 12 || item.id == 13 || item.id == 15 || item.id == 16)
        {
            if (!player.GetComponent<PlayerController>().IsTwoHand())
            {
                btnUseText.fontSize = 13f;
                btnUseText.text = "Equipar";
            }
            else
            {
                btnUseText.fontSize = 13f;
                btnUseText.text = "Desequipar";
            }
        }
        if (item.id == 0 || item.id == 18 || item.id == 7 || item.id == 19)
        {
            btnUseText.fontSize = 13f;
            btnUseText.text = "Comer";
        }
        if (item.id == 2 || item.id == 3 || item.id == 4 || item.id == 5 || item.id == 14 || item.id == 28)
        {
            btnUseText.fontSize = 13f;
            btnUseText.text = "Fabricar";
        }
        if (item.id == 29 || item.id == 30)
        {
            btnUseText.fontSize = 13f;
            btnUseText.text = "Beber";
        }
    }

    public void PanelInfoEnter(ItemClass item)
    {
        ItemName.text = item.itemName;
        ItemDurability.text = "Durabilidad: " + item.durability.ToString() + "/100";
        ItemQuality.text = "Calidad: " + item.quality.ToString();
        ItemWeight.text = "Peso: " + item.weight.ToString() + "kg";
        panelInfo.SetActive(true);
        panelInfo.transform.position = Input.mousePosition;
    }

    public void PanelInfoExit()
    {
        panelInfo.SetActive(false);
    }

    public float CalculateWeight()
    {
        weight = 0;
        foreach (var item in poket)
        {
            weight += item.weight;
        }
        WeightText.text = weight + "/100kg";
        return weight;
    }

    public void InteractItem(ItemClass item)
    {
        switch (item.id)
        {
            case 6:
                player.GetComponent<PlayerController>().UpdatePrimitiveTool();
                break;
            case 7:
                player.GetComponent<PlayerController>().Eat(item.durability);
                RemoveItemByIDAndQuality(item.id, item.quality);
                ListItems();
                break;
            case 8:
                player.GetComponent<PlayerController>().UpdateFishingRod();
                break;
            case 9:
                player.GetComponent<PlayerController>().UpdatePrimitiveKnife();
                break;
            case 10:
                player.GetComponent<PlayerController>().UpdateLantern();
                break;
            case 11:
                player.GetComponent<PlayerController>().UpdateIronAxe();
                break;
            case 12:
                player.GetComponent<PlayerController>().UpdateHoe();
                break;
            case 13:
                player.GetComponent<PlayerController>().UpdateMeltingPot();
                break;
            case 15:
                player.GetComponent<PlayerController>().UpdateShovel();
                break;
            case 16:
                player.GetComponent<PlayerController>().UpdateLittlePickaxe();
                break;
            case 18:
                player.GetComponent<PlayerController>().Eat(item.durability);
                RemoveItemByIDAndQuality(item.id, item.quality);
                ListItems();
                break;
            case 19:
                player.GetComponent<PlayerController>().Eat(item.durability);
                RemoveItemByIDAndQuality(item.id, item.quality);
                ListItems();
                break;
            case 29:
                player.GetComponent<PlayerController>().UpdateStamine(item.durability);
                RemoveItemByIDAndQuality(item.id, item.quality);
                ListItems();
                break;
            case 30:
                player.GetComponent<PlayerController>().UpdateHealth(item.durability);
                RemoveItemByIDAndQuality(item.id, item.quality);
                ListItems();
                break;
        }
        if (item.id == 2 || item.id == 3 || item.id == 4 || item.id == 5 || item.id == 14)
        {
            GeneralGui.Instance.UpdatePanelCrafting();
        }
    }

    [Command]
    public void CmdDropItem(ItemClass item)
    {
        DropItem(item);
    }

    public void DropItem(ItemClass item)
    {
        NotificationManager.Instance.Notification("Soltaste " + item.itemName, 5f);
        GameObject newObject = Instantiate(DroppedItemPrefab, player.transform.position, Quaternion.identity);
        newObject.GetComponent<DropedItemController>().Add(item);
        NetworkServer.Spawn(newObject);
    }
}