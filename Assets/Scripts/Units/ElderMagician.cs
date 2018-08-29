using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class ElderMagician : Boss {

    UnityEvent attackFinishedEvent;

    public GameObject fireballPrefab;
    public GameObject missilePrefab;

    public float fireballCastDelay = 1f;
    float fireballSpeed = 4f;

    float missileSpeed = 5f;

    DamageOverTimeEffect burnDot;

    public Sprite fireSprite;

    Coroutine attackRoutine;

    public GameObject orb;
    int orbType;
    int orbHealth; //Takes 15 taps to kill the orb
    int orbMaxHealth = 15;
    Coroutine orbRoutine;

    float fireballDamageMultiplier = 4;
    float missileDamageMultiplier = 3;

    protected override void Awake()
    {
        base.Awake();
        attackFinishedEvent = new UnityEvent();

        if (isServer)
        {
            //After every ability is finished they should invoke the event to trigger the next decision
            attackFinishedEvent.AddListener(()=> { StartCoroutine(MakeNextDecision()); });

        }
    }

    protected override void Start()
    {
        base.Start();



        if (isServer)
        {
            Debug.Log("IS SERVER");
            //Fire off the first decision
            StartCoroutine(MakeNextDecision());

        }


    }


    [Server]
    public IEnumerator MakeNextDecision()
    {
       // Debug.Log("Waiting");
        yield return new WaitForSeconds(baseSpeed);
       // Debug.Log("Deciding");
        //Should probably find a better way to implement this to restart time between attacks after the boss finishes an ability

        
        int decision = Random.Range(0, 100);
        if(decision < 15)
        {
            RpcFireball();

        }else if(decision < 30)
        {
            RpcMissiles();
        }else if(decision < 50)
        {
            if (orb.activeSelf == false)
            {
                RpcSpawnOrb();
            }
            else
            {
                RpcAttack();
            }
        }
        else
        {
            RpcAttack();
        }

        /*
        switch (decision)
        {
            case 0:
                RpcFireball();
                break;
            case 1:
                if (burnDot == null)
                {
                    RpcBurn();
                }else
                {
                    RpcAttack();
                }
                break;
            case 2:
                RpcMissiles();
                break;
            case 3:
                RpcAttack();
                break;
        }
        */
    }

    public override void RpcAttack()
    {
        base.RpcAttack();

        if (isServer)
        {
            FinishedAttack();
        }

    }
    [ClientRpc]
    public void RpcFireball()
    {


        StartCoroutine(Fireball());
    }

    [Server]
    public void FinishedAttack()
    {
        StartCoroutine(MakeNextDecision());

    }

    IEnumerator Fireball()
    {

        DisplayText("Fireball!");
        var fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        var targetSquare = GetRandomPlayerPawn().positionSquare;

        yield return new WaitForSeconds(fireballCastDelay);

        fireball.GetComponent<FireballProjectile>().FireAt(gameObject, targetSquare, fireballSpeed, (int)(damage * fireballDamageMultiplier));
        

        if (isServer)
        {
            //Debug.Log("WAS SERVER");
            FinishedAttack();
        }
    }
    

    IEnumerator BurnRoutine()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1f);
            if(burnDot != null)
            {
                burnDot.damage = (int)(burnDot.damage * 1.5f);
            }
        }
        burnDot = null;


    }

    public void RefreshDamage()
    {
        if(burnDot == null)
        {
            playerPawn.GetComponent<PlayerPawn>().movedSquaresEvent.RemoveListener(RefreshDamage);
            return;
        }
        burnDot.damage = 10;
    }


    [ClientRpc]
    public void RpcBurn()
    {
        DisplayText("Burn!");
        burnDot = new DamageOverTimeEffect(playerPawn.gameObject, 10, 2f, 10);
        burnDot.instigator = gameObject;
        burnDot.graphicSprite = fireSprite;
        playerPawn.GetComponent<Debuffs>().AddDamageOverTimeEffect(burnDot);
        playerPawn.GetComponent<PlayerPawn>().movedSquaresEvent.AddListener(RefreshDamage);
        StartCoroutine(BurnRoutine());


        if (isServer)
        {
            FinishedAttack();
        }

    }


    [ClientRpc]
    public void RpcMissiles()
    {

        DisplayText("Misilles!");
        var missile1 = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        var missile2 = Instantiate(missilePrefab, transform.position, Quaternion.identity);

        var targetSquare = bossFightManager.leftSquare;
        var targetSquare2 = bossFightManager.rightSquare;

        targetSquare.SetColor(Color.red);
        targetSquare2.SetColor(Color.red);

        int missileDamage = (int)(damage * missileDamageMultiplier);
        missile1.GetComponent<WizardMissiles>().FireAt(gameObject, targetSquare, missileSpeed, missileDamage);
        missile2.GetComponent<WizardMissiles>().FireAt(gameObject, targetSquare2, missileSpeed, missileDamage);



        if (isServer)
        {
            FinishedAttack();
        }


    }

    [ClientRpc]
    public void RpcSpawnOrb()
    {
        orb.SetActive(true);
        orbType = Random.Range(0, 3);

        switch (orbType)
        {
            case 0:
                orb.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 1:
                orb.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case 2:
                orb.GetComponent<SpriteRenderer>().color = Color.green;
                break;
        }

        orbHealth = orbMaxHealth;
        orbRoutine = StartCoroutine(OrbRoutine());

        orb.GetComponent<EnemyComponent>().componentHitEvent.RemoveAllListeners(); //bad
        orb.GetComponent<EnemyComponent>().componentHitEvent.AddListener(OrbWasHit);


        if (isServer)
        {
            Debug.Log("Finished orb attack?");
            FinishedAttack();
        }
    }

    IEnumerator OrbRoutine()
    {
        if(orbType == 1)
        {
            Debug.Log("Buffing stats");
            //fireballDamageMu *= 2;
           // missileDamage *= 2;
            damage *= 2;
        }else
        {
            while (true)
            {
                switch (orbType)
                {
                    case 0:
                        Health health = GetComponent<Health>();
                        int healing = health.maxHealth / 50; //Heals 3% max health every second
                        GetComponent<Health>().AddHealth(healing);
                        break;
                    case 1:
                        break;
                    case 2:
                        break;

                }

                yield return new WaitForSeconds(1);
            }
        }


    }

    void OrbWasHit(int damage)
    {
        Debug.Log("Orb was hit for " + damage + " damage");
        orbHealth--;
        if(orbHealth <= 0)
        {
            if (orbType == 1)
            {
                Debug.Log("Lowering damages");
                //fireballDamage /= 2;
                //missileDamage /= 2;
                this.damage /= 2;
            }

            if (orbRoutine != null)
            {

                StopCoroutine(orbRoutine);
                orbRoutine = null;
            }


            orb.SetActive(false);
            

        }
    }
    

    public override void ReceiveAttack(DamageInfo damageInfo, int childHit = -1)
    {

        //Damage reduction
        Debug.Log("Hit child: " + childHit);

        if (childHit == -1)
        {
            health.CmdTakeDamage(damageInfo.instigator, damageInfo.damage);

        }
        if (childHit == 0)
        {
            health.CmdTakeDamage(damageInfo.instigator, damageInfo.damage * 2);
        }


        //Visual feedback
    }

    protected override IEnumerator BehaviourTick()
    {
        yield return null;
        /*
        while (true)
        {

            yield return new WaitForSeconds(currentSpeed);
            //Should probably find a better way to implement this to restart time between attacks after the boss finishes an ability
            if (pauseBehaviorTick)
                continue;

            int decision = Random.Range(0, 3);
            switch (decision)
            {
                case 0:
                    RpcFireball();
                    break;
                case 1:
                    if(burnDot == null)
                    {
                        RpcBurn();
                    }
                    break;
                case 2:
                    RpcMissiles();
                    break;
                case 3:
                    RpcAttack();
                    break;
            }
        }
        */
        
    }
}
