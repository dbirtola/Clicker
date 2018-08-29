using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedKnuckles : Artifact {



    protected override void Activate()
    {
        base.Activate();

        player.GetComponent<PlayerStats>().AddPersistentDamagePercentIncrease(effectStrength[level-1]);
    }

    protected override void Deactivate()
    {
        base.Deactivate();

        player.GetComponent<PlayerStats>().AddPersistentDamagePercentIncrease(-1 * effectStrength[level-1]);

    }

    public override string GetEffectText(int level)
    {
        if(level == 0)
        {
            return "0%";
        }

        return effectStrength[level-1] * 100 + "%";
    }
}
