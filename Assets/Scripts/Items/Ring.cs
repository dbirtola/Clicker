using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ring : Equipment {

    public int healthValue;
    
    override public void Awake()
    {
        base.Awake();
        //implicitProperty = new ItemProperty();

        //Going to change to some sort of magic find property maybe?
        implicitProperty = new CriticalChanceProperty(this);

    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Equip()
    {
        base.Equip();
        playerStats.AddBonusHealth(GetTotalHealthValue());
    }

    public override void Unequip()
    {
        base.Unequip();
        playerStats.AddBonusHealth(-1*GetTotalHealthValue());
    }


    
    public int GetTotalHealthValue()
    {
        return healthValue + 20 * refineLevel;
    }

}
