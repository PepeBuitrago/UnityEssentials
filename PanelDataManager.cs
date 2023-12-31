using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelDataManager : MonoBehaviour
{
    public static PanelDataManager Instance;

    [SerializeField]
    private GameObject panelTree, panelStone, panelGround;

    [SerializeField]
    private TMP_Text namePanelTree, qualityPanelTree, durationPanelTree;

    [SerializeField]
    private TMP_Text namePanelStone, qualityPanelStone, durationPanelStone;

    public PlayerController player;

    private void Start()
    {
        Instance = this;
    }

    public void ClosePanelGround()
    {
        if (player != null)
        {
            panelGround.SetActive(false);
            player.UpdateIsFree(true);
        }
        
    }

    public void OpenPanelGround()
    {
        
        if (player != null)
        {
            panelGround.SetActive(true);
            panelGround.transform.position = Input.mousePosition;
            
            ClosePanelStone();
            ClosePanelTree();
        }
        
    }

    public void ClosePanelTree()
    {
        if (player != null)
        {
            panelTree.SetActive(false);
            player.UpdateIsFree(true);
        }
        
    }

    public void OpenPanelTree(Transform hit)
    {
        if(player != null)
        {
            player.UpdateFocusObject(hit.transform.gameObject);
            TreeController TC = hit.transform.GetComponent<TreeController>();
            namePanelTree.text = TC.GetNameTree();
            qualityPanelTree.text = "Calidad: " + TC.GetQualityTree().ToString();
            durationPanelTree.text = "Durabilidad: " + TC.GetDurabilityTree().ToString() + "/100";
            panelTree.SetActive(true);
            panelTree.transform.position = Input.mousePosition;
            ClosePanelStone();
            ClosePanelGround();
            player.UpdateIsFree(false);
        }
        
    }

    public void ClosePanelStone()
    {
        if (player != null)
        {
            panelStone.SetActive(false);
            player.UpdateIsFree(true);
        }
        
    }

    public void OpenPanelStone(Transform hit)
    {
        if (player != null)
        {
            player.UpdateFocusObject(hit.transform.gameObject);
            StoneController ST = hit.transform.GetComponent<StoneController>();
            namePanelStone.text = ST.GetNameStone();
            qualityPanelStone.text = "Calidad: " + ST.GetQualityStone().ToString();
            durationPanelStone.text = "Durabilidad: " + ST.GetDurabilityStone().ToString() + "/100";
            panelStone.SetActive(true);
            panelStone.transform.position = Input.mousePosition;
            ClosePanelTree();
            ClosePanelGround();
            player.UpdateIsFree(false);
        }
        
    }

    public void FindInGround()
    {
        if (player != null)
        {
            player.FindInGround();
        }
    }

    public void Mining()
    {
        if (player != null)
        {
            player.Mining();
        }
    }

    public void ChopTree()
    {
        if (player != null)
        {
            player.ChopTree();
        }
    }
}
