using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour {

    Item item;
    //Button button;

    void Awake()
    {
       // button = GetComponent<Button>();
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
}
