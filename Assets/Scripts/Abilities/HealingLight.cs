using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingLight : Ability {


	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Use(GameObject target)
    {
        base.Use(target);

        Debug.Log("USING HEAL");
        Health health = target.GetComponent<Health>();
        int heal = (int)(health.maxHealth * 0.3f);
        health.AddHealth(heal);


    }
}
