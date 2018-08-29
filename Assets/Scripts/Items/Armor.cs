using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorSlot
{
    Helmet,
    Armor,
    Gloves,
    Boots
}

public class Armor : Item {

    public int armorValue;
   // public int healthValue;

    public ArmorSlot slot;

    override protected void Awake()
    {
        base.Awake();
        //implicitProperty = new ItemProperty();
        implicitProperty = new BonusHealthProperty(this);
        

    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Equip()
    {
        base.Equip();
        playerStats.AddBonusArmor(GetTotalArmorValue());
        //playerStats.AddBonusHealth(healthValue);
    }

    public override void Unequip()
    {
        base.Unequip();
        playerStats.AddBonusArmor(-1 * GetTotalArmorValue());
        //playerStats.AddBonusHealth(-1 * healthValue);
    }

    // Update is called once per frame
    void Update () {
		
	}

    
    public int GetTotalArmorValue()
    {
        return armorValue + 3 * refineLevel;
    }

   // public int GetHealthValue()
    //{
    //    return healthValue;
    //}
}
