using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDamageProperty : ItemProperty{


    //Stored as an int because this whole property is a hack
    //and every other property is stored as an int where it probably shouldve been a float
    //so things like this didnt need to be so hacky, but awell we will avoid
    //the proper way of doing this for another day
    //When this is fixed it also needs to be taken care of in ManaPerTapProperty

        //This value should be /10 to get the real value
    public int value;


    public PoisonDamageProperty(Item item) : base(item)
    {

    }
   
	public override string GetDisplayString()
    {
        string displayString = "";

        
        if (IsMax())
        {
            displayString += "*";
        }

        displayString += "Poison Damage: " + value/10f + "x";

        if(item.refineLevel > 0)
        {
            displayString += " (+" + ((GetTotalValue() - value)/10f).ToString() + ")";
        }

        return displayString;
    }


    public override void Equip(Item item, PlayerStats playerStats)
    {
        playerStats.AddPoisonDamage(GetTotalValue()/10f);
    }

    public override void Unequip(Item item, PlayerStats playerStats)
    {
        playerStats.AddPoisonDamage(-1 * GetTotalValue()/10f);
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
        return Mathf.Ceil((iLvl*10f/120));
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
