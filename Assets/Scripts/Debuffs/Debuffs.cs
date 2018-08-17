using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DamageOverTimeEffect
{
    public GameObject instigator;
    public GameObject target;
    public int damage;
    public float period;
    public float expireTime;
    public float duration;
    public int stacks = 1;
    public bool isActive = false;
    public Sprite graphicSprite;
    public GameObject graphicObject;

    public DamageOverTimeEffect(GameObject target, int damage, float period, float duration)
    {
        this.target = target;
        this.damage = damage;
        this.period = period;
        this.duration = duration;
        this.expireTime = Time.time + duration;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetActive(bool active)
    {
        this.isActive = active;
    }

    public void RefreshDuration()
    {
        expireTime = Time.time + duration;
    }
}

public class SlowEffect
{
    public Coroutine coroutine;

    public float percent;
    public float expireTime;
    
    public SlowEffect(float percent, float duration)
    {
        this.percent = percent;
        this.expireTime = Time.time + duration;
    }

    public float GetRemainingDuration()
    {
        return expireTime - Time.time;
    }

}


public class Debuffs : NetworkBehaviour {
    
    public Health health;
    
    //Keep a list of all damage over time effects in case we need to modify them with other buffs/debuffs at a later time
    List<DamageOverTimeEffect> damageOverTimeEffects;
    List<SlowEffect> slowsEffects;

    static public GameObject debuffGraphicPrefab;

    void Awake()
    {
        health = GetComponent<Health>();
        damageOverTimeEffects = new List<DamageOverTimeEffect>();
        slowsEffects = new List<SlowEffect>();
        debuffGraphicPrefab = Resources.Load("DebuffGraphic") as GameObject;
    }

    /*
    public void OnDestroy()
    {
        RemoveAllDebuffs();
    }
	*/

    public void RemoveAllDebuffs()
    {
        RemoveAllDamageOverTimeEffects();
        RemoveAllSlows();
    }


    /********************
     * Damage over time *
     ********************/

    public void AddDamageOverTimeEffect(GameObject instigator, int damage, float period, float duration)
    {
        CmdAddDamageOverTimeEffect(instigator, damage, period, duration);
    }

    
    public void ServerAddDamageOverTimeEffect(DamageOverTimeEffect dot)
    {
        if (!isServer)
        {
            return;
        }

        damageOverTimeEffects.Add(dot);
        StartCoroutine(StartDOT(dot));

        if (dot.graphicSprite != null)
        {
            dot.graphicObject = Instantiate(debuffGraphicPrefab, dot.target.transform.position, Quaternion.identity);
            dot.graphicObject.transform.SetParent(dot.target.transform);
            dot.graphicObject.GetComponent<SpriteRenderer>().sprite = dot.graphicSprite;
        }

        Debug.Log("Added");
    }

    [Command]
    public void CmdAddDamageOverTimeEffect(GameObject instigator, int damage, float period, float duration)
    {
        DamageOverTimeEffect dot = new DamageOverTimeEffect(gameObject, damage, period, duration);
        dot.instigator = instigator;
        damageOverTimeEffects.Add(dot);
        StartCoroutine(StartDOT(dot));

        if (dot.graphicSprite != null)
        {
            dot.graphicObject = Instantiate(debuffGraphicPrefab, dot.target.transform.position, Quaternion.identity);
            dot.graphicObject.transform.SetParent(dot.target.transform);
            dot.graphicObject.GetComponent<SpriteRenderer>().sprite = dot.graphicSprite;
        }

    }

    IEnumerator StartDOT(DamageOverTimeEffect dot)
    {
        dot.SetActive(true);
        while(dot.expireTime > Time.time)
        {
            //Dont tick on first application, cause the damage numbers will clutter up
            yield return new WaitForSeconds(dot.period);

            health.CmdTakeDamage(dot.instigator, dot.damage * dot.stacks);
        }

        dot.SetActive(false);
        damageOverTimeEffects.Remove(dot);
    }

    public void RemoveAllDamageOverTimeEffects()
    {
        //Sets all damage over time effects to a state which will be cleaned up and removed by their coroutines
        for(int i = 0; i < damageOverTimeEffects.Count; i++)
        {
            damageOverTimeEffects[i].expireTime = Time.time;
            damageOverTimeEffects[i].damage = 0;
            Destroy(damageOverTimeEffects[i].graphicObject);
            damageOverTimeEffects[i].SetActive(false);
        }
    }


    /********************
     *      Slow        *
     ********************/


    public void AddSlow(float percent, float duration)
    {
        SlowEffect slow = new SlowEffect(percent, duration);
        slow.coroutine = StartCoroutine(StartSlowRoutine(slow, duration));
        slowsEffects.Add(slow);


    }

    IEnumerator StartSlowRoutine(SlowEffect slow, float duration)
    {
       // float reducedSpeed = GetComponent<Unit>().currentSpeed * slow.percent;
       // GetComponent<Unit>().currentSpeed += reducedSpeed;

        //Color c = GetComponent<SpriteRenderer>().color;
       // c.b += 100;
       // GetComponent<SpriteRenderer>().color = c;

        yield return new WaitForSeconds(duration);

       // GetComponent<Unit>().currentSpeed -= reducedSpeed;
       // c.b -= 100;
        //GetComponent<SpriteRenderer>().color = c;

        slowsEffects.Remove(slow);
    }

    public void RemoveAllSlows()
    {
        foreach(SlowEffect slow in slowsEffects)
        {
            StopCoroutine(slow.coroutine);
        }
        GetComponent<Unit>().currentSpeed = GetComponent<Unit>().baseSpeed;
    }

    public float GetSlowPercent()
    {
        float slowPercent = 0;
        for(int i = 0; i < slowsEffects.Count; i++)
        {
            if(slowsEffects[i].percent > slowPercent)
            {
                slowPercent = slowsEffects[i].percent;
            }
        }

        return slowPercent;
    }
}
