using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemQuality
{
    Normal,
    Magic,
    Rare,
    Epic,
    Unique
}

//Only need to save data generated through gameplay, not data from the prefabs
[System.Serializable]
public class ItemData
{
    public string itemType;
    public ItemQuality itemQuality;
    public int itemLevel;
    public int refineLevel;

    public List<ItemPropertyData> itemPropertyDatas;
    
}

public class Item : MonoBehaviour {



    public string itemName;
    public Sprite itemIcon;
    ItemQuality itemQuality = ItemQuality.Normal;
    public int itemLevel = 1;
    protected ItemProperty implicitProperty;
    protected List<ItemProperty> properties;

    protected bool isEquipped = false;

    protected PlayerStats playerStats;
    

    public int refineLevel = 0;


    virtual protected void Awake()
    {
        DontDestroyOnLoad(gameObject);

        properties = new List<ItemProperty>();
        playerStats = FindObjectOfType<PlayerStats>();

    }
    virtual protected void Start()
    {
       // implicitProperty = new BonusHealthProperty(this);
    }

    public string GetDisplayName()
    {
        if(refineLevel != 0)
        {
            return itemName + " +" + refineLevel;
        }else
        {
            return itemName;
        }
    }
	
    public virtual void Equip()
    {

        implicitProperty.Equip(this, playerStats);
        foreach(ItemProperty ip in properties)
        {
            ip.Equip(this, playerStats);
        }
        isEquipped = true;
        
    }

    public virtual void Unequip()
    {
       implicitProperty.Unequip(this, playerStats);
       foreach (ItemProperty ip in properties)
       {
            ip.Unequip(this, playerStats);
        }
        isEquipped = false;
    }

    public ItemProperty GetImplicitProperty()
    {
        return implicitProperty;
   
    }

    public List<ItemProperty> GetItemProperties()
    {
        return properties;
    }
    
    public bool GetEquipped()
    {
        return isEquipped;
    }

    public virtual void IncreaseRefineLevel()
    {
        refineLevel++;
    }

    public void RollProperties(int itemLevel)
    {
        properties.Clear();

        int numProperties = GetMaxProperties();
        for(int i = 0; i < numProperties; i++)
        {
            AddRandomProperty();
        }
    }

    public void AddRandomProperty()
    {
        bool wasEquipped = GetEquipped();
        if (wasEquipped)
        {
            Unequip();
        }

        ItemProperty newProperty = null;
        int roll = 0;

        do
        {
            roll = Random.Range(0, 6);
            switch (roll)
            {
                case 0:
                    newProperty = new BonusHealthProperty(this);
                    break;
                case 1:
                    newProperty = new CriticalChanceProperty(this);
                    break;
                case 2:
                    newProperty = new CriticalDamageProperty(this);
                    break;
                case 3:
                    newProperty = new BonusArmorProperty(this);
                    break;
                case 4:
                    newProperty = new ManaPerTapProperty(this);
                    break;
                case 5:
                    newProperty = new PoisonDamageProperty(this);
                    break;
            }




        } while (HasPropertyOfType(newProperty.GetType()) == true);

       // newProperty.Roll();
        properties.Add(newProperty);

        if (wasEquipped)
        {
            Equip();
        }
    }

    public bool HasPropertyOfType(System.Type type)
    {
        foreach(ItemProperty ip in properties)
        {
            if(ip.GetType() == type)
            {
                return true;
            }
        }
        return false;
    }

    public bool RemoveProperty(ItemProperty property)
    {
        if (!properties.Contains(property))
        {
            return false;
        }


        bool wasEquipped = GetEquipped();
        if (wasEquipped)
        {
            Unequip();
        }

        properties.Remove(property);

        if (wasEquipped)
        {
            Equip();
        }

        return true;
    }

    public int GetMaxProperties()
    {
        switch (itemQuality)
        {
            case ItemQuality.Normal:
                return 0;
            case ItemQuality.Magic:
                return 2;
            case ItemQuality.Rare:
                return 3;
            case ItemQuality.Epic:
                return 4;
        }

        return 0;
    }

    public void SetQuality(int quality)
    {
        itemQuality = (ItemQuality)quality;
    }

    public ItemQuality GetQuality()
    {
        return itemQuality;
    }

    public virtual void SetLevel(int level)
    {
        itemLevel = level;
    }

    

    
        //Probably a better and safer way to do this than string comparison, but this works for now
       
    ItemProperty AddProperty(string propertyName)
    {
        ItemProperty newProperty = null;
        switch (propertyName)
        {
            case "BonusHealthProperty":
                newProperty = new BonusHealthProperty(this);
                break;
            case "CriticalChanceProperty":
                newProperty = new CriticalChanceProperty(this);
                break;
            case "CriticalDamageProperty":
                newProperty = new CriticalDamageProperty(this);
                break;
            case "BonusArmorProperty":
                newProperty = new BonusArmorProperty(this);
                break;
            case "ManaPerTapProperty":
                newProperty = new ManaPerTapProperty(this);
                break;
            case "PoisonDamageProperty":
                newProperty = new PoisonDamageProperty(this);
                break;
            default:
                Debug.LogError("Unable to load property of type: " + propertyName);
                return null;
        }

        Debug.Log("Loaded property: " + newProperty.GetType().ToString());
        properties.Add(newProperty);

        return newProperty;
    }


    public ItemData SaveItemData()
    {
        ItemData itemData = new ItemData();
        //Basically this has an issue because the 4 main armor types are all of class Armor, which makes GetType() return Armor for each one
        //Need to either split into subclasses or find a better way to store the itemtype data. Name will work but is not reliable, as item names
        //Should be able to change without corrupting save game data
        itemData.itemType = itemName; //THIS NEEDS TO BE FIXED. ONLY WORKS BECAUSE THATS HOW PREFABS ARE NAMED
        
        itemData.itemLevel = itemLevel;
        itemData.itemQuality = itemQuality;
        itemData.refineLevel = refineLevel;
        itemData.itemPropertyDatas = new List<ItemPropertyData>();

        foreach (ItemProperty ip in properties)
        {
            itemData.itemPropertyDatas.Add(ip.SavePropertyData());
        }

        return itemData;
    }


    //Type should have already been determined by the inventory class when loading the items
    public void LoadItemData(ItemData itemData)
    {
        itemLevel = itemData.itemLevel;
        itemQuality = itemData.itemQuality;
        refineLevel = itemData.refineLevel;
        
        foreach(ItemPropertyData ipd in itemData.itemPropertyDatas)
        {
            var newProperty = AddProperty(ipd.propertyType);
            newProperty.LoadPropertyData(ipd);
        }
        
    }

    //Values of items are representing in base number of crafting taps
    //A player with 100 base materials per tap would get 100 x crafting value materials for a sale
    //
    public int GetSaleValue()
    {
        int baseValue = (int)(itemLevel * 2.5f);
        int qualityBonus = (int)itemQuality * 10;
        if(qualityBonus != 0)
        {
            baseValue *= qualityBonus;
        }
        return baseValue;
    }
}
