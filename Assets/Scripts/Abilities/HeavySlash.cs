using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySlash : Ability {

    public float tapDamageMultiplier = 15;


    public override void QueueAbility()
    {
        base.QueueAbility();

        owningUnit.GetComponent<PlayerPawn>().aboutToAttackEvent.AddListener(OnAttack);
        //damageMultiplierStruct = playerStats.AddDamageMultiplierForTaps(tapDamageMultiplier, 1);
    }

    public override void CancelQueue()
    {
        base.CancelQueue();

    }

    public void OnAttack(DamageInfo dmg)
    {
        dmg.damage *= 15;
        dmg.damageCauser = gameObject;
        owningUnit.GetComponent<PlayerPawn>().aboutToAttackEvent.RemoveListener(OnAttack);

    }

    public override void Use(GameObject target)
    {
        base.Use(target);

    }

    
}
