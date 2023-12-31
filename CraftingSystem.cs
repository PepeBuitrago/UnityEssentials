using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<ItemClass> inventory = new List<ItemClass>();
    [SerializeField] private List<ItemClass> inventoryList = new List<ItemClass>();
    [SerializeField] private List<int> inventoryCount = new List<int>();
    [SerializeField] private Button btnRope, btnPrimitiveTool, btnPrimitiveKnife, btnFishingRod, btnBrick, btnBucket, btnIronAxe, btnHoe, btnShovel, btnPickaxe, btnMeltingPot, btnLantern, btnIronIngot, btnAnvil, btnBell, btnCharcoal, btnGlass;
    private int piecePrimitiveTool, piecePrimitiveKnife, pieceFishingRod, pieceBucket, pieceIronAxe, pieceHoe, pieceShovel, piecePickaxe;

    [SerializeField]
    private AudioSource frogeCraftingSound;

    public static CraftingSystem Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    public void UpdateButtonsState()
    {
        UpdateInventoryList();
        SetupLists();
        ResetButtons();

        //Rope
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 4 && inventoryCount[x] >= 5)
            {
                btnRope.interactable = true;
            }
        }

        //PrimitveTool
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 2 && inventoryCount[x] >= 10)
            {
                piecePrimitiveTool++;
            }
        }
        for (int y = 0; y < inventoryList.Count; y++)
        {
            if (inventoryList[y].id == 3 && inventoryCount[y] >= 1)
            {
                piecePrimitiveTool++;
            }
        }

        if (piecePrimitiveTool == 2)
        {
            btnPrimitiveTool.interactable = true;
        }

        //PrimitveKnife
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 2 && inventoryCount[x] >= 5)
            {
                piecePrimitiveKnife++;
            }
        }
        for (int y = 0; y < inventoryList.Count; y++)
        {
            if (inventoryList[y].id == 3 && inventoryCount[y] >= 2)
            {
                piecePrimitiveKnife++;
            }
        }

        if (piecePrimitiveKnife == 2)
        {
            btnPrimitiveKnife.interactable = true;
        }

        //Fishrod
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 2 && inventoryCount[x] >= 15)
            {
                pieceFishingRod++;
            }
        }
        for (int y = 0; y < inventoryList.Count; y++)
        {
            if (inventoryList[y].id == 5 && inventoryCount[y] >= 1)
            {
                pieceFishingRod++;
            }
        }

        if (pieceFishingRod == 2)
        {
            btnFishingRod.interactable = true;
        }

        //Brick
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 3 && inventoryCount[x] >= 5)
            {
                btnBrick.interactable = true;
            }
        }

        //Bucket
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 2 && inventoryCount[x] >= 15)
            {
                pieceBucket++;
            }
        }
        for (int y = 0; y < inventoryList.Count; y++)
        {
            if (inventoryList[y].id == 5 && inventoryCount[y] >= 1)
            {
                pieceBucket++;
            }
        }
        for (int y = 0; y < inventoryList.Count; y++)
        {
            if (inventoryList[y].id == 14 && inventoryCount[y] >= 1)
            {
                pieceBucket++;
            }
        }

        if (pieceBucket == 3)
        {
            btnBucket.interactable = true;
        }

        //IronAxe
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 2 && inventoryCount[x] >= 15)
            {
                pieceIronAxe++;
            }
        }
        for (int y = 0; y < inventoryList.Count; y++)
        {
            if (inventoryList[y].id == 14 && inventoryCount[y] >= 1)
            {
                pieceIronAxe++;
            }
        }

        if (pieceIronAxe == 2)
        {
            btnIronAxe.interactable = true;
        }

        //Hoe
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 2 && inventoryCount[x] >= 15)
            {
                pieceHoe++;
            }
        }
        for (int y = 0; y < inventoryList.Count; y++)
        {
            if (inventoryList[y].id == 14 && inventoryCount[y] >= 1)
            {
                pieceHoe++;
            }
        }

        if (pieceHoe == 2)
        {
            btnHoe.interactable = true;
        }

        //Shovel
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 2 && inventoryCount[x] >= 15)
            {
                pieceShovel++;
            }
        }
        for (int y = 0; y < inventoryList.Count; y++)
        {
            if (inventoryList[y].id == 14 && inventoryCount[y] >= 1)
            {
                pieceShovel++;
            }
        }

        if (pieceShovel == 2)
        {
            btnShovel.interactable = true;
        }

        //Pickaxe
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 2 && inventoryCount[x] >= 15)
            {
                piecePickaxe++;
            }
        }
        for (int y = 0; y < inventoryList.Count; y++)
        {
            if (inventoryList[y].id == 14 && inventoryCount[y] >= 2)
            {
                piecePickaxe++;
            }
        }

        if (piecePickaxe == 2)
        {
            btnPickaxe.interactable = true;
        }

        //MeltingPot
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 14 && inventoryCount[x] >= 5)
            {
                btnMeltingPot.interactable = true;
            }
        }

        //Lantern
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 14 && inventoryCount[x] >= 2)
            {
                btnLantern.interactable = true;
            }
        }

        //IronIngot
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 17 && inventoryCount[x] >= 5)
            {
                btnIronIngot.interactable = true;
            }
        }

        //Anvil
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 14 && inventoryCount[x] >= 25)
            {
                btnAnvil.interactable = true;
            }
        }

        //Bell
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 14 && inventoryCount[x] >= 10)
            {
                btnBell.interactable = true;
            }
        }

        //Charcoal
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 2 && inventoryCount[x] >= 2)
            {
                btnCharcoal.interactable = true;
            }
        }

        //Glass
        for (int x = 0; x < inventoryList.Count; x++)
        {
            if (inventoryList[x].id == 27 && inventoryCount[x] >= 5)
            {
                btnGlass.interactable = true;
            }
        }
    }

    public void CraftingRope()
    {
        UpdateInventoryList();
        float quality = CalculateAverageQualityForRecipe(4, 5);
        if (quality != 0)
        {
            ItemClass itm = items[5].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);
            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 cuerda.", 5f);
    }

    
    public void CraftingPrimitiveTool()
    {
        UpdateInventoryList();
        float quality = 0;

        quality += CalculateAverageQualityForRecipe(2, 10);
        quality += CalculateAverageQualityForRecipe(3, 1);

        quality = quality / 2;

        if (quality != 0)
        {
            ItemClass itm = items[6].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);

            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 herramienta primitiva.", 5f);
    }

    public void CraftingPrimitiveKnife()
    {
        UpdateInventoryList();
        float quality = 0;

        quality += CalculateAverageQualityForRecipe(2, 5);
        quality += CalculateAverageQualityForRecipe(3, 2);

        quality = quality / 2;

        if (quality != 0)
        {
            ItemClass itm = items[9].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);

            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 cuchillo primitivo.", 5f);
    }

    public void CraftingFishingRod()
    {
        UpdateInventoryList();
        float quality = 0;

        quality += CalculateAverageQualityForRecipe(2, 10);
        quality += CalculateAverageQualityForRecipe(5, 1);

        quality = quality / 2;

        if (quality != 0)
        {
            ItemClass itm = items[8].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);

            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 caña de pescar.", 5f);
    }

    public void CraftingBrick()
    {
        UpdateInventoryList();
        float quality = CalculateAverageQualityForRecipe(3, 5);
        if (quality != 0)
        {
            ItemClass itm = items[20].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);
            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 Ladrillo.", 5f);
    }

    public void CraftingBucket()
    {
        UpdateInventoryList();
        float quality = 0;

        quality += CalculateAverageQualityForRecipe(2, 15);
        quality += CalculateAverageQualityForRecipe(14, 1);
        quality += CalculateAverageQualityForRecipe(5, 1);

        quality = quality / 3;

        if (quality != 0)
        {
            ItemClass itm = items[21].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);

            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 cubeta.", 5f);
    }

    public void CraftingIronAxe()
    {
        UpdateInventoryList();
        float quality = 0;

        quality += CalculateAverageQualityForRecipe(2, 15);
        quality += CalculateAverageQualityForRecipe(14, 1);

        quality = quality / 2;

        if (quality != 0)
        {
            ItemClass itm = items[11].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);

            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 hacha de hierro.", 5f);
    }

    public void CraftingHoe()
    {
        UpdateInventoryList();
        float quality = 0;

        quality += CalculateAverageQualityForRecipe(2, 15);
        quality += CalculateAverageQualityForRecipe(14, 1);

        quality = quality / 2;

        if (quality != 0)
        {
            ItemClass itm = items[12].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);

            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 azada.", 5f);
    }

    public void CraftingShovel()
    {
        UpdateInventoryList();
        float quality = 0;

        quality += CalculateAverageQualityForRecipe(2, 15);
        quality += CalculateAverageQualityForRecipe(14, 1);

        quality = quality / 2;

        if (quality != 0)
        {
            ItemClass itm = items[15].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);

            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 pala.", 5f);
    }

    public void CraftingPickaxe()
    {
        UpdateInventoryList();
        float quality = 0;

        quality += CalculateAverageQualityForRecipe(2, 15);
        quality += CalculateAverageQualityForRecipe(14, 2);

        quality = quality / 2;

        if (quality != 0)
        {
            ItemClass itm = items[16].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);

            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 pico de hierro.", 5f);
    }

    public void CraftingMeltingPot()
    {
        UpdateInventoryList();
        float quality = CalculateAverageQualityForRecipe(14, 5);
        if (quality != 0)
        {
            ItemClass itm = items[13].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);
            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 crisol.", 5f);
    }

    public void CraftingLantern()
    {
        UpdateInventoryList();
        float quality = CalculateAverageQualityForRecipe(14, 2);
        if (quality != 0)
        {
            ItemClass itm = items[10].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);
            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 linterna.", 5f);
    }

    public void CraftingIronIngot()
    {
        UpdateInventoryList();
        float quality = CalculateAverageQualityForRecipe(17, 5);
        if (quality != 0)
        {
            ItemClass itm = items[14].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);
            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 lingote de hierro.", 5f);
    }

    public void CraftingAnvil()
    {
        UpdateInventoryList();
        float quality = CalculateAverageQualityForRecipe(14, 25);
        if (quality != 0)
        {
            ItemClass itm = items[23].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);
            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 yunque.", 5f);
    }

    public void CraftingBell()
    {
        UpdateInventoryList();
        float quality = CalculateAverageQualityForRecipe(14, 10);
        if (quality != 0)
        {
            ItemClass itm = items[24].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);
            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 campana.", 5f);
    }

    public void CraftingCharcoal()
    {
        UpdateInventoryList();
        float quality = CalculateAverageQualityForRecipe(2, 2);
        if (quality != 0)
        {
            ItemClass itm = items[26].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);
            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 carbon.", 5f);
    }

    public void CraftingGlass()
    {
        UpdateInventoryList();
        float quality = CalculateAverageQualityForRecipe(27, 5);
        if (quality != 0)
        {
            ItemClass itm = items[28].CreateInstance();
            itm.quality = (int)quality;
            InventoryManager.instance.Add(itm);
            InventoryManager.instance.ListItems();
            GeneralGui.Instance.UpdatePanelInventory();
        }
        frogeCraftingSound.Play();
        NotificationManager.Instance.Notification("Fabricaste 1 vidrio.", 5f);
    }

    public void UpdateInventoryList()
    {
        inventory = InventoryManager.instance.GetInventory();
    }

    void SetupLists() 
    {
        inventoryList.Clear();
        inventoryCount.Clear();
        foreach (ItemClass item in inventory)
        {
            bool exist = false;
            for (int x = 0; x < inventoryList.Count; x++)
            {
                if (item.id == inventoryList[x].id)
                {
                    exist = true;
                    inventoryCount[x]++;
                }
            }
            if (!exist)
            {
                inventoryList.Add(item);
                inventoryCount.Add(1);
            }
        }
    }

    void ResetButtons()
    {
        btnRope.interactable = false;
        btnPrimitiveTool.interactable = false;
        btnPrimitiveKnife.interactable = false;
        btnFishingRod.interactable = false;
        btnBrick.interactable = false;
        btnBucket.interactable = false;
        btnIronAxe.interactable = false;
        btnHoe.interactable = false;
        btnPickaxe.interactable = false;
        btnShovel.interactable = false;
        btnLantern.interactable = false;
        btnMeltingPot.interactable = false;
        btnIronIngot.interactable = false;
        btnAnvil.interactable = false;
        btnBell.interactable = false;

        piecePrimitiveTool = 0;
        piecePrimitiveKnife = 0;
        pieceFishingRod = 0;
        pieceBucket = 0;
        pieceIronAxe = 0;
        pieceHoe = 0;
        piecePickaxe = 0;
        pieceShovel = 0;
    }

    // Método para verificar si hay suficientes elementos con el mismo ID
    public float CalculateAverageQualityForRecipe(int id, int quantityNeeded)
    {
        // Filtrar elementos con el mismo ID
        List<ItemClass> sameIdItems = inventory.Where(item => item.id == id).ToList();

        // Verificar si hay suficientes elementos con el mismo ID
        if (sameIdItems.Count >= quantityNeeded)
        {
            // Ordenar los elementos por calidad de menor a mayor
            sameIdItems = sameIdItems.OrderBy(item => item.quality).ToList();

            // Tomar los 'quantityNeeded' elementos con mayores calidades
            List<ItemClass> topQualityItems = sameIdItems.TakeLast(quantityNeeded).ToList();

            // Calcular el promedio de las calidades de esos elementos
            float averageQuality = (float)topQualityItems.Average(item => item.quality);

            for (int i = 0; i < quantityNeeded; i++)
            {
                InventoryManager.instance.RemoveItemByIDAndQuality(topQualityItems[i].id, topQualityItems[i].quality);
            }

            return averageQuality;
        }
        else
        {
            Debug.Log("No hay suficientes elementos con el mismo ID.");
            return 0f;
        }
    }
}
