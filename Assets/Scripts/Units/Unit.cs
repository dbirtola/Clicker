using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;



public class UnitDiedEvent : UnityEvent<Unit>
{

}


public class DealtDamageEvent : UnityEvent<GameObject, int>
{

}

public class Unit : NetworkBehaviour {

    public string unitName;
    protected Health health;
    protected Debuffs debuffs;

    //Current speed is the monsters desired speed. To Factor in slows, use GetEffectiveSpeed();
    public float currentSpeed;
    //Base speed is the speed
    public float baseSpeed = 1;

    public float slowPercent = 0;

    public UnitDiedEvent unitDied;
    public DamageEvent aboutToTakeDamageEvent;
    
    //the dealt damage event is invoked after damage reductions to attribute a total number of damage to a specific unit,
    //typically by the health script.
    public DealtDamageEvent dealtDamageEvent { get; private set; }

    protected virtual void Start()
    {

    }

    protected virtual void Awake()
    {
        unitDied = new UnitDiedEvent();
        aboutToTakeDamageEvent = new DamageEvent();
        dealtDamageEvent = new DealtDamageEvent();
        health = GetComponent<Health>();
        debuffs = GetComponent<Debuffs>();

        currentSpeed = baseSpeed;
    }
    
    virtual public void OnDeath(GameObject killer)
    {


        unitDied.Invoke(this);
        RpcProcessDeath();
    }

    /*
    //Receives an attack and performs damage reduction and visual feedback
    //Childhit = -1 means the base object is hit, else it refers to the index of the child hit
    public virtual void ReceiveAttack(GameObject instigator, int damage, int childHit = -1)
    {
        //Damage reduction

        if(childHit == -1)
        {
            health.CmdTakeDamage(instigator, damage);

        }


        //Visual feedback
    }
    */


    //Receives an attack and performs damage reduction and visual feedback
    //Childhit = -1 means the base object is hit, else it refers to the index of the child hit
    public virtual void ReceiveAttack(DamageInfo damageInfo, int childHit = -1)
    {
        //Damage reduction

        if (childHit == -1)
        {
            aboutToTakeDamageEvent.Invoke(damageInfo);
            health.CmdTakeDamage(damageInfo.instigator, damageInfo.damage);

        }


        //Visual feedback
    }

    [ClientRpc]
    public virtual void RpcProcessDeath()
    {
    }

    /*
    public float GetEffectiveSpeed()
    {
        return currentSpeed - (currentSpeed * GetComponent<Debuffs>().GetSlowPercent());
    }
    */

    public float GetEffectiveSpeed()
    {
        float slowPercent = 0;
        if (debuffs != null)
        {
            slowPercent = debuffs.GetSlowPercent();
        }

        return currentSpeed + (currentSpeed * slowPercent);
    }


}
