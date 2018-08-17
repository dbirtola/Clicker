using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class Slime: Enemy{



    public override void SetLevel(int level)
    {
        base.SetLevel(level);
        health.maxHealth = (int)Mathf.Ceil(50 + 7 * level * Mathf.Pow(1.1f, level));
        health.health = health.maxHealth;
        damage = 3 + level;
        //exp = health/10 + (health /5)*1.02^level
        experience = (int)Mathf.Ceil(health.maxHealth / 10 + (health.maxHealth / 5) * Mathf.Pow(1.02f, level));


    }

}
