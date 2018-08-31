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


    protected PlayerStats playerStats;


    protected bool isEquipped = false;

    public string itemName;
    public Sprite itemIcon;
    protected ItemQuality itemQuality = ItemQuality.Normal;
    public int itemLevel = 1;


    public virtual void Awake()
    {

        DontDestroyOnLoad(gameObject);

    }
    
    public virtual string GetDisplayName()
    {

        return "Null";
    }

    public virtual void Equip()
    {
        isEquipped = true;
    }

    public virtual void Unequip()
    {

        isEquipped = false;
    }


    public bool GetEquipped()
    {
        return isEquipped;
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


}
