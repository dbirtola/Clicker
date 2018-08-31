using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusHealthProperty : ItemProperty{

    public int value = 0;


    public BonusHealthProperty(Equipment item) : base(item)
    {
    }

    public override string GetDisplayString()
    {
        string displayString = "";


        if (IsMax())
        {
            displayString += "*";
        }

        displayString += "Health +" + value;

        if (item.refineLevel > 0)
        {
            displayString += " (+" + (GetTotalValue() - value).ToString() + ")";
        }

        return displayString;
    }

    public override void Equip(Equipment item, PlayerStats playerStats)
    {
        playerStats.AddBonusHealth(GetTotalValue());
    }

    public override void Unequip(Equipment item, PlayerStats playerStats)
    {
        playerStats.AddBonusHealth(-1 * GetTotalValue());
    }

    public override void Roll(int iLvl)
    {
        base.Roll(iLvl);
        int max = (int)GetMaxRoll(iLvl);
        value = (int)(Mathf.Ceil(Random.Range((2 / 3f) * max, max)));
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

    
    // =9 + iLvl* 1.055^iLvl
    public override float GetMaxRoll(int iLvl)
    {
        return (Mathf.Ceil(9 + iLvl * (Mathf.Pow(1.055f, iLvl))));
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
    
    
    //value * 1.07 ^ refineLevel
    public override int GetTotalValue()
    {
        return (int)Mathf.Ceil(value * Mathf.Pow(1.07f, item.refineLevel));
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
