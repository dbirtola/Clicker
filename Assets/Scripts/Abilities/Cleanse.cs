using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanse : Ability {


    public override void Use(GameObject target)
    {
        base.Use(target);
        target.GetComponent<Debuffs>().RemoveAllDebuffs();
    }
}
