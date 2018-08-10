using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamageProperty : ItemProperty{


    //Stored as an int for consistency to the singles digit place
    public int value;


    public CriticalDamageProperty(Item item) : base(item)
    {

    }
   
	public override string GetDisplayString()
    {
        string displayString = "";

        
        if (IsMax())
        {
            displayString += "*";
        }

        displayString += "Critical Damage +" + value + "%";

        if(item.refineLevel > 0)
        {
            displayString += " (+" + (GetTotalValue() - value).ToString() + "%)";
        }

        return displayString;
    }


    public override void Equip(Item item, PlayerStats playerStats)
    {
        playerStats.AddBonusCriticalDamage(GetTotalValue()/100);
    }

    public override void Unequip(Item item, PlayerStats playerStats)
    {
        playerStats.AddBonusCriticalDamage(-1 * GetTotalValue()/100);
    }


    public override void Roll(int iLvl)
    {
        base.Roll(iLvl);
        int max = (int)GetMaxRoll(iLvl);
        value = (int)(Mathf.Ceil(Random.Range((2f / 3) * max, max)));
    }


    public override void UpgradeRoll()
    {
        int newRoll = (int)Mathf.Ceil(Random.Range(value + 1, GetMaxRoll(item.itemLevel)));
        value = newRoll;

        if (value > GetMaxRoll(item.itemLevel))
        {
            value = (int)GetMaxRoll(item.itemLevel);
        }
    }


    //=(iLvl + 1.06^(iLvl) + 7)/
    public override float GetMaxRoll(int iLvl)
    {
        return (Mathf.Ceil(iLvl + Mathf.Pow(1.06f, iLvl) + 7));
    }

    public override bool IsMax()
    {
        if (value == (int)GetMaxRoll(item.itemLevel))
        {
            return true;
        }
        else
        {
            return false;
        }

    }



    //=value) * 1.06^refineLevel - 0.01
    public override int GetTotalValue()
    {
        return (int)Mathf.Ceil(value * Mathf.Pow(1.06f, item.refineLevel) - 0.01f);
    }

    public override ItemPropertyData SavePropertyData()
    {
        ItemPropertyData ipd = base.SavePropertyData();
        ipd.propertyData = value.ToString();

        return ipd;
    }

    public override void LoadPropertyData(ItemPropertyData ipd)
    {
        value = int.Parse(ipd.propertyData);
    }
}
