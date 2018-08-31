using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Weapon : Equipment {

    public int damageValue;
    
    override public void Awake()
    {
        base.Awake();
        //implicitProperty = new ItemProperty();
        implicitProperty = new CriticalChanceProperty(this);

    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Equip()
    {
        base.Equip();
        playerStats.AddBonusDamage(GetTotalDamageValue());
    }

    public override void Unequip()
    {
        base.Unequip();
        playerStats.AddBonusDamage(-1*GetTotalDamageValue());
    }

    public override void SetLevel(int level)
    {
        base.SetLevel(level);
        damageValue = 3 + (int)(1.5f * level);
    }

    public int GetTotalDamageValue()
    {
        return damageValue + 3 * refineLevel;
    }

    

}
