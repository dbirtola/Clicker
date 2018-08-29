using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineLight : Ability {


    public override void Use(GameObject target)
    {
        base.Use(target);

        var mana = target.GetComponent<Mana>();
        float percent = mana.currentMana / mana.maxMana;
        mana.UseMana(mana.currentMana);

        var health = target.GetComponent<Health>();
        health.AddHealth((int)(health.maxHealth * percent));

        
    }
}
