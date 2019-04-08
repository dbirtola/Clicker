using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour {

    Item item;
    Button button;

    float timeClicked = 0;
    float doubleClickThreshold = 0.25f;
    
    void Awake()
    {
       // timeClicked = float.MaxValue;

        button = GetComponent<Button>();


       // button.onClick.AddListener(FindObjectOfType<InventoryUI>().ShowItemInfo);

    }
	// Use this for initialization
	void Start () {
		
	}

   
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetItem(Item item)
    {
        this.item = item;

    }

    public Item GetItem()
    {
        return item;
    }


    public void UpdateButtonWithItem(Item item)
    {
        
        SetItem(item);


        InventoryUI inv = FindObjectOfType<InventoryUI>();
        
        if (item == null)
        {
            button.onClick.RemoveAllListeners();
            //Handle null item
            button.GetComponentInChildren<Text>().text = null;
            button.image.sprite = inv.emptyIcon;
            return;
        }
        button.onClick.RemoveAllListeners();
        // button.onClick.AddListener(() => 

        //This is getting sloppy, should probably redo this
        //Definitely redo this. it assumes all items are going to be equipment and that they should be equipped

        if (item.GetComponent<Equipment>())
        {

            button.onClick.AddListener(() =>
            {
                Debug.Log("Clicked");
                if (Time.time - timeClicked < doubleClickThreshold)
                {
                    inv.itemInfoBox.gameObject.SetActive(false);
                    FindObjectOfType<PlayerInventory>().EquipItem(item.GetComponent<Equipment>());
                }
                else
                {
                    inv.ShowItemInfo(item.GetComponent<Equipment>());
                    Debug.Log("Not fast enough: " + Time.time + " vs " + timeClicked);
                    timeClicked = Time.time;
                }

            }

);

        }


        //button.GetComponentInChildren<Text>().text = item.itemName; //Move to inside the box
        button.image.sprite = item.itemIcon;
    }
}
