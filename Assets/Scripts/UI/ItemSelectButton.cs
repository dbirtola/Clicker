using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectButton : MonoBehaviour {

    public ItemSelectPanel itemSelectPanel;

    public Sprite emptyIcon;

    void Start()
    {

    }

    public void ShowItemSelect()
    {
        itemSelectPanel.gameObject.SetActive(true);
        FindObjectOfType<PlayerCrafting>().itemSelectedForCraftingEvent.AddListener(UpdateButtonWithItem);

    }

    void UpdateButtonWithItem(Item item)
    {
        Button button = GetComponent<Button>();
        if (item == null)
        {
            button.onClick.RemoveAllListeners();
            //Handle null item
            button.GetComponentInChildren<Text>().text = null;
            button.image.sprite = emptyIcon;
            return;
        }

       // button.onClick.RemoveAllListeners();
        //button.onClick.AddListener(() => itemSelectedEvent.Invoke(item));



        button.GetComponent<InventoryItemButton>().SetItem(item);
        button.GetComponentInChildren<Text>().text = item.itemName; //Move to inside the box
        button.image.sprite = item.itemIcon;
    }
}
