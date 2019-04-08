using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class CraftingScreen : MonoBehaviour {



    public PlayerCrafting playerCrafting;

    public Button refineButton;
    public Button upgradePropertiesButton;
    public Button addRandomPropertyButton;
    public Button removePropertyButton;
    public Button itemSelectButton;

    public ItemSelectPanel itemSelectPanel;
    public ItemInfoBox itemInfoBox;
    public GameObject propertySelectButtons;

    public Text refineCostText;
    public Text UpgradePropertiesCostText;
    public Text addPropertyText;
    public Text removePropertyText;


    public Text materialsText;

    public Equipment selectedItem;
    

    public void Awake()
    {
    }

    public void Start()
    {
        playerCrafting.itemSelectedForCraftingEvent.AddListener(UpdateCraftingInfo);
        playerCrafting.itemRefinedEvent.AddListener(UpdateCraftingInfo);

        /*
        playerCrafting.materialGainedEvent.AddListener(
            (int num) =>
            {
                materialsText.text = "Materials: " + playerCrafting.materials;
            }
        );
        */
    }



    void UpdateCraftingInfo(Equipment item)
    {
        selectedItem = item;

        itemInfoBox.UpdateWithItem(item);


        refineCostText.text = playerCrafting.GetRefineCost(item).ToString();
        addPropertyText.text = playerCrafting.GetAddRandomPropertyCost(item).ToString();
        removePropertyText.text = playerCrafting.GetRemovePropertyCost(item).ToString();
        UpgradePropertiesCostText.text = playerCrafting.GetPropertyUpgradeCost(item).ToString();

        if(playerCrafting.GetRefineCost(selectedItem) > playerCrafting.materials)
        {
            refineButton.interactable = false;
        }else
        {
            refineButton.interactable = true;
        }

        if(playerCrafting.GetAddRandomPropertyCost(selectedItem) > playerCrafting.materials || item.GetItemProperties().Count >= item.GetMaxProperties()){
            addRandomPropertyButton.interactable = false;
        }else
        {
            addRandomPropertyButton.interactable = true;
        }


        int numPropertiesMaxed = 0;
        foreach(ItemProperty ip in item.GetItemProperties())
        {
            if (ip.IsMax())
            {
                numPropertiesMaxed++;
            }
        }
        bool isMaxed = (numPropertiesMaxed == item.GetItemProperties().Count);
        if(playerCrafting.GetRemovePropertyCost(selectedItem) > playerCrafting.materials || isMaxed)
        {
            removePropertyButton.interactable = false;
        }else
        {
            removePropertyButton.interactable = true;   
        }

    }

    public void RefineSelectedItem()
    {
        playerCrafting.RefineItem(selectedItem);
        UpdateCraftingInfo(selectedItem);
    }

    public void UpgradePropertiesSelectedItem()
    {
        playerCrafting.UpgradeProperties(selectedItem);
        UpdateCraftingInfo(selectedItem);
    }

    public void AddRandomPropertySelectedItem()
    {
        playerCrafting.AddRandomProperty(selectedItem);
        UpdateCraftingInfo(selectedItem);
    }

    public void RemovePropertyFromSelectedItem(int selectedProperty)
    {
        playerCrafting.RemoveProperty(selectedItem, selectedItem.GetItemProperties()[selectedProperty]);
        UpdateCraftingInfo(selectedItem);
        propertySelectButtons.SetActive(false);
    }

    public void RequestPropertyToRemove()
    {
        propertySelectButtons.SetActive(true);
    }

    public void CancelRequestPropertyToRemove()
    {
        propertySelectButtons.SetActive(false);
    }
}
