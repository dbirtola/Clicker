using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSwing : Ability {

    public float duration = 7;
    public float delay = 0.1f;

    public override void Use(GameObject target)
    {
        base.Use(target);
        // owningPlayer.tappedEnemyEvent.AddListener(tapAgain);
        owningPlayer.playerStats.AddDoubleSwingChance(0.5f);
        StartCoroutine(expire());
    }

    IEnumerator expire()
    {
        yield return new WaitForSeconds(duration);

        owningPlayer.playerStats.AddDoubleSwingChance(-0.5f);
        // owningPlayer.tappedEnemyEvent.RemoveListener(tapAgain);
    }

    /*
    public void tapAgain(GameObject target)
    {
        StartCoroutine(tapAgainRoutine(target));
    }
    */

    //Slight delay so the numbers arent just stacked on eachother. Plus two swings would usually have a very slight delay between them.

}
