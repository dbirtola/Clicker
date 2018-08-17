using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemSelectedEvent : UnityEvent<Item>{

}

public class ItemSelectPanel : MonoBehaviour {

    public ItemSelectedEvent itemSelectedEvent;

    public PlayerInventory playerInventory;

    public ItemInfoBox itemInfoBox;
    public Button[] itemButtons;

    public Sprite emptyIcon;



    public void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        itemButtons = GetComponentsInChildren<Button>();

        itemSelectedEvent = new ItemSelectedEvent();

    }

    public void Start()
    {
        playerInventory.inventoryModifiedEvent.AddListener(Refresh);
        //playerInventory.itemEquippedEvent.AddListener();
        Refresh();
    }

    public void Refresh()
    {
        var items = playerInventory.getInventoryItems();



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

    void UpdateButtonWithItem(Button button, Item item)
    {
        if (item == null)
        {
            button.onClick.RemoveAllListeners();
            //Handle null item
            button.GetComponentInChildren<Text>().text = null;
            button.image.sprite = emptyIcon;
            return;
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => FindObjectOfType<PlayerCrafting>().itemSelectedForCraftingEvent.Invoke(item));
        button.onClick.AddListener(() => gameObject.SetActive(false));


        button.GetComponent<InventoryItemButton>().SetItem(item);
        button.GetComponentInChildren<Text>().text = item.itemName; //Move to inside the box
        button.image.sprite = item.itemIcon;
    }
}
