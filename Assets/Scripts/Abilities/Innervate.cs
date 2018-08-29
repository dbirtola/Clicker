using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Innervate : Ability {

    public float duration = 10f;

    float mptGained;
    float damageTaken;

    public override void Use(GameObject target)
    {
        base.Use(target);

        var playerStats = owningPlayer.playerStats;
        mptGained = playerStats.GetTotalStatStruct().manaPerTap;
        damageTaken = playerStats.GetTotalStatStruct().damage * 0.5f;

        playerStats.AddManaPerTap(mptGained);
        playerStats.AddBonusDamage((int)(-1 * damageTaken));

        StartCoroutine(expire());
    }

    IEnumerator expire()
    {
        yield return new WaitForSeconds(duration);

        owningPlayer.playerStats.AddManaPerTap(-1 * mptGained);
        owningPlayer.playerStats.AddBonusDamage((int)(damageTaken));

        // owningPlayer.tappedEnemyEvent.RemoveListener(tapAgain);
    }

}
