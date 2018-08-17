using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTroll : Enemy {



    protected override void Start()
    {
        base.Start();

        aboutToTakeDamageEvent.AddListener(
            (DamageInfo dmg) =>
            {
                if (dmg.damageCauser.GetComponent<Ability>())
                {
                    dmg.damage = 0;
                }
            }
        );
    }
}
