using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Should only need to store the value rolled, everything else should be determinable by the items loaded properties
//May need more or less info as well if item properties become more complicated than just a number
//For now that info is up to the item to encode into its propertyData string
[System.Serializable]
public class ItemPropertyData
{
    public string propertyType;
    public string propertyData;
}

public class ItemProperty {

    protected Item item;

    public ItemProperty(Item item)
    {
        this.item = item;
        Roll(this.item.itemLevel);
    }

    public virtual string GetDisplayString()
    {
        return "No Display Information";
    }

    public virtual void Equip(Item item, PlayerStats playerStats)
    {
    }

    public virtual void Unequip(Item item, PlayerStats playerStats)
    {

    }

    public virtual void Roll(int iLvl)
    {

    }

    public virtual void UpgradeRoll()
    {

    }

    public virtual bool IsMax()
    {

        return true;
    }

    public virtual float GetMaxRoll(int iLvl)
    {
        return Mathf.Infinity;
    }


    public virtual int GetTotalValue()
    {
        return 0;
    }

    //Item property classes are responsible for filling in their fields themselves, since their value types
    //and volatile data may change from class to class
    public virtual ItemPropertyData SavePropertyData()
    {
        ItemPropertyData ipd = new ItemPropertyData();
        ipd.propertyType = GetType().ToString();

        return ipd;
    }


    //Should override the load function and not call base, there is nothing the base can do that is useful at the current moment.
    //Alright this became kind of a mess. Its simple, but it was copy and paste code cause the inheritence for item properties is kindof trash.
    //Good area for refactoring when it comes to cleaning up the code base. Won't take long, basically just need to convert a class or two from storing
    //data as a float to int, since every item property class is storing as int right now except one or two.
    public virtual void LoadPropertyData(ItemPropertyData ipd)
    {
        Debug.LogError(ipd.propertyType + " not loading its property data succesfully");
    }

}
