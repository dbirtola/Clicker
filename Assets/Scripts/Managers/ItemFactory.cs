using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemFactory : MonoBehaviour {
    static ItemFactory itemFactory = null;

    Dictionary<string, GameObject> items;

    public const float baseDropChance = 0.25f;
    public const float qualityChance = 0.2f;

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
        items["Ring"] = Resources.Load("Ring") as GameObject;


    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Item GetItemDrop(Enemy monster)
    {

        if (Random.Range(0, 1f) > baseDropChance)
        {
            return null; //No Drop;
        }

        Item newItem = null;


        int itemType = Random.Range(0, 6);


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
            case 5:
                newItem = Instantiate(items["Ring"]).GetComponent<Item>();
                break;
        }
        

        int quality = 0;
        if(Random.Range(0, 1f) <= qualityChance * monster.dropQualityMultiplier)
        {
            quality++;
            if(Random.Range(0, 1f) <= qualityChance * monster.dropQualityMultiplier)
            {
                quality++;
                if(Random.Range(0, 1f) <= qualityChance * monster.dropQualityMultiplier)
                {
                    quality++;
                    if(Random.Range(0, 1f) <= qualityChance * monster.dropQualityMultiplier)
                    {
                        quality++;
                    }
                }
            }
        }

        newItem.SetQuality(quality);
        newItem.SetLevel(monster.level);
        newItem.RollProperties(monster.level);

        return newItem;
    }


    //For testing purposes
    public Item GetItemDrop(int level)
    {

        if (Random.Range(0, 1f) > baseDropChance)
        {
            return null; //No Drop;
        }

        Item newItem = null;


        int itemType = Random.Range(0, 6);


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
            case 5:
                newItem = Instantiate(items["Ring"]).GetComponent<Item>();
                break;
        }


        int quality = 0;
        if (Random.Range(0, 1f) <= qualityChance)
        {
            quality++;
            if (Random.Range(0, 1f) <= qualityChance)
            {
                quality++;
                if (Random.Range(0, 1f) <= qualityChance)
                {
                    quality++;
                    if (Random.Range(0, 1f) <= qualityChance)
                    {
                        quality++;
                    }
                }
            }
        }

        newItem.SetQuality(quality);
        newItem.SetLevel(level);
        newItem.RollProperties(level);

        return newItem;
    }

    //itemType should match the class name of the item as stored
    //in the dictionary "items"
    public Item SpawnItemOfType(string itemType)
    {
        if(items[itemType] == null)
        {
            Debug.LogError("Could not spawn item of type: " + itemType);
            return null;
        }

        Item newItem = Instantiate(items[itemType]).GetComponent<Item>();
        return newItem;
    }
}
