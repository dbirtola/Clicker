using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleave : Ability {

    public float damageMultiplier = 5f;

    public override void Use(GameObject target)
    {
        base.Use(target);

        var enemies = FindObjectsOfType<Enemy>();

        foreach(Enemy enemy in enemies)
        {
            int baseDamage = (int)(owningPlayer.playerStats.GetNextTapDamage() * damageMultiplier);
            DamageInfo dmg = new DamageInfo(owningUnit.gameObject, gameObject, enemy.gameObject, baseDamage);
            enemy.ReceiveAttack(dmg);
        }
    }
}
