using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBurn : Ability {
    DamageOverTimeEffect dot;

    public override void Use(GameObject target)
    {
        base.Use(target);

        dot = new DamageOverTimeEffect(target, 50, 0.5f, 10);

        target.GetComponent<Debuffs>().AddDamageOverTimeEffect(dot);
        target.GetComponent<PlayerPawn>().movedSquaresEvent.AddListener(RefreshDamage);
    }

    public void RefreshDamage()
    {
        dot.damage = 50;
    }
}
