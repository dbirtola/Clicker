using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPerTapProperty : ItemProperty{


    //Stored as an int because this whole property is a hack
    //and every other property is stored as an int where it probably shouldve been a float
    //so things like this didnt need to be so hacky, but awell we will avoid
    //the proper way of doing this for another day

        //This value should be /100 to get the real value
    public int value;


    public ManaPerTapProperty(Item item) : base(item)
    {

    }
   
	public override string GetDisplayString()
    {
        string displayString = "";

        
        if (IsMax())
        {
            displayString += "*";
        }

        displayString += "Mana Per Tap +" + value/100f;

        if(item.refineLevel > 0)
        {
            displayString += " (+" + ((GetTotalValue() - value)/100f).ToString() + ")";
        }

        return displayString;
    }


    public override void Equip(Item item, PlayerStats playerStats)
    {
        playerStats.AddManaPerTap(GetTotalValue()/100f);
    }

    public override void Unequip(Item item, PlayerStats playerStats)
    {
        playerStats.AddManaPerTap(-1 * GetTotalValue()/100f);
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


    //=(iLvl/120)
    public override float GetMaxRoll(int iLvl)
    {
        //Didnt feel like looking up proper way to take the ceiling of 2 decimal places, so this is a little hacky
        return Mathf.Ceil((iLvl*100f/120));
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
        return (int)Mathf.Ceil(value * Mathf.Pow(1.0564f, item.refineLevel));
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
