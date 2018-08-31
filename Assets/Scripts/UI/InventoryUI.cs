using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    public PlayerInventory playerInventory;

    public Button helmetSlot;
    public Button gloveSlot;
    public Button armorSlot;
    public Button bootsSlot;
    public Button weaponSlot;
    public Button ringSlot;

    public ItemInfoBox itemInfoBox;
    public Button[] itemButtons;

    public Sprite emptyIcon;

    public Dropdown autoSellDropdown;

    public void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        itemButtons = GetComponentsInChildren<Button>();

    }

    public void Start()
    {
        playerInventory.inventoryModifiedEvent.AddListener(Refresh);
        //playerInventory.itemEquippedEvent.AddListener();
        Refresh();
    }

    public void UpdateAutoSell()
    {
        //The dropdown treats 0 as none, whereas the inventory treats -1 as none

        Debug.Log("Updated auto sell");
        playerInventory.autoSellQuality = autoSellDropdown.value - 1;
    }

    public void ShowItemInfo(Equipment item)
    {

        itemInfoBox.UpdateWithItem(item);
        itemInfoBox.gameObject.SetActive(true);
    }

    public void Refresh()
    {
        var items = playerInventory.getInventoryItems();

        UpdateButtonWithItem(armorSlot, playerInventory.GetArmor());
        UpdateButtonWithItem(gloveSlot, playerInventory.GetGloves());
        UpdateButtonWithItem(bootsSlot, playerInventory.GetBoots());
        UpdateButtonWithItem(helmetSlot, playerInventory.GetHelmet());
        UpdateButtonWithItem(weaponSlot, playerInventory.GetWeapon());
        UpdateButtonWithItem(ringSlot, playerInventory.GetRing());

        for (int i = 0; i < itemButtons.Length; i++)
        {
            //itemButtons[i].GetComponentInChildren<Text>().text = items[i].itemName;
            if (i < items.Count)
            {
                UpdateButtonWithItem(itemButtons[i], items[i]);
            }
            else
            {
                UpdateButtonWithItem(itemButtons[i], null);
            }
        }
    }

    void UpdateButtonWithItem(Button button, Equipment item)
    {
        button.GetComponent<InventoryItemButton>().UpdateButtonWithItem(item);
        /*
        if(item == null)
        {
            //button.onClick.RemoveAllListeners();
            //Handle null item
            button.GetComponentInChildren<Text>().text = null;
            button.image.sprite = emptyIcon;
            return;
        }
       // button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ShowItemInfo(item));



        button.GetComponent<InventoryItemButton>().SetItem(item);
        button.GetComponentInChildren<Text>().text = item.itemName; //Move to inside the box
        button.image.sprite = item.itemIcon;

    */
    }
}
