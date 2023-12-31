using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGui : MonoBehaviour
{
    public static GeneralGui Instance;

    [SerializeField]
    private GameObject skillsPanel;
    [SerializeField]
    private bool skillsPanelState;

    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private bool inventoryPanelState = true;

    [SerializeField]
    private GameObject chatPanel;
    [SerializeField]
    private bool chatPanelState = true;

    [SerializeField]
    private GameObject buildingPanel;
    [SerializeField]
    private bool buildingPanelState;

    [SerializeField]
    private GameObject craftingPanel;
    [SerializeField]
    private bool craftingPanelState;

    [SerializeField]
    private GameObject forgePanel;
    [SerializeField]
    private bool forgePanelState;

    [SerializeField]
    private GameObject foundryPanel;
    [SerializeField]
    private bool foundryPanelState;

    [SerializeField]
    private GameObject KilmPanel;
    [SerializeField]
    private bool KilmPanelState;

    private void Start()
    {
        Instance = this;
    }

    public void UpdatePanelSkills() 
    {
        if (!skillsPanelState)
        {
            ClosePanels();
            skillsPanel.SetActive(true);
            skillsPanelState = true;
            return;
        }
        if (skillsPanelState)
        {
            ClosePanels();
            return;
        }
    }
    public void UpdatePanelInventory()
    {
        if (!inventoryPanelState)
        {
            ClosePanels();
            inventoryPanel.SetActive(true);
            inventoryPanelState = true;
            InventoryManager.instance.PanelInfoExit();
            return;
        }
        if (inventoryPanelState)
        {
            ClosePanels();
            return;
        }
    }

    public void UpdatePanelChat()
    {
        if (!chatPanelState)
        {
            ClosePanels();
            chatPanel.SetActive(true);
            chatPanelState = true;
            InventoryManager.instance.PanelInfoExit();
            return;
        }
        if (chatPanelState)
        {
            ClosePanels();
            return;
        }
    }

    public void UpdatePanelBuilding()
    {
        if (!buildingPanelState)
        {
            ClosePanels();
            buildingPanel.SetActive(true);
            buildingPanelState = true;
            return;
        }
        if (buildingPanelState)
        {
            ClosePanels();
            return;
        }
    }

    public void UpdatePanelCrafting()
    {
        if (!craftingPanelState)
        {
            ClosePanels();
            craftingPanel.SetActive(true);
            //CraftingSystem.Instance.UpdateButtonsState();
            craftingPanelState = true;
            return;
        }
        if (craftingPanelState)
        {
            ClosePanels();
            return;
        }
    }

    public void UpdatePanelForge()
    {
        if (!forgePanelState)
        {
            ClosePanels();
            forgePanel.SetActive(true);
            CraftingSystem.Instance.UpdateButtonsState();
            forgePanelState = true;
            return;
        }
        if (forgePanelState)
        {
            ClosePanels();
            return;
        }
    }

    public void UpdatePanelFoundry()
    {
        if (!foundryPanelState)
        {
            ClosePanels();
            foundryPanel.SetActive(true);
            CraftingSystem.Instance.UpdateButtonsState();
            foundryPanelState = true;
            return;
        }
        if (foundryPanelState)
        {
            ClosePanels();
            return;
        }
    }

    public void UpdatePanelKilm()
    {
        if (!KilmPanelState)
        {
            ClosePanels();
            KilmPanel.SetActive(true);
            CraftingSystem.Instance.UpdateButtonsState();
            KilmPanelState = true;
            return;
        }
        if (KilmPanelState)
        {
            ClosePanels();
            return;
        }
    }

    public void ClosePanels()
    {
        if (skillsPanelState)
        {
            skillsPanel.SetActive(false);
            skillsPanelState = false;
            return;
        }
        if (craftingPanelState)
        {
            craftingPanel.SetActive(false);
            craftingPanelState = false;
            return;
        }
        if (buildingPanelState)
        {
            buildingPanel.SetActive(false);
            buildingPanelState = false;
            return;
        }
        if (inventoryPanelState)
        {
            inventoryPanel.SetActive(false);
            inventoryPanelState = false;
            return;
        }
        if (chatPanelState)
        {
            chatPanel.SetActive(false);
            chatPanelState = false;
            return;
        }
        if (forgePanelState)
        {
            forgePanel.SetActive(false);
            forgePanelState = false;
            return;
        }
        if (foundryPanelState)
        {
            foundryPanel.SetActive(false);
            foundryPanelState = false;
            return;
        }
        if (KilmPanelState)
        {
            KilmPanel.SetActive(false);
            KilmPanelState = false;
            return;
        }
    }
}
