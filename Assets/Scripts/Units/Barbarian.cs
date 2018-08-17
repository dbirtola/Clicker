using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class Barbarian : Enemy {

    protected override void Start()
    {
        base.Start();

        health.tookDamageEvent.AddListener(OnTookDamage);

        
    }

    public override void SetLevel(int level)
    {
        base.SetLevel(level);
        health.maxHealth = (int)Mathf.Ceil(50 + 8 * level * Mathf.Pow(1.1f, level));
        health.health = health.maxHealth;
        damage = (int)Mathf.Ceil(5 + 1.2f * level);
        experience = (int)Mathf.Ceil(health.maxHealth / 10 + (health.maxHealth / 5) * Mathf.Pow(1.02f, level));

    }


    void OnTookDamage(int damage)
    {
        if(health.health != 0)
        {
            currentSpeed = baseSpeed / (health.maxHealth / health.health);
        }
    }

    /*

    [ClientRpc]
    public override void RpcProcessDeath()
    {

        Debug.LogError("Processing Death for: " + unitName);
        FindObjectOfType<PlayerStats>().AddExperience(45);
        FindObjectOfType<PlayerInventory>().addItem(FindObjectOfType<ItemFactory>().GetItemDrop(level));

        Destroy(gameObject);
    }

    */
}
