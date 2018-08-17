using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBolt : Ability {


    public override void QueueAbility()
    {
        base.QueueAbility();
        //owningPlayer.dealtTapDamageEvent.AddListener((int damage, GameObject target) => { owningPlayer.dealtTapDamageEvent.RemoveListener(ApplyEffects); });

        // damageMultiplierStruct = playerStats.AddDamageMultiplierForTaps(3, 1);

        owningUnit.GetComponent<PlayerPawn>().aboutToAttackEvent.AddListener(ApplyEffects);
    }



    public override void Use(GameObject target)
    {
        base.Use(target);
        //ApplyEffects(target);
        //damageMultiplierStruct = null;
    }

    void ApplyEffects(DamageInfo dmg)
    {
        dmg.damage *= 3;
        dmg.damageCauser = gameObject;
        var debuff = dmg.target.GetComponent<Debuffs>();
        if(debuff != null)
        {
            debuff.AddSlow(0.35f, 5);
        }
        owningUnit.GetComponent<PlayerPawn>().aboutToAttackEvent.RemoveListener(ApplyEffects);

    }
}
