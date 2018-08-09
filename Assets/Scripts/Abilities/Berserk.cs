using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : Ability {

    public float damageMultiplier = 1.5f;
    public float duration = 10f;


    public override void Use(GameObject target)
    {
        base.Use(target);
        Health health = target.GetComponent<Health>();
        int currentHealth = health.health;
        health.SetHealth(currentHealth / 2);
        playerStats.AddDamageMultiplierForDuration(damageMultiplier, duration);

    }
}
