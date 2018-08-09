using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySlash : Ability {

    public float tapDamageMultiplier = 15;


    public override void QueueAbility()
    {
        base.QueueAbility();

        damageMultiplierStruct = playerStats.AddDamageMultiplierForTaps(tapDamageMultiplier, 1);
    }

    public override void CancelQueue()
    {
        base.CancelQueue();

    }

    public override void Use(GameObject target)
    {
        base.Use(target);

    }
}
