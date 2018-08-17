using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;


public class DealtTapDamageEvent : UnityEvent<int, GameObject>
{

}

public class TappedEnemyEvent : UnityEvent<GameObject>
{

}

public class KilledEnemyEvent : UnityEvent<Enemy>
{

}

public class Player : NetworkBehaviour {

    //Will probably deprecate in favor of tappedEnemyEvent
    public DealtTapDamageEvent dealtTapDamageEvent;

    public TappedEnemyEvent tappedEnemyEvent;

    public KilledEnemyEvent killedEnemyEvent;

    static Player playerController = null;
    public PlayerPawn pPawn;

    public PlayerStats playerStats;

    void Awake()
    {
        dealtTapDamageEvent = new DealtTapDamageEvent();
        tappedEnemyEvent = new TappedEnemyEvent();
        killedEnemyEvent = new KilledEnemyEvent();


        playerStats = GetComponent<PlayerStats>();

        if(playerController != null)
        {
            Destroy(gameObject);
        }else
        {
            playerController = this;
        }

    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


            //Sends the hit command with the index of the child hit. Cannot simply send the child hit since only the base networked
            //object should be used.

            if(hit.collider != null) 
            {
                //The attack function can disregard whether it hit the enemies base object or an enemyComponent.
                //It is up to the enemy component to relay the attack information to the base object.
                Attack(hit.collider.gameObject);

                float tapAgainChance = playerStats.GetDoubleAttackChance();

                //The double swing effect is used by multiple items. If each one tries to handle the logic on their own using the event,
                //either one hit will be able to trigger 2-4 attacks or things will get messy trying to avoid that, so the logic for it is just
                //included here
                if(Random.Range(0, 1f) < tapAgainChance)
                {
                    StartCoroutine(attackAgainRoutine(hit.collider.gameObject));
                }

                //Check to see if we clicked the enemy itself or a component of that enemy
                //The event is meant to only care about the enemy hit, not which component was hit.
                EnemyComponent ec = hit.collider.gameObject.GetComponent<EnemyComponent>();
                if(ec != null) { 

                    tappedEnemyEvent.Invoke(ec.baseObject);
                }else
                {
                    tappedEnemyEvent.Invoke(hit.collider.gameObject);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.O)) 
        {
            ItemFactory iFactory = FindObjectOfType<ItemFactory>();

            GetComponent<PlayerInventory>().PickUpItem(iFactory.GetItemDrop(0));
        }
	}

    IEnumerator attackAgainRoutine(GameObject target)
    {
        yield return new WaitForSeconds(0.1f);

        //Necessary check to make sure the target hasnt died in the last 0.1 seconds
        if(target != null)
        {
            Attack(target);
        }
    }

    public void Attack(GameObject target)
    {
        int damage = playerStats.GetNextTapDamage();

        //Might want to move crit logic on to the player pawn since it makes more sense
        float critChance = playerStats.GetCriticalChance();
        bool crit = Random.Range(0, 100) < critChance;
        if (crit)
        {
            Debug.Log("Adding: " + damage * playerStats.GetCriticalDamage());
            damage += (int)(damage * playerStats.GetCriticalDamage());
        }
        int childIndex = -1;
        if (target.GetComponent<EnemyComponent>())
        {
            childIndex = target.GetComponent<EnemyComponent>().childIndex;
            pPawn.CmdAttack(target.GetComponent<EnemyComponent>().baseObject, childIndex, damage);
        }else
        {
            pPawn.CmdAttack(target, childIndex, damage);
        }

        dealtTapDamageEvent.Invoke(damage, target);
        float manaPerTap = GetComponent<PlayerStats>().GetTotalStatStruct().manaPerTap;
        GetPlayerPawn().GetComponent<Mana>().GainMana(manaPerTap);
    }

    public void SetPlayerPawn(PlayerPawn playerPawn)
    {
        pPawn = playerPawn;
        playerPawn.CmdInitializeStats(GetComponent<PlayerStats>().GetTotalStatStruct());
        
    }



    public PlayerPawn GetPlayerPawn()
    {
        return pPawn;
    }



    
}
