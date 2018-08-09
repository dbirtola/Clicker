using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Weapon : Item {

    public int damageValue;
    
    override protected void Awake()
    {
        base.Awake();
        //implicitProperty = new ItemProperty();

    }

    protected override void Start()
    {
        base.Start();
        implicitProperty = new CriticalChanceProperty(this);
    }

    public override void Equip()
    {
        base.Equip();
        playerStats.AddBonusDamage(damageValue);
    }

    public override void Unequip()
    {
        base.Unequip();
        playerStats.AddBonusDamage(-1*damageValue);
    }


    
    public int GetDamageValue()
    {
        return damageValue;
    }

}
