using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class KillerBee : Enemy{
    

    public EnemyComponent weakSpot;
    public int weakSpotDamageMultiplier = 2;
    public float moveSpeed = 1;
	// Use this for initialization
	protected override void Awake () {
        base.Awake();
        
        weakSpot = GetComponentInChildren<EnemyComponent>();
	}

    /*
    public override void SetLevel(int level)
    {
        base.SetLevel(level);
        health.maxHealth = (int)(Mathf.Ceil(50 + 7 * level * Mathf.Pow(1.1f, level))*1.8);
        health.health = health.maxHealth;
        damage = (int)Mathf.Ceil(4 + 1.5f *level);
        experience = (int)Mathf.Ceil(health.maxHealth / 9 + (health.maxHealth / 5) * Mathf.Pow(1.02f, level));

    }
    */

    protected override void Start()
    {
        base.Start();
       // weakSpot.componentHitEvent.AddListener(WeakSpotHit);
    }

    /*
    public void WeakSpotHit(int damage)
    {
        Debug.Log("Hit weak spot!");
        health.ReceiveAttack(damage * 2);
    }
    */


    protected override void Update()
    {
        
        base.Update();
        float outerRadius = 5f;
        float innerRadius = 2.5f;
        float c = 1;
        float x = (outerRadius - innerRadius) * Mathf.Cos(Time.time) + c * Mathf.Cos((outerRadius / innerRadius - 1) * Time.time);
        float y = (outerRadius - innerRadius) * Mathf.Sin(Time.time) - c * Mathf.Sin((outerRadius / innerRadius - 1) * Time.time);

        float a = 2.5f;
        float b = 10f;
        c = 0.5f ;
        float n = (2 / 3f);
        a = 2 * c * n / (n + 1);
        b = ((n - 1) * c) / (n + 1);
        x = (a - b) * Mathf.Cos(Time.time* 1.5f) + c * Mathf.Cos((a - b) / b * Time.time * 1.5f);
        y = (a - b) * Mathf.Sin(Time.time * 1.5f) - c * Mathf.Sin((a - b) / b * Time.time * 1.5f);

        transform.position += new Vector3(x, y, 0) * Time.deltaTime * moveSpeed;

    }

    /*
    public override void ReceiveAttack(GameObject instigator, int damage, int childHit = -1)
    {

        //Damage reduction


        if (childHit == -1)
        {
            health.CmdTakeDamage(instigator, damage);

        }
        if(childHit == 0)
        {
            health.CmdTakeDamage(instigator, damage * weakSpotDamageMultiplier);
        }


        //Visual feedback
    }
    */


    public override void ReceiveAttack(DamageInfo damageInfo, int childHit = -1)
    {

        //Damage reduction


        if (childHit == -1)
        {
            health.CmdTakeDamage(damageInfo.instigator, damageInfo.damage);

        }
        if (childHit == 0)
        {
            health.CmdTakeDamage(damageInfo.instigator, damageInfo.damage * weakSpotDamageMultiplier);
        }


        //Visual feedback
    }

    /*
    [ClientRpc]
    public override void RpcProcessDeath()
    {

        
        Debug.LogError("Processing Death for: " + unitName);
       
        FindObjectOfType<PlayerStats>().AddExperience(20);
        FindObjectOfType<PlayerInventory>().addItem(FindObjectOfType<ItemFactory>().GetItemDrop(level));

        Destroy(gameObject);

        
    }*/

}
