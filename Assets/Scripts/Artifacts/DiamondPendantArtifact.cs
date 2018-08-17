using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPendantArtifact : Artifact {

    protected override void Activate()
    {
        base.Activate();
        player.GetComponent<PlayerStats>().AddOreRateMultiplier(effectStrength[level - 1]);
    }

    protected override void Deactivate()
    {
        base.Deactivate();
        player.GetComponent<PlayerStats>().AddOreRateMultiplier(effectStrength[level - 1] * -1);

    }

    public override string GetEffectText(int level)
    {
        if(level == 0)
        {
            return "0%";
        }

        return effectStrength[level - 1] + "%";
    }
}
