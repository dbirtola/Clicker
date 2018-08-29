using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchDoctor : Enemy {

    DamageOverTimeEffect dot;

    //Meant to be the basic attack, will cause default animation to be used
    public override void RpcAttack()
    {
        foreach (var player in FindObjectsOfType<PlayerPawn>())
        {
            player.GetComponent<Unit>().ReceiveAttack(new DamageInfo(gameObject, gameObject, player.gameObject, damage));
            if(dot == null)
            {
                dot = new DamageOverTimeEffect(player.gameObject, 2, 1.5f, 10);
                dot.instigator = gameObject;
                player.GetComponent<Debuffs>().AddDamageOverTimeEffect(dot);
            }else
            {
                dot.stacks++;
                dot.RefreshDuration();
            }

        }
    }
}
