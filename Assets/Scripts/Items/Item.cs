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

    public void RollProperties(int itemLevel)
    {
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
                Debug.Log("Property: " + ip.GetType() + " matched type: " + type);
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


}
