using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemFactory : MonoBehaviour {
    static ItemFactory itemFactory = null;

    Dictionary<string, GameObject> items;
    Dictionary<string, GameObject> charms;

    public const float baseDropChance = 0.1f;
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

        charms = new Dictionary<string, GameObject>();
        charms["MagicFindCharm"] = Resources.Load("MagicFindCharm") as GameObject;
        charms["CriticalCharm"] = Resources.Load("CriticalCharm") as GameObject;
        charms["HealthCharm"] = Resources.Load("HealthCharm") as GameObject;


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

        Equipment newItem = null;


        int itemType = Random.Range(0, 6);


        switch (itemType)
        {
            case 0:
                newItem = Instantiate(items["Armor"]).GetComponent<Equipment>(); 
                break;
            case 1:
                newItem = Instantiate(items["Boots"]).GetComponent<Equipment>();
                break;
            case 2:
                newItem = Instantiate(items["Gloves"]).GetComponent<Equipment>();
                break;
            case 3:
                newItem = Instantiate(items["Helmet"]).GetComponent<Equipment>();
                break;
            case 4:
                newItem = Instantiate(items["Weapon"]).GetComponent<Equipment>();
                break;
            case 5:
                newItem = Instantiate(items["Ring"]).GetComponent<Equipment>();
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

    public Item GetCharmDrop()
    {
        Charm newCharm = null;
        int charmType = Random.Range(0, 3);

        switch (charmType)
        {
            case 0:
                newCharm = Instantiate(charms["MagicFindCharm"]).GetComponent<Charm>();
                break;
            case 1:
                newCharm = Instantiate(charms["CriticalCharm"]).GetComponent<Charm>();
                break;
            case 2:
                newCharm = Instantiate(charms["HealthCharm"]).GetComponent<Charm>();
                break;

        }

        return newCharm;
    }


    //For testing purposes
    public Item GetItemDrop(int level, bool guaranteeDrop = false)
    {

        if (Random.Range(0, 1f) > baseDropChance && !guaranteeDrop)
        {
            return null; //No Drop;
        }

        Equipment newItem = null;


        int itemType = Random.Range(0, 6);


        switch (itemType)
        {
            case 0:
                newItem = Instantiate(items["Armor"]).GetComponent<Equipment>();
                break;
            case 1:
                newItem = Instantiate(items["Boots"]).GetComponent<Equipment>();
                break;
            case 2:
                newItem = Instantiate(items["Gloves"]).GetComponent<Equipment>();
                break;
            case 3:
                newItem = Instantiate(items["Helmet"]).GetComponent<Equipment>();
                break;
            case 4:
                newItem = Instantiate(items["Weapon"]).GetComponent<Equipment>();
                break;
            case 5:
                newItem = Instantiate(items["Ring"]).GetComponent<Equipment>();
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
    public Equipment SpawnEquipmentOfType(string itemType)
    {
        if(items[itemType] == null)
        {
            Debug.LogError("Could not spawn item of type: " + itemType);
            return null;
        }

        Equipment newItem = Instantiate(items[itemType]).GetComponent<Equipment>();
        return newItem;
    }
}
