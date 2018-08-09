using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusArmorProperty : ItemProperty{

    public int value;

    public BonusArmorProperty(Item item) : base(item)
    {
    }

    public override string GetDisplayString()
    {
        string displayString = "";


        if (IsMax())
        {
            displayString += "*";
        }

        displayString += "Armor +" + value + "";

        if (item.refineLevel > 0)
        {
            displayString += " (+" + (GetTotalValue() - value).ToString() + ")";
        }

        return displayString;
    }



    public override void Roll(int iLvl)
    {
        base.Roll(iLvl);
        int max = (int)GetMaxRoll(iLvl);
        value = (int)(Mathf.Ceil(Random.Range((2f / 3) * max, max)));
    }




    public override void Equip(Item item, PlayerStats playerStats)
    {
        playerStats.AddBonusArmor(GetTotalValue());
    }

    public override void Unequip(Item item, PlayerStats playerStats)
    {
        playerStats.AddBonusArmor(-1 * GetTotalValue());
    }

    //Might want to add checks to make sure it doesnt go over the max
    public override void UpgradeRoll()
    {
        int newRoll = (int)Mathf.Ceil(Random.Range(value + 1, GetMaxRoll(item.itemLevel)));
        value = newRoll;

        if(value > GetMaxRoll(item.itemLevel))
        {
            value = (int)GetMaxRoll(item.itemLevel);
        }
    }


    //= 3 + 10 * 1.05^iLvl + iLvl
    public override float GetMaxRoll(int iLvl)
    {
        return (Mathf.Ceil(3 + 10 * Mathf.Pow(1.05f, iLvl) + iLvl)); 
    }

    public override bool IsMax()
    {
        if (value == (int)GetMaxRoll(item.itemLevel))
        {
            return true;
        }else
        {
            return false;
        }

    }

    //=value * 1.0565 ^ refineLevel
    public override int GetTotalValue()
    {
        return (int)Mathf.Ceil(value * Mathf.Pow(1.0565f, item.refineLevel));
    }
}
