using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceBuff : Buff {

    public float expBonus = 0;

    public Buffs buffs;

    public override void Awake()
    {
        buffText = "Exp";
    }

    public override void ActivateBuff(Unit target, int duration)
    {
        base.ActivateBuff(target, duration);
        buffs = target.GetComponent<Buffs>();

        target.GetOwner().GetComponent<PlayerStats>().AddExperienceMultiplier(expBonus);
        StartCoroutine(ExpireAfterTime(target, duration));
    }

    public override void DeactivateBuff(Unit target)
    {
        base.DeactivateBuff(target);
        target.GetOwner().GetComponent<PlayerStats>().AddExperienceMultiplier(-1 * expBonus);
        buffs.RemoveBuff(this);
    }
}
