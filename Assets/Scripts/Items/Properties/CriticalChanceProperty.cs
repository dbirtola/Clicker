using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChanceProperty : ItemProperty{

    
    //public int value;

    public float value;
   
    public CriticalChanceProperty(Item item) : base(item)
    {

    }
    
	public override string GetDisplayString()
    {
        string displayString = "";


        if (IsMax())
        {
            displayString += "*";
        }

        displayString += "Critical Chance +" + value + "%";

        if (item.refineLevel > 0)
        {
            displayString += " (+" + (GetTotalValue() - value).ToString() + "%)";
        }

        return displayString;
    }


    public override void Equip(Item item, PlayerStats playerStats)
    {
        playerStats.AddBonusCriticalChance(GetTotalValue());
    }

    public override void Unequip(Item item, PlayerStats playerStats)
    {
        playerStats.AddBonusCriticalChance(-1 * GetTotalValue());
    }


    public override void Roll(int iLvl)
    {
        base.Roll(iLvl);
        float max = GetMaxRoll(iLvl);
        Debug.Log("ROLLING CRIT WITH RANGE: " + (2 / 3f) * max + "  - " + max);
        value = (int)Mathf.Ceil(Random.Range((2f / 3) * max, max));
    }


    public override void UpgradeRoll()
    {
        float newRoll = (int)Mathf.Ceil(Random.Range(value + 1, GetMaxRoll(item.itemLevel)));
        value = newRoll;

        if (value > GetMaxRoll(item.itemLevel))
        {
            value = GetMaxRoll(item.itemLevel);
        }
    }


    //=(1/600* iLvl)
    public override float GetMaxRoll(int iLvl)
    {
        return Mathf.Ceil((iLvl / 600f) * 100);
    }

    public override bool IsMax()
    {
        if (value == GetMaxRoll(item.itemLevel))
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    //=value + value * 1/5(refineLevel)
    public override int GetTotalValue()
    {
        return (int)Mathf.Ceil(value + value * 1/5f * item.refineLevel);
    }
}
