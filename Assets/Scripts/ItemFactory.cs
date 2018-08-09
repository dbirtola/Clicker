using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour {
    static ItemFactory itemFactory = null;

    Dictionary<string, GameObject> items;


    void Awake()
    {
        if(itemFactory != null)
        {
            Destroy(gameObject);
        }else
        {
            itemFactory = this;
        }
        DontDestroyOnLoad(gameObject);

        items = new Dictionary<string, GameObject>();
        items["Armor"] = Resources.Load("Armor") as GameObject;
        items["Boots"] = Resources.Load("Boots") as GameObject;
        items["Gloves"] = Resources.Load("Gloves") as GameObject;
        items["Helmet"] = Resources.Load("Helmet") as GameObject;
        items["Weapon"] = Resources.Load("Weapon") as GameObject;


    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Item GetItemDrop(int monsterLevel)
    {

        Item newItem = null;

        int itemType = Random.Range(0, 5);


        if(monsterLevel < 20)
        {
            switch (itemType)
            {
                case 0:
                    newItem = Instantiate(items["Armor"]).GetComponent<Item>(); 
                    break;
                case 1:
                    newItem = Instantiate(items["Boots"]).GetComponent<Item>();
                    break;
                case 2:
                    newItem = Instantiate(items["Gloves"]).GetComponent<Item>();
                    break;
                case 3:
                    newItem = Instantiate(items["Helmet"]).GetComponent<Item>();
                    break;
                case 4:
                    newItem = Instantiate(items["Weapon"]).GetComponent<Item>();
                    break;
            }
        }

        int quality = 0;
        if(Random.Range(0, 4) == 0)
        {
            quality++;
            if(Random.Range(0, 4) == 0)
            {
                quality++;
                if(Random.Range(0, 4) == 0)
                {
                    quality++;
                    if(Random.Range(0, 4) == 0)
                    {
                        quality++;
                    }
                }
            }
        }
        newItem.SetQuality(quality);
        newItem.RollProperties(monsterLevel);

        return newItem;
    }
}
