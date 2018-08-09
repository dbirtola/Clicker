using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gouge : Ability {



    public override void QueueAbility()
    {
        base.QueueAbility();

        damageMultiplierStruct = playerStats.AddDamageMultiplierForTaps(3, 1);
    }

    public override void CancelQueue()
    {
        base.CancelQueue();

        playerStats.RemoveDamageMultiplier(damageMultiplierStruct);
        damageMultiplierStruct = null;
    }

    public override void Use(GameObject target)
    {
        base.Use(target);

        // owningPlayer.dealtTapDamageEvent.AddListener(ApplyEffects);
        ApplyEffects(target);
        damageMultiplierStruct = null;
       // owningPlayer.dealtTapDamageEvent.AddListener((int damage, GameObject target) => { owningPlayer.dealtTapDamageEvent.RemoveListener(ApplyEffects); });
    }

    void ApplyEffects(GameObject target)
    {
        
        var debuffs = target.GetComponent<Debuffs>();
        if(debuffs != null)
        {
            debuffs.AddDamageOverTimeEffect(owningPawn.gameObject, 5, 0.25f, 10);
        }
    }
}
