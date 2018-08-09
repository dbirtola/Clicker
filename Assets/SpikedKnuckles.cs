using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedKnuckles : Artifact {



    protected override void Activate()
    {
        base.Activate();

        player.GetComponent<PlayerStats>().AddPersistentDamagePercentIncrease(effectStrength[level]);
    }

    protected override void Deactivate()
    {
        base.Deactivate();

        player.GetComponent<PlayerStats>().AddPersistentDamagePercentIncrease(-1 * effectStrength[level]);

    }

    public override string GetEffectText(int level)
    {
        return base.GetEffectText(level) + "%";
    }
}
