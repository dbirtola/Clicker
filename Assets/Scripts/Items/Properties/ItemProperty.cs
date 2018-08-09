using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
