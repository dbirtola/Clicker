using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyToTheTowerArtifact : Artifact {

    protected override void Activate()
    {
        base.Activate();
        player.GetComponent<PlayerStats>().AddTowerStartBonus((int)effectStrength[level - 1]);
    }

    protected override void Deactivate()
    {
        base.Deactivate();
        player.GetComponent<PlayerStats>().AddTowerStartBonus((int)effectStrength[level - 1] * -1);
    }

    public override string GetEffectText(int level)
    {
        return base.GetEffectText(level);
    }
}
