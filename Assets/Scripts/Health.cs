using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class DamageInfo
{
    //The unit responsible for the damage
    public GameObject instigator;
    //The specific object dealing the damage
    public GameObject damageCauser;
    public GameObject target;
    public int damage;

    public DamageInfo(GameObject instigator, GameObject causer, GameObject target, int damage)
    {
        this.instigator = instigator;
        this.damageCauser = causer;
        this.target = target;
        this.damage = damage;
    }
}

public class TookDamageEvent : UnityEvent<int>{

}

public class DamageEvent : UnityEvent<DamageInfo>
{

}

public class Health : NetworkBehaviour {

    [SyncVar]
    public int health;
    
    public int maxHealth;

    public bool showDamage = true;
    public FloatingText floatingTextPrefab;

    public DamageEvent aboutToTakeDamageEvent;
    public TookDamageEvent tookDamageEvent;
    

    void Awake()
    {
        tookDamageEvent = new TookDamageEvent();
        floatingTextPrefab = (Resources.Load("FloatingText") as GameObject).GetComponent<FloatingText>();

        tookDamageEvent.AddListener(DisplayDamage);
    }


    public void SetHealth(int health)
    {
        this.health = health;
    }

    public void AddHealth(int health)
    {
        this.health += health;
        if(this.health >= maxHealth)
        {
            this.health = maxHealth;
        }
    }
 
    [Command]
    public void CmdTakeDamage(GameObject instigator, int damage)
    {



        health -= damage;
        // tookDamageEvent.Invoke(damage);
        RpcTookDamage(instigator, damage);

        if(health <= 0)
        {
            //Die
            GetComponent<Unit>().OnDeath(instigator);
        }
    }


    [ClientRpc]
    void RpcTookDamage(GameObject instigator, int damage)
    {
        tookDamageEvent.Invoke(damage);
        instigator.GetComponent<Unit>().dealtDamageEvent.Invoke(gameObject, damage);
    }


    void DisplayDamage(int health)
    {
        // tookDamageEvent.Invoke(this.health - health);


        //this.health = health;

        if (GetComponent<PlayerPawn>())
        {
            return;
        }



       // this.health -= health;
        if(health < 0)
        {
            //GetComponent<Unit>().OnDeath();
        }
    }
}
